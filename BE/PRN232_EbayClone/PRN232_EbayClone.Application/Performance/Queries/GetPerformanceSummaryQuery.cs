using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using TrafficSnapshot = PRN232_EbayClone.Application.Performance.Queries.TrafficMetricCalculator.TrafficSnapshot;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceSummaryQuery(Guid SellerId, string? Period) : IQuery<PerformanceSummaryDto>;

public sealed class GetPerformanceSummaryQueryHandler(
    IUserRepository UserRepository,
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetPerformanceSummaryQuery, PerformanceSummaryDto>
{
    private static readonly HashSet<string> PaidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        OrderStatusCodes.PaidAndShipped,
        OrderStatusCodes.PaidAwaitingFeedback,
        OrderStatusCodes.ShippedAwaitingFeedback
    };

    public async Task<Result<PerformanceSummaryDto>> Handle(GetPerformanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var user = await UserRepository.GetByIdAsync(sellerId, cancellationToken);
        if (user is null)
        {
            return Error.Failure("Performance.Summary.UserNotFound", "Seller not found.");
        }

        var nowUtc = DateTime.UtcNow;
        var selectedPeriodKey = NormalizePeriodKey(request.Period);
        var salesPeriods = BuildSalesPeriods(nowUtc);
        var earliestStart = salesPeriods.Min(static period => period.RangeStart);

        var overviewRecords = await PerformanceRepository.GetOverviewRecordsAsync(
            request.SellerId,
            ToUtcDateTime(earliestStart, TimeOnly.MinValue),
            cancellationToken);

        var paidOrders = overviewRecords
            .Where(record => PaidStatuses.Contains(record.StatusCode))
            .Select(record => (DateOnly.FromDateTime((record.PaidAt ?? record.OrderedAt).Date), record.TotalAmount))
            .ToList();

        var currency = overviewRecords.FirstOrDefault()?.Currency ?? "USD";
        var salesPeriodsDto = salesPeriods
            .Select(period => new PerformanceSalesPeriodDto(
                period.Key,
                period.Label,
                CalculateTotal(paidOrders, period.RangeStart, period.RangeEnd)))
            .ToList();

        var selectedPeriod = salesPeriods.FirstOrDefault(period => string.Equals(period.Key, selectedPeriodKey, StringComparison.OrdinalIgnoreCase));
        if (selectedPeriod == default)
        {
            selectedPeriod = salesPeriods.First(static period => period.Key == "last_31_days");
        }

        var comparisonRange = BuildComparisonRange(selectedPeriod);

        var currentRangeStartUtc = ToUtcDateTime(selectedPeriod.RangeStart, TimeOnly.MinValue);
        var currentRangeEndUtc = ToUtcDateTime(selectedPeriod.RangeEnd, TimeOnly.MaxValue);
        var previousRangeStartUtc = ToUtcDateTime(comparisonRange.Start, TimeOnly.MinValue);
        var previousRangeEndUtc = ToUtcDateTime(comparisonRange.End, TimeOnly.MaxValue);

        var currentTrafficAggregate = await PerformanceRepository.GetTrafficAggregateAsync(
            request.SellerId,
            currentRangeStartUtc,
            currentRangeEndUtc,
            cancellationToken);

        var previousTrafficAggregate = await PerformanceRepository.GetTrafficAggregateAsync(
            request.SellerId,
            previousRangeStartUtc,
            previousRangeEndUtc,
            cancellationToken);

        var currentTrafficSnapshot = TrafficMetricCalculator.FromAggregate(currentTrafficAggregate);
        var previousTrafficSnapshot = TrafficMetricCalculator.FromAggregate(previousTrafficAggregate);

        var selectedTotal = CalculateTotal(paidOrders, selectedPeriod.RangeStart, selectedPeriod.RangeEnd);
        var paymentRecords = await PerformanceRepository.GetPaymentRecordsAsync(
            request.SellerId,
            currentRangeStartUtc,
            currentRangeEndUtc,
            cancellationToken);

        var currentPayments = FilterPaymentsByPaidDate(paymentRecords, currentRangeStartUtc, currentRangeEndUtc);
        currency = ResolveCurrency(currency, currentPayments);

        var salesSummary = BuildSalesSummary(selectedPeriod, salesPeriodsDto, currency, nowUtc);
        var sellingCosts = BuildSellingCosts(currentPayments, currency);
        var traffic = BuildTrafficMetrics(currentTrafficSnapshot, previousTrafficSnapshot);
        var sellerLevelRecord = await PerformanceRepository.GetSellerLevelAsync(request.SellerId, nowUtc, cancellationToken);
        var sellerLevel = BuildSellerLevel(sellerLevelRecord ?? new PerformanceSellerLevelRecord(
            Region: "US",
            CurrentLevel: user.PerformanceLevel.Name,
            EvaluatedTodayLevel: user.PerformanceLevel.Name,
            TransactionDefectRate: 0m,
            LateShipmentRate: 0m,
            TrackingUploadedOnTimeRate: 0m,
            CasesClosedWithoutSellerResolution: 0,
            TransactionsLast12Months: 0,
            SalesLast12Months: 0m,
            Currency: currency,
            NextEvaluationDate: DateOnly.FromDateTime(nowUtc.Date)));

        var summary = new PerformanceSummaryDto(salesSummary, sellingCosts, traffic, sellerLevel);
        return Result.Success(summary);
    }

    private static string NormalizePeriodKey(string? period)
    {
        if (string.IsNullOrWhiteSpace(period))
        {
            return "last_31_days";
        }

        return period.Trim().ToLowerInvariant();
    }

    private static decimal CalculateTotal(IReadOnlyList<(DateOnly PaidDate, decimal TotalAmount)> orders, DateOnly from, DateOnly to)
    {
        // TotalAmount ở đây phải là: (SubTotal - Discount) + ShippingCost + TaxAmount
        // Đảm bảo PerformanceOverviewRecord.TotalAmount được tính đúng từ database
        return orders
            .Where(order => order.PaidDate >= from && order.PaidDate <= to)
            .Sum(order => Math.Round(order.TotalAmount, 2, MidpointRounding.AwayFromZero));
    }

    private static PerformanceSalesSummaryDto BuildSalesSummary(
        (string Key, string Label, DateOnly RangeStart, DateOnly RangeEnd) selectedPeriod,
        IReadOnlyList<PerformanceSalesPeriodDto> periods,
        string currency,
        DateTime nowUtc)
    {
        var caption = $"Chart for sales data across {selectedPeriod.Label}";
        var note = $"Data for {FormatDate(selectedPeriod.RangeStart)} - {FormatDate(selectedPeriod.RangeEnd)} as of {FormatTimestamp(nowUtc)} UTC. " +
                   $"Total sales = (Item price - Discounts) + Buyer-paid shipping + Taxes collected. Currency: {currency}.";

        return new PerformanceSalesSummaryDto(caption, note, periods);
    }

    private static (DateOnly Start, DateOnly End) BuildComparisonRange((string Key, string Label, DateOnly RangeStart, DateOnly RangeEnd) selectedPeriod)
    {
        if (string.Equals(selectedPeriod.Key, "this_year", StringComparison.OrdinalIgnoreCase))
        {
            return (selectedPeriod.RangeStart.AddYears(-1), selectedPeriod.RangeEnd.AddYears(-1));
        }

        var length = selectedPeriod.RangeEnd.DayNumber - selectedPeriod.RangeStart.DayNumber + 1;
        var compareEnd = selectedPeriod.RangeStart.AddDays(-1);
        var compareStart = compareEnd.AddDays(-(length - 1));
        return (compareStart, compareEnd);
    }

    private static IReadOnlyList<PerformanceSellingCostDto> BuildSellingCosts(
        IReadOnlyCollection<PerformancePaymentRecord> payments,
        string currency)
    {
        if (payments.Count == 0)
        {
            return new List<PerformanceSellingCostDto>
            {
                new("total-sales", "Total sales", 0m, true, null),
                new("taxes", "Taxes and government fees", 0m, false, null),
                new("ebay-fees", "eBay fees", 0m, false, null),
                new("shipping-labels", "Shipping labels", 0m, false, null),
                new("discounts", "Seller-funded discounts", 0m, false, null),
                new("net-sales", "Net sales", 0m, true, null)
            };
        }

        decimal Sum(Func<PerformancePaymentRecord, decimal> selector)
            => payments.Sum(selector);

        // Total Sales = (SubTotal - Discount) + ShippingCost + TaxAmount
        // Đây là tổng doanh thu thô bao gồm shipping và tax mà buyer trả
        var subTotalAmount = Math.Round(Sum(p => p.SubTotalAmount), 2, MidpointRounding.AwayFromZero);
        var discounts = Math.Round(Sum(p => p.DiscountAmount), 2, MidpointRounding.AwayFromZero);
        var shippingCost = Math.Round(Sum(p => p.ShippingAmount), 2, MidpointRounding.AwayFromZero);
        var taxes = Math.Round(Sum(p => p.TaxAmount), 2, MidpointRounding.AwayFromZero);
        
        var totalSales = Math.Round((subTotalAmount - discounts) + shippingCost + taxes, 2, MidpointRounding.AwayFromZero);
        
        var platformFees = Math.Round(Sum(p => p.PlatformFeeAmount), 2, MidpointRounding.AwayFromZero);
        var shippingLabels = Math.Round(Sum(p => p.ShippingLabelAmount), 2, MidpointRounding.AwayFromZero);

        // Net Sales = Total Sales - Taxes - Platform Fees - Shipping Labels
        var netSales = Math.Round(totalSales - taxes - platformFees - shippingLabels, 2, MidpointRounding.AwayFromZero);

        return new List<PerformanceSellingCostDto>
        {
            new("total-sales", "Total sales", totalSales, true, null),
            new("taxes", "Taxes and government fees", taxes, false, null),
            new("ebay-fees", "eBay fees", platformFees, false, null),
            new("shipping-labels", "Shipping labels", shippingLabels, false, null),
            new("discounts", "Seller-funded discounts", discounts, false, null),
            new("net-sales", "Net sales", netSales, true, null)
        };
    }

    private static IReadOnlyList<PerformanceTrafficMetricDto> BuildTrafficMetrics(TrafficSnapshot current, TrafficSnapshot previous)
    {
        const string changeSuffix = "vs prior period";

        return new List<PerformanceTrafficMetricDto>
        {
            new("listing-impressions", "Listing impressions", FormatNumber(current.Impressions), $"{FormatPercentageChange(current.Impressions, previous.Impressions)} {changeSuffix}"),
            new("click-through", "Click-through rate", FormatRate(current.ClickThroughRate), $"{FormatPercentageChange(current.ClickThroughRate, previous.ClickThroughRate)} {changeSuffix}"),
            new("listing-views", "Listing page views", FormatNumber(current.ListingViews), $"{FormatPercentageChange(current.ListingViews, previous.ListingViews)} {changeSuffix}"),
            new("sales-conversion", "Sales conversion rate", FormatRate(current.ConversionRate), $"{FormatPercentageChange(current.ConversionRate, previous.ConversionRate)} {changeSuffix}")
        };
    }

    private static string FormatNumber(int value)
        => value.ToString("N0", CultureInfo.InvariantCulture);

    private static string FormatRate(decimal percentage)
    {
        var rounded = Math.Round(percentage, 1, MidpointRounding.AwayFromZero);
        return $"{rounded:0.0}%";
    }

    private static string FormatPercentageChange(int current, int previous)
    {
        if (previous == 0)
        {
            return current == 0 ? "0.0%" : "N/A";
        }

        var change = (current - previous) / (decimal)previous * 100m;
        var rounded = Math.Round(change, 1, MidpointRounding.AwayFromZero);
        return $"{(rounded >= 0 ? "+" : string.Empty)}{rounded}%";
    }

    private static string FormatPercentageChange(decimal current, decimal previous)
    {
        if (previous == 0m)
        {
            return current == 0m ? "0.0%" : "N/A";
        }

        var change = (current - previous) / previous * 100m;
        var rounded = Math.Round(change, 1, MidpointRounding.AwayFromZero);
        return $"{(rounded >= 0 ? "+" : string.Empty)}{rounded}%";
    }

    private static PerformanceSellerLevelDto BuildSellerLevel(PerformanceSellerLevelRecord record)
    {
        var title = $"Seller level (Region: {record.Region})";
        var message = record.CurrentLevel switch
        {
            var level when string.Equals(level, SellerPerformanceLevel.TopRated.Name, StringComparison.OrdinalIgnoreCase)
                => "You are currently Top Rated. Maintain excellent service to keep your benefits.",
            var level when string.Equals(level, SellerPerformanceLevel.AboveStandard.Name, StringComparison.OrdinalIgnoreCase)
                => "You are Above Standard. Focus on defect and shipment metrics to reach Top Rated status.",
            _ when record.TransactionsLast12Months == 0
                => "We are still collecting enough transactions to evaluate your seller level.",
            _
                => "Your recent performance is below standard. Review your defect and shipping metrics to improve before the next evaluation."
        };

        return new PerformanceSellerLevelDto(title, message);
    }

    private static IReadOnlyList<PerformancePaymentRecord> FilterPaymentsByPaidDate(
        IEnumerable<PerformancePaymentRecord> payments,
        DateTime startUtc,
        DateTime endUtc)
    {
        return payments
            .Where(payment =>
            {
                var effectiveDate = payment.PaidAtUtc ?? payment.OrderedAtUtc;
                return effectiveDate >= startUtc && effectiveDate <= endUtc;
            })
            .ToList();
    }

    private static string ResolveCurrency(string currentCurrency, IReadOnlyCollection<PerformancePaymentRecord> payments)
    {
        if (payments.Count == 0)
        {
            return currentCurrency ?? "USD";
        }

        return payments
            .Select(payment => payment.Currency)
            .FirstOrDefault(c => !string.IsNullOrWhiteSpace(c))
            ?? currentCurrency
            ?? "USD";
    }

    private static DateTime ToUtcDateTime(DateOnly date, TimeOnly time)
        => DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Utc);

    private static string FormatDate(DateOnly value) => value.ToString("MMM d, yyyy", CultureInfo.InvariantCulture);

    private static string FormatTimestamp(DateTime timestamp) => timestamp.ToString("MMM d 'at' h:mmtt", CultureInfo.InvariantCulture);

    private static IReadOnlyList<(string Key, string Label, DateOnly RangeStart, DateOnly RangeEnd)> BuildSalesPeriods(DateTime nowUtc)
    {
        var today = DateOnly.FromDateTime(nowUtc.Date);
        var startOfYear = new DateOnly(today.Year, 1, 1);

        return new List<(string, string, DateOnly, DateOnly)>
        {
            ("today", "Today", today, today),
            ("last_7_days", "Last 7 days", today.AddDays(-6), today),
            ("last_31_days", "Last 31 days", today.AddDays(-30), today),
            ("last_90_days", "Last 90 days", today.AddDays(-89), today),
            ("this_year", "This year", startOfYear, today)
        };
    }
}
