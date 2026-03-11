using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceSalesReportQuery(Guid SellerId, string? Period, string? Compare) : IQuery<PerformanceSalesReportDto>;

public sealed class GetPerformanceSalesReportQueryHandler(
    IUserRepository UserRepository,
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetPerformanceSalesReportQuery, PerformanceSalesReportDto>
{
    public async Task<Result<PerformanceSalesReportDto>> Handle(GetPerformanceSalesReportQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var user = await UserRepository.GetByIdAsync(sellerId, cancellationToken);
        if (user is null)
        {
            return Error.Failure("Performance.Sales.UserNotFound", "Seller not found.");
        }

        var nowUtc = DateTime.UtcNow;
        var normalizedPeriod = NormalizePeriod(request.Period);
        var primaryRange = BuildPrimaryRange(normalizedPeriod, nowUtc);

        var normalizedCompare = NormalizeCompare(request.Compare);
        var compareRange = BuildCompareRange(primaryRange, normalizedCompare);

        var paymentRecords = await PerformanceRepository.GetPaymentRecordsAsync(
            request.SellerId,
            ToUtcDateTime(primaryRange.Start, TimeOnly.MinValue),
            ToUtcDateTime(primaryRange.End, TimeOnly.MaxValue),
            cancellationToken);

        var currentPayments = FilterPayments(paymentRecords, primaryRange.Start, primaryRange.End);

        IReadOnlyList<PerformancePaymentRecord> comparePayments = Array.Empty<PerformancePaymentRecord>();
        if (compareRange.HasValue)
        {
            var comparisonPayments = await PerformanceRepository.GetPaymentRecordsAsync(
                request.SellerId,
                ToUtcDateTime(compareRange.Value.Start, TimeOnly.MinValue),
                ToUtcDateTime(compareRange.Value.End, TimeOnly.MaxValue),
                cancellationToken);

            comparePayments = FilterPayments(comparisonPayments, compareRange.Value.Start, compareRange.Value.End);
        }

        var (totalBuyers, oneTimeBuyers, repeatBuyers) = CalculateBuyerSegments(currentPayments);
        var (previousBuyerCount, _, _) = CalculateBuyerSegments(comparePayments);

        var percentChange = compareRange.HasValue
            ? FormatPercentageChange(totalBuyers, previousBuyerCount)
            : "N/A";
        var percentOfTotal = totalBuyers == 0
            ? "0.0%"
            : $"{Math.Round(repeatBuyers / (decimal)totalBuyers * 100m, 1, MidpointRounding.AwayFromZero)}%";

        var compareLabel = compareRange.HasValue
            ? FormatRange(compareRange.Value.Start, compareRange.Value.End, primaryRange.LengthInDays)
            : "No comparison selected";

        var report = new PerformanceSalesReportDto(
            UpdatedAt(nowUtc),
            FormatRange(primaryRange.Start, primaryRange.End),
            compareLabel,
            new PerformanceBuyerInsightsDto(totalBuyers, percentChange, oneTimeBuyers, repeatBuyers, percentOfTotal),
            "We couldn't find any sales or selling costs in the selected time period. Try changing the date range or filters.",
            "We couldn't find any sales in the selected time period. Try changing the date range or filters."
        );

        return Result.Success(report);
    }

    private static string NormalizePeriod(string? period)
    {
        if (string.IsNullOrWhiteSpace(period))
        {
            return "last_31_days";
        }

        return period.Trim().ToLowerInvariant() switch
        {
            "today" => "today",
            "last_7_days" => "last_7_days",
            "last_31_days" => "last_31_days",
            "last_90_days" => "last_90_days",
            "this_year" => "this_year",
            "current_month" => "current_month",
            _ => "last_31_days"
        };
    }

    private static string NormalizeCompare(string? compare)
    {
        if (string.IsNullOrWhiteSpace(compare))
        {
            return "previous_period";
        }

        return compare.Trim().ToLowerInvariant() switch
        {
            "previous_period" => "previous_period",
            "prior_period" => "previous_period",
            "prior_months" => "previous_period",
            "previous_year" => "previous_year",
            "prior_year" => "previous_year",
            "none" => "none",
            _ => "previous_period"
        };
    }

    private static IReadOnlyList<PerformancePaymentRecord> FilterPayments(
        IEnumerable<PerformancePaymentRecord> payments,
        DateOnly rangeStart,
        DateOnly rangeEnd)
    {
        var startUtc = ToUtcDateTime(rangeStart, TimeOnly.MinValue);
        var endUtc = ToUtcDateTime(rangeEnd, TimeOnly.MaxValue);

        return payments
            .Where(payment =>
            {
                var effectiveDate = payment.PaidAtUtc ?? payment.OrderedAtUtc;
                return effectiveDate >= startUtc && effectiveDate <= endUtc;
            })
            .ToList();
    }

    private static (int TotalBuyers, int OneTime, int Repeat) CalculateBuyerSegments(
        IReadOnlyCollection<PerformancePaymentRecord> payments)
    {
        if (payments.Count == 0)
        {
            return (0, 0, 0);
        }

        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var payment in payments)
        {
            var key = BuildBuyerKey(payment);
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            counts[key] = counts.TryGetValue(key, out var count) ? count + 1 : 1;
        }

        var totalBuyers = counts.Count;
        var oneTime = counts.Values.Count(value => value == 1);
        var repeat = counts.Values.Count(value => value > 1);

        return (totalBuyers, oneTime, repeat);
    }

    private static string BuildBuyerKey(PerformancePaymentRecord payment)
    {
        if (payment.BuyerId.HasValue)
        {
            return payment.BuyerId.Value.ToString();
        }

        if (!string.IsNullOrWhiteSpace(payment.BuyerUsername))
        {
            return payment.BuyerUsername!;
        }

        if (!string.IsNullOrWhiteSpace(payment.BuyerFullName))
        {
            return payment.BuyerFullName!;
        }

        return payment.OrderId.ToString();
    }

    private static string FormatPercentageChange(decimal current, decimal compare)
    {
        if (compare == 0m)
        {
            return current == 0m ? "0.0%" : "N/A";
        }

        var change = (current - compare) / compare * 100m;
        var rounded = Math.Round(change, 1, MidpointRounding.AwayFromZero);
        return $"{(rounded >= 0 ? "+" : string.Empty)}{rounded}%";
    }

    private static DateTime ToUtcDateTime(DateOnly date, TimeOnly time)
        => DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Utc);

    private static (DateOnly Start, DateOnly End, int LengthInDays) BuildPrimaryRange(string period, DateTime nowUtc)
    {
        var today = DateOnly.FromDateTime(nowUtc.Date);

        return period switch
        {
            "today" => (today, today, 1),
            "last_7_days" => (today.AddDays(-6), today, 7),
            "last_31_days" => (today.AddDays(-30), today, 31),
            "last_90_days" => (today.AddDays(-89), today, 90),
            "this_year" => (new DateOnly(today.Year, 1, 1), today, today.DayOfYear),
            "current_month" => BuildCurrentMonthRange(today),
            _ => (today.AddDays(-30), today, 31)
        };
    }

    private static (DateOnly Start, DateOnly End, int LengthInDays) BuildCurrentMonthRange(DateOnly today)
    {
        var start = new DateOnly(today.Year, today.Month, 1);
        var length = today.Day;
        return (start, today, length);
    }

    private static CompareRange? BuildCompareRange((DateOnly Start, DateOnly End, int LengthInDays) primaryRange, string compare)
    {
        return compare switch
        {
            "none" => null,
            "previous_year" => new CompareRange(
                SafeAddYears(primaryRange.Start, -1),
                SafeAddYears(primaryRange.End, -1),
                primaryRange.LengthInDays),
            _ => BuildPreviousPeriodRange(primaryRange)
        };
    }

    private static CompareRange BuildPreviousPeriodRange((DateOnly Start, DateOnly End, int LengthInDays) primaryRange)
    {
        var compareEnd = primaryRange.Start.AddDays(-1);
        var compareStart = compareEnd.AddDays(-(primaryRange.LengthInDays - 1));
        return new CompareRange(compareStart, compareEnd, primaryRange.LengthInDays);
    }

    private static string FormatRange(DateOnly start, DateOnly end, int? days = null)
    {
        var core = $"{start.ToString("MMM d, yyyy", CultureInfo.InvariantCulture)} - {end.ToString("MMM d, yyyy", CultureInfo.InvariantCulture)}";
        if (days is null)
        {
            return core;
        }

        return $"{core} ({days} days)";
    }

    private static string UpdatedAt(DateTime timestamp)
    {
        return timestamp.ToString("MMM d 'at' h:mmtt 'UTC'", CultureInfo.InvariantCulture);
    }

    private static DateOnly SafeAddYears(DateOnly value, int years)
    {
        var targetYear = value.Year + years;
        var daysInMonth = DateTime.DaysInMonth(targetYear, value.Month);
        var day = Math.Min(value.Day, daysInMonth);
        return new DateOnly(targetYear, value.Month, day);
    }

    private readonly record struct CompareRange(DateOnly Start, DateOnly End, int LengthInDays);
}
