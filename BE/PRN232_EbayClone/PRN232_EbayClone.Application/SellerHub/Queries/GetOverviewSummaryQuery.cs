using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Dtos;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Application.SellerHub.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellerHub.Queries;

public sealed record GetOverviewSummaryQuery() : IQuery<OverviewSummaryDto>;

public sealed class GetOverviewSummaryQueryHandler(
    IUserContext UserContext,
    IUserRepository UserRepository,
    IListingRepository ListingRepository,
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetOverviewSummaryQuery, OverviewSummaryDto>
{
    private static readonly HashSet<string> PaidStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        OrderStatusCodes.PaidAndShipped,
        OrderStatusCodes.PaidAwaitingFeedback,
        OrderStatusCodes.ShippedAwaitingFeedback
    };

    private static readonly HashSet<string> AwaitingShipmentStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        OrderStatusCodes.AwaitingShipment,
        OrderStatusCodes.AwaitingShipmentOverdue,
        OrderStatusCodes.AwaitingShipmentShipWithin24h,
        OrderStatusCodes.AwaitingExpeditedShipment
    };

    public async Task<Result<OverviewSummaryDto>> Handle(GetOverviewSummaryQuery request, CancellationToken cancellationToken)
    {
        var sellerId = Guid.Parse(UserContext.UserId!);
        var user = await UserRepository.GetByIdAsync(new UserId(sellerId), cancellationToken);
        if (user is null)
        {
            return Error.Failure("SellerHub.Overview.UserNotFound", "Seller not found.");
        }

        var nowUtc = DateTime.UtcNow;
        var ownerKey = user.Id.Value.ToString();

        var listingSnapshot = await ListingRepository.GetOverviewSnapshotAsync(ownerKey, nowUtc, cancellationToken);
        var ordersFrom = nowUtc.AddDays(-90);
        var orderRecords = await PerformanceRepository.GetOverviewRecordsAsync(sellerId, ordersFrom, cancellationToken);

        var currency = orderRecords.FirstOrDefault()?.Currency ?? "USD";
        var paidOrders = orderRecords
            .Where(r => PaidStatuses.Contains(r.StatusCode))
            .Select(r => (DateOnly.FromDateTime((r.PaidAt ?? r.OrderedAt).Date), r.TotalAmount))
            .ToList();

        var ordersLast90Days = orderRecords.Count(r => !string.Equals(r.StatusCode, OrderStatusCodes.Draft, StringComparison.OrdinalIgnoreCase));
        var salesLast90Days = paidOrders.Sum(r => r.TotalAmount);

        var awaitingPaymentCount = CountByStatus(orderRecords, OrderStatusCodes.AwaitingPayment);
        var awaitingShipmentCount = orderRecords.Count(r => AwaitingShipmentStatuses.Contains(r.StatusCode));
        var shippedAwaitingFeedbackCount = CountByStatus(orderRecords, OrderStatusCodes.ShippedAwaitingFeedback)
            + CountByStatus(orderRecords, OrderStatusCodes.PaidAwaitingFeedback);
        var cancelledCount = CountByStatus(orderRecords, OrderStatusCodes.Cancelled);

        var outstandingTasks = awaitingPaymentCount + awaitingShipmentCount + cancelledCount;
        var statusLevel = outstandingTasks > 0 ? "warning" : "success";
        var statusMessage = outstandingTasks > 0
            ? $"You have {outstandingTasks} task{(outstandingTasks > 1 ? "s" : string.Empty)} waiting for attention."
            : "You're all caught up!";

        var listingsSection = BuildListingsSection(listingSnapshot);
        var ordersSection = BuildOrdersSection(awaitingShipmentCount, awaitingPaymentCount, shippedAwaitingFeedbackCount, cancelledCount);
        var salesSection = BuildSalesSection(nowUtc, paidOrders, currency);

        var header = new OverviewHeaderDto(
            user.Id.Value,
            user.FullName,
            user.Username,
            user.Email.Value,
            ListingViewsLast90Days: 0,
            SalesLast90Days: Math.Round(salesLast90Days, 2, MidpointRounding.AwayFromZero),
            SalesCurrency: currency,
            OrdersLast90Days: ordersLast90Days
        );

        var status = new OverviewStatusDto(statusMessage, statusLevel, outstandingTasks);

        var summary = new OverviewSummaryDto(header, status, listingsSection, ordersSection, salesSection);
        return Result.Success(summary);
    }

    private static OverviewSectionDto BuildListingsSection(ListingOverviewSnapshot snapshot)
    {
        snapshot.StatusCounts.TryGetValue(ListingStatus.Draft, out var draftCount);
        snapshot.StatusCounts.TryGetValue(ListingStatus.Active, out var activeCount);
        snapshot.StatusCounts.TryGetValue(ListingStatus.Scheduled, out var scheduledCount);
        snapshot.StatusCounts.TryGetValue(ListingStatus.Ended, out var endedCount);

        var items = new List<OverviewSectionItemDto>
        {
            new("create", "Create listing", null, "/listing-form"),
            new("drafts", "Drafts", draftCount, "/listings/drafts"),
            new("active", "Active listings", activeCount, "/listings/active"),
            new("questions", "With questions", snapshot.WithQuestions, null),
            new("offers", "With open offers from buyers", snapshot.WithOpenOffers, null),
            new("auctions", "All auctions", endedCount, "/listings/ended"),
            new("reserve", "With reserve met", snapshot.WithReserveMet, null),
            new("auctionsEnding", "Auctions ending today", snapshot.AuctionsEndingToday, "/listings/active"),
            new("binRenewing", "Buy It Now renewing today", snapshot.BuyItNowRenewingToday, "/listings/active"),
            new("scheduled", "Scheduled listings", scheduledCount, "/listings/scheduled"),
            new("unsold", "Unsold and not relisted", snapshot.UnsoldNotRelisted, "/listings/unsold")
        };

        return new OverviewSectionDto("Listings", items);
    }

    private static OverviewSectionDto BuildOrdersSection(int awaitingShipment, int awaitingPayment, int shippedAwaitingFeedback, int cancelled)
    {
        var items = new List<OverviewSectionItemDto>
        {
            new("all", "See all orders", null, "/orders/all"),
            new("awaitingShipment", "Awaiting shipment - print shipping label", awaitingShipment, "/orders/awaiting-shipment"),
            new("returns", "All open returns/replacements", 0, null),
            new("cancellations", "Open cancellations", cancelled, "/orders/cancelled"),
            new("awaitingPayment", "Awaiting payment", awaitingPayment, "/orders/awaiting-payment"),
            new("shippedAwaitingFeedback", "Shipped and awaiting your feedback", shippedAwaitingFeedback, "/orders/paid-shipped"),
            new("combined", "Orders eligible for combined purchases", 0, null)
        };

        return new OverviewSectionDto("Orders", items);
    }

    private static OverviewSalesDto BuildSalesSection(DateTime nowUtc, IReadOnlyList<(DateOnly PaidDate, decimal TotalAmount)> paidOrders, string currency)
    {
        var today = DateOnly.FromDateTime(nowUtc.Date);
        var chartStart = today.AddDays(-30);
        var totalsByDate = paidOrders
            .GroupBy(x => x.PaidDate)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.TotalAmount));

        var chartPoints = new List<OverviewSalesPointDto>(31);
        for (var date = chartStart; date <= today; date = date.AddDays(1))
        {
            totalsByDate.TryGetValue(date, out var total);
            chartPoints.Add(new OverviewSalesPointDto(date, Math.Round(total, 2, MidpointRounding.AwayFromZero)));
        }

        static decimal Round(decimal amount) => Math.Round(amount, 2, MidpointRounding.AwayFromZero);

        var summary = new List<OverviewSalesSummaryRowDto>
        {
            new("today", "Today", Round(paidOrders.Where(p => p.PaidDate == today).Sum(p => p.TotalAmount))),
            new("last7", "Last 7 days", Round(paidOrders.Where(p => p.PaidDate >= today.AddDays(-6)).Sum(p => p.TotalAmount))),
            new("last31", "Last 31 days", Round(paidOrders.Where(p => p.PaidDate >= today.AddDays(-30)).Sum(p => p.TotalAmount))),
            new("last90", "Last 90 days", Round(paidOrders.Where(p => p.PaidDate >= today.AddDays(-89)).Sum(p => p.TotalAmount)))
        };

        return new OverviewSalesDto(chartPoints, summary, currency);
    }

    private static int CountByStatus(IEnumerable<PerformanceOverviewRecord> records, string statusCode)
    {
        return records.Count(r => string.Equals(r.StatusCode, statusCode, StringComparison.OrdinalIgnoreCase));
    }
}
