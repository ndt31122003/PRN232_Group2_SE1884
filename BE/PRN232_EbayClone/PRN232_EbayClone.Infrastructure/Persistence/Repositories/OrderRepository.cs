using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public class OrderRepository :
    Repository<Order, Guid>,
    IOrderRepository
{
    public OrderRepository(
    ApplicationDbContext context,
    IDbConnectionFactory connectionFactory)
    : base(context, connectionFactory)
    {
    }
    private const int MaxPageSize = 200;

    private static readonly CancellationStatus[] CancellationOpenStatuses = new[]
    {
        CancellationStatus.PendingSellerResponse,
        CancellationStatus.PendingBuyerConfirmation
    };

    private static readonly CancellationStatus[] CancellationInProgressStatuses = new[]
    {
        CancellationStatus.PendingBuyerConfirmation,
        CancellationStatus.AwaitingRefund
    };

    private static readonly CancellationStatus[] CancellationCompletedStatuses = new[]
    {
        CancellationStatus.Completed,
        CancellationStatus.AutoCancelled
    };

    private static readonly ReturnStatus[] ReturnNeedsAttentionStatuses = new[]
    {
        ReturnStatus.PendingSellerResponse,
        ReturnStatus.DeliveredToSeller,
        ReturnStatus.RefundPending
    };

    private static readonly ReturnStatus[] ReturnClosedStatuses = new[]
    {
        ReturnStatus.RefundCompleted,
        ReturnStatus.Closed,
        ReturnStatus.SellerDeclined
    };

    private static readonly ReturnStatus[] ReturnInProgressStatuses = new[]
    {
        ReturnStatus.AwaitingBuyerReturn,
        ReturnStatus.InTransitBackToSeller,
        ReturnStatus.DeliveredToSeller,
        ReturnStatus.RefundPending,
        ReturnStatus.ReplacementSent
    };

    private static readonly ReturnStatus[] ReturnShippedStatuses = new[]
    {
        ReturnStatus.InTransitBackToSeller,
        ReturnStatus.ReplacementSent
    };


    public override Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Orders
            .AsSplitQuery()
            .Include(o => o.Status)
            .ThenInclude(o => o.AllowedTransitions)
            .ThenInclude(transition => transition.ToStatus)
            .Include(o => o.StatusHistory)
            .Include(o => o.Items)
            .Include(o => o.ItemShipments)
            .Include(o => o.SellerFeedback)
        .Include(o => o.Buyer)
            .Include(o => o.CancellationRequests)
            .Include(o => o.ReturnRequests)
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<(List<Order> Orders, int TotalCount)> GetOrdersByUserIdAsync(Guid userId, OrderFilterDto filter, CancellationToken cancellationToken = default)
    {
        var baseQuery = DbContext.Orders
            .AsNoTracking()
            .Where(o => o.SellerId == userId);

        if (!string.IsNullOrEmpty(filter.Status))
        {
            var normalizedStatus = filter.Status.Trim();
            if (normalizedStatus.IndexOf(',', StringComparison.Ordinal) >= 0)
            {
                var statusCodes = normalizedStatus
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(code => code.Trim())
                    .Where(code => !string.IsNullOrEmpty(code))
                    .ToArray();

                if (statusCodes.Length > 0)
                {
                    baseQuery = baseQuery.Where(o => statusCodes.Contains(o.Status.Code));
                }
            }
            else if (string.Equals(normalizedStatus, OrderStatusCodes.AwaitingShipment, StringComparison.OrdinalIgnoreCase))
            {
                baseQuery = baseQuery.Where(o => o.Status.Code.StartsWith(normalizedStatus));
            }
            else
            {
                baseQuery = baseQuery.Where(o => o.Status.Code == normalizedStatus);
            }
        }

        var (from, to) = filter.Period == OrderPeriod.Custom && filter.FromDate.HasValue && filter.ToDate.HasValue
            ? (filter.FromDate.Value.Date, filter.ToDate.Value.Date.AddDays(1).AddTicks(-1))
            : GetDateRange(filter.Period);

        baseQuery = baseQuery.Where(o => o.OrderedAt >= from && o.OrderedAt <= to);

        if (!string.IsNullOrWhiteSpace(filter.Keyword) && filter.SearchBy.HasValue)
        {
            var keyword = filter.Keyword.Trim().ToLower();
            baseQuery = filter.SearchBy.Value switch
            {
                OrderSearchBy.BuyerUsername => baseQuery.Where(o => o.Buyer != null && o.Buyer.Username.ToLower().Contains(keyword)),
                OrderSearchBy.BuyerName => baseQuery.Where(o => o.Buyer != null && o.Buyer.FullName.ToLower().Contains(keyword)),
                OrderSearchBy.OrderNumber => baseQuery.Where(o => o.OrderNumber.ToLower().Contains(keyword)),
                OrderSearchBy.SalesRecordNumber => baseQuery.Where(o => o.Id.ToString().ToLower().Contains(keyword)),
                OrderSearchBy.ItemTitle => baseQuery.Where(o => o.Items.Any(i => i.Title.ToLower().Contains(keyword))),
                OrderSearchBy.ItemId => baseQuery.Where(o => o.Items.Any(i => i.ListingId.ToString().ToLower().Contains(keyword))),
                OrderSearchBy.CustomLabel => baseQuery.Where(o => o.Items.Any(i => i.Sku.ToLower().Contains(keyword))),
                _ => baseQuery
            };
        }

        var orderedQuery = filter.SortBy switch
        {
            OrderSortBy.DatePaid => filter.SortDescending
                ? baseQuery.OrderByDescending(o => o.PaidAt ?? o.OrderedAt)
                : baseQuery.OrderBy(o => o.PaidAt ?? o.OrderedAt),
            OrderSortBy.Buyer => filter.SortDescending
                ? baseQuery.OrderByDescending(o => o.Buyer != null ? o.Buyer.FullName : string.Empty)
                : baseQuery.OrderBy(o => o.Buyer != null ? o.Buyer.FullName : string.Empty),
            OrderSortBy.CustomLabel => filter.SortDescending
                ? baseQuery.OrderByDescending(o => o.Items.Select(i => i.Sku).FirstOrDefault())
                : baseQuery.OrderBy(o => o.Items.Select(i => i.Sku).FirstOrDefault()),
            OrderSortBy.Total => filter.SortDescending
                ? baseQuery.OrderByDescending(o => o.Total.Amount)
                : baseQuery.OrderBy(o => o.Total.Amount),
            _ => filter.SortDescending
                ? baseQuery.OrderByDescending(o => o.PaidAt ?? o.OrderedAt)
                : baseQuery.OrderBy(o => o.PaidAt ?? o.OrderedAt),
        };

        var totalCount = await orderedQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (new List<Order>(), 0);
        }

        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        var skip = (pageNumber - 1) * pageSize;

        var orderIds = await orderedQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(o => o.Id)
            .ToListAsync(cancellationToken);

        if (orderIds.Count == 0)
        {
            return (new List<Order>(), totalCount);
        }

        var ordersWithIncludes = await DbContext.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.Status)
            .Include(o => o.Buyer)
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
                .ThenInclude(h => h.FromStatus)
            .Include(o => o.StatusHistory)
                .ThenInclude(h => h.ToStatus)
            .Include(o => o.ItemShipments)
            .Include(o => o.SellerFeedback)
            .Where(o => orderIds.Contains(o.Id))
            .ToListAsync(cancellationToken);

        var orderLookup = ordersWithIncludes.ToDictionary(o => o.Id);
        var orderedResult = orderIds
            .Where(orderLookup.ContainsKey)
            .Select(id => orderLookup[id])
            .ToList();

        return (orderedResult, totalCount);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForSellerAsync(Guid sellerId, DateTime fromUtc, DateTime toUtc, CancellationToken cancellationToken = default)
    {
        return await DbContext.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.Status)
            .Include(o => o.Items)
            .Include(o => o.Buyer)
            .Include(o => o.ItemShipments)
            .Where(o => o.SellerId == sellerId && o.OrderedAt >= fromUtc && o.OrderedAt <= toUtc)
            .OrderByDescending(o => o.OrderedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetByIdsAsync(IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default)
    {
        if (orderIds is null)
        {
            return Array.Empty<Order>();
        }

        var ids = orderIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (ids.Count == 0)
        {
            return Array.Empty<Order>();
        }

        return await DbContext.Orders
            .AsSplitQuery()
            .Include(o => o.Status)
            .ThenInclude(status => status.AllowedTransitions)
            .ThenInclude(transition => transition.ToStatus)
            .Include(o => o.StatusHistory)
            .ThenInclude(history => history.FromStatus)
            .Include(o => o.StatusHistory)
            .ThenInclude(history => history.ToStatus)
            .Include(o => o.Items)
            .Include(o => o.ItemShipments)
            .Include(o => o.SellerFeedback)
            .Include(o => o.Buyer)
            .Include(o => o.CancellationRequests)
            .Include(o => o.ReturnRequests)
            .Where(o => ids.Contains(o.Id))
            .ToListAsync(cancellationToken);
    }
    //helper rangedate for filter
    private static (DateTime Start, DateTime End) GetDateRange(OrderPeriod period)
    {
        var nowUtc = DateTime.UtcNow;
        var startOfTodayUtc = DateTime.SpecifyKind(nowUtc.Date, DateTimeKind.Utc);
        var endOfTodayUtc = startOfTodayUtc.AddDays(1).AddTicks(-1);

        DateTime StartOfMonthUtc(int year, int month) => new(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime StartOfYearUtc(int year) => new(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return period switch
        {
            OrderPeriod.Today => (startOfTodayUtc, endOfTodayUtc),
            OrderPeriod.Yesterday => (startOfTodayUtc.AddDays(-1), startOfTodayUtc.AddTicks(-1)),
            OrderPeriod.ThisWeek =>
                (startOfTodayUtc.AddDays(-(int)startOfTodayUtc.DayOfWeek),
                 startOfTodayUtc.AddDays(7 - (int)startOfTodayUtc.DayOfWeek).AddTicks(-1)),
            OrderPeriod.LastWeek =>
                (startOfTodayUtc.AddDays(-(int)startOfTodayUtc.DayOfWeek - 7),
                 startOfTodayUtc.AddDays(-(int)startOfTodayUtc.DayOfWeek).AddTicks(-1)),
            OrderPeriod.ThisMonth =>
                (
                    StartOfMonthUtc(startOfTodayUtc.Year, startOfTodayUtc.Month),
                    StartOfMonthUtc(startOfTodayUtc.Year, startOfTodayUtc.Month).AddMonths(1).AddTicks(-1)
                ),
            OrderPeriod.LastMonth =>
                (
                    StartOfMonthUtc(startOfTodayUtc.Year, startOfTodayUtc.Month).AddMonths(-1),
                    StartOfMonthUtc(startOfTodayUtc.Year, startOfTodayUtc.Month).AddTicks(-1)
                ),
            OrderPeriod.ThisYear =>
                (
                    StartOfYearUtc(startOfTodayUtc.Year),
                    StartOfYearUtc(startOfTodayUtc.Year).AddYears(1).AddTicks(-1)
                ),
            OrderPeriod.LastYear =>
                (
                    StartOfYearUtc(startOfTodayUtc.Year - 1),
                    StartOfYearUtc(startOfTodayUtc.Year).AddTicks(-1)
                ),
            OrderPeriod.Last7Days => (startOfTodayUtc.AddDays(-6), endOfTodayUtc),
            OrderPeriod.Last30Days => (startOfTodayUtc.AddDays(-29), endOfTodayUtc),
            OrderPeriod.Last90Days => (startOfTodayUtc.AddDays(-89), endOfTodayUtc),
            _ => (startOfTodayUtc.AddDays(-89), endOfTodayUtc)
        };
    }

    public Task<List<OrderStatus>> GetAllOrderStatusesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.OrderStatuses
            .AsNoTracking()
            .OrderBy(status => status.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductResearchOrderItemRecord>> GetProductResearchOrderItemsAsync(
        Guid? sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        string? keyword,
        Guid? categoryId,
        CancellationToken cancellationToken)
    {
        var keywordPattern = string.IsNullOrWhiteSpace(keyword)
            ? null
            : $"%{keyword.Trim()}%";

        const string sql = """
SELECT
    o.id AS "OrderId",
    oi.listing_id AS "ListingId",
    oi.title AS "Title",
    oi.sku AS "Sku",
    oi.quantity AS "Quantity",
    oi.unit_price_amount AS "UnitPriceAmount",
    oi.unit_price_currency AS "UnitPriceCurrency",
    oi.total_price_amount AS "TotalPriceAmount",
    o.ordered_at AS "OrderedAtUtc",
    o.paid_at AS "PaidAtUtc",
    o.shipping_cost_amount AS "ShippingAmount",
    o.shipping_cost_currency AS "ShippingCurrency",
    o.buyer_id AS "BuyerId",
    oi.image_url AS "ImageUrl",
    l.category_id AS "CategoryId"
FROM orders o
JOIN "order_items" oi ON oi.order_id = o.id
JOIN listing l ON l.id = oi.listing_id
WHERE (@SellerId IS NULL OR o.seller_id = @SellerId)
    AND o.ordered_at BETWEEN @FromUtc AND @ToUtc
  AND (@KeywordPattern IS NULL
       OR oi.title ILIKE @KeywordPattern
       OR oi.sku ILIKE @KeywordPattern)
  AND (@CategoryId IS NULL OR l.category_id = @CategoryId);
""";

        using var connection = await ConnectionFactory.CreateConnectionAsync();

        var parameters = new
        {
            SellerId = sellerId,
            FromUtc = fromUtc,
            ToUtc = toUtc,
            KeywordPattern = keywordPattern,
            CategoryId = categoryId
        };

        var records = await connection.QueryAsync<ProductResearchOrderItemRecord>(new CommandDefinition(
            sql,
            parameters,
            cancellationToken: cancellationToken));

        return records.AsList();

    }

    public Task<OrderStatus?> GetStatusByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return DbContext.OrderStatuses
            .FirstOrDefaultAsync(status => status.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<OrderOverviewRecord>> GetOverviewRecordsAsync(Guid sellerId, DateTime fromDateUtc, CancellationToken cancellationToken)
    {
        var records = await DbContext.Orders
            .AsNoTracking()
            .Include(o => o.Status)
            .Where(o => o.SellerId == sellerId && o.OrderedAt >= fromDateUtc)
            .Select(o => new OrderOverviewRecord(
                o.OrderedAt,
                o.PaidAt,
                o.Status.Code,
                o.Total.Amount,
                o.Total.Currency))
            .ToListAsync(cancellationToken);

        return records;
    }

    public async Task<(IReadOnlyList<CancellationSummaryRecord> Records, int TotalCount)> GetCancellationRequestsAsync(
        Guid sellerId,
        CancellationFilterDto filter,
        CancellationToken cancellationToken)
    {

        var (fromUtc, toUtc) = ResolvePeriod(filter.Period, filter.FromDate, filter.ToDate);

        var query = DbContext.CancellationRequests
            .AsNoTracking()
            .Where(request => request.SellerId == sellerId)
            .Where(request => request.RequestedAt >= fromUtc && request.RequestedAt <= toUtc);

        query = filter.Status switch
        {
            CancellationFilterStatus.Open => query.Where(request => CancellationOpenStatuses.Contains(request.Status)),
            CancellationFilterStatus.Requests => query.Where(request => request.InitiatedBy == CancellationInitiator.Buyer),
            CancellationFilterStatus.InProgress => query.Where(request => CancellationInProgressStatuses.Contains(request.Status)),
            CancellationFilterStatus.Declined => query.Where(request => request.Status == CancellationStatus.Declined),
            CancellationFilterStatus.Cancelled => query.Where(request => CancellationCompletedStatuses.Contains(request.Status)),
            _ => query
        };

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim().ToLower();
            query = filter.SearchBy switch
            {
                CancellationSearchBy.BuyerUsername => query.Where(request =>
                    request.Order.Buyer != null && request.Order.Buyer.Username.ToLower().Contains(keyword)),
                CancellationSearchBy.CancelId => query.Where(request =>
                    request.Id.ToString().ToLower().Contains(keyword)),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orderedQuery = filter.SortBy switch
        {
            CancellationSortBy.DateRequested => filter.SortDescending
                ? query.OrderByDescending(request => request.RequestedAt)
                : query.OrderBy(request => request.RequestedAt),
            _ => filter.SortDescending
                ? query.OrderByDescending(request => request.RequestedAt)
                : query.OrderBy(request => request.RequestedAt)
        };

        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        var skip = (pageNumber - 1) * pageSize;

        var records = await orderedQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(request => new CancellationSummaryRecord(
                request.Id,
                request.OrderId,
                request.Order.OrderNumber,
                request.Order.OrderedAt,
                request.RequestedAt,
                request.SellerRespondedAt,
                request.SellerResponseDeadlineUtc,
                request.AutoClosedAt,
                request.CompletedAt,
                request.Status,
                request.InitiatedBy,
                request.Reason,
                request.BuyerNote,
                request.SellerNote,
                request.OrderTotalSnapshot.Amount,
                request.OrderTotalSnapshot.Currency,
                request.RefundAmount != null ? request.RefundAmount.Amount : (decimal?)null,
                request.RefundAmount != null ? request.RefundAmount.Currency : null,
                request.Order.Buyer != null ? request.Order.Buyer.Username : string.Empty,
                request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty,
                request.Order.Items.Select(item => item.Title).ToList(),
                request.Order.Items.Count))
            .ToListAsync(cancellationToken);

        return (records, totalCount);
    }

    public async Task<CancellationSummaryRecord?> GetCancellationRequestAsync(
        Guid sellerId,
        Guid cancellationRequestId,
        CancellationToken cancellationToken)
    {
        return await DbContext.CancellationRequests
            .AsNoTracking()
            .Where(request => request.SellerId == sellerId && request.Id == cancellationRequestId)
            .Select(request => new CancellationSummaryRecord(
                request.Id,
                request.OrderId,
                request.Order.OrderNumber,
                request.Order.OrderedAt,
                request.RequestedAt,
                request.SellerRespondedAt,
                request.SellerResponseDeadlineUtc,
                request.AutoClosedAt,
                request.CompletedAt,
                request.Status,
                request.InitiatedBy,
                request.Reason,
                request.BuyerNote,
                request.SellerNote,
                request.OrderTotalSnapshot.Amount,
                request.OrderTotalSnapshot.Currency,
                request.RefundAmount != null ? request.RefundAmount.Amount : (decimal?)null,
                request.RefundAmount != null ? request.RefundAmount.Currency : null,
                request.Order.Buyer != null ? request.Order.Buyer.Username : string.Empty,
                request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty,
                request.Order.Items.Select(item => item.Title).ToList(),
                request.Order.Items.Count))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<Order?> GetOrderByCancellationRequestIdAsync(
        Guid sellerId,
        Guid cancellationRequestId,
        CancellationToken cancellationToken)
    {
        return DbContext.Orders
            .AsSplitQuery()
            .Include(o => o.Status)
                .ThenInclude(status => status.AllowedTransitions)
                .ThenInclude(transition => transition.ToStatus)
            .Include(o => o.StatusHistory)
                .ThenInclude(history => history.FromStatus)
            .Include(o => o.StatusHistory)
                .ThenInclude(history => history.ToStatus)
            .Include(o => o.Items)
            .Include(o => o.ItemShipments)
            .Include(o => o.SellerFeedback)
            .Include(o => o.Buyer)
            .Include(o => o.CancellationRequests)
            .Include(o => o.ReturnRequests)
            .SingleOrDefaultAsync(
                o => o.SellerId == sellerId && o.CancellationRequests.Any(request => request.Id == cancellationRequestId),
                cancellationToken);
    }

    public Task<Order?> GetOrderByReturnRequestIdAsync(
        Guid sellerId,
        Guid returnRequestId,
        CancellationToken cancellationToken)
    {
        return DbContext.Orders
            .AsSplitQuery()
            .Include(o => o.Status)
                .ThenInclude(status => status.AllowedTransitions)
                .ThenInclude(transition => transition.ToStatus)
            .Include(o => o.StatusHistory)
                .ThenInclude(history => history.FromStatus)
            .Include(o => o.StatusHistory)
                .ThenInclude(history => history.ToStatus)
            .Include(o => o.Items)
            .Include(o => o.ItemShipments)
            .Include(o => o.SellerFeedback)
            .Include(o => o.Buyer)
            .Include(o => o.CancellationRequests)
            .Include(o => o.ReturnRequests)
            .SingleOrDefaultAsync(
                o => o.SellerId == sellerId && o.ReturnRequests.Any(request => request.Id == returnRequestId),
                cancellationToken);
    }

    public async Task<(IReadOnlyList<ReturnRequestSummaryRecord> Records, int TotalCount)> GetReturnRequestsAsync(
        Guid sellerId,
        ReturnFilterDto filter,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var (fromUtc, toUtc) = ResolvePeriod(filter.Period, filter.FromDate, filter.ToDate);

        var query = DbContext.ReturnRequests
            .AsNoTracking()
            .Where(request => request.SellerId == sellerId)
            .Where(request => request.RequestedAt >= fromUtc && request.RequestedAt <= toUtc);

        query = filter.Status switch
        {
            ReturnFilterStatus.NeedsAttention => query.Where(request => ReturnNeedsAttentionStatuses.Contains(request.Status)),
            ReturnFilterStatus.OpenReturnsReplacements => query.Where(request => !ReturnClosedStatuses.Contains(request.Status)),
            ReturnFilterStatus.OpenReplacements => query.Where(request =>
                request.PreferredResolution == ReturnResolution.Replacement && !ReturnClosedStatuses.Contains(request.Status)),
            ReturnFilterStatus.OpenReturns => query.Where(request =>
                request.PreferredResolution != ReturnResolution.Replacement && !ReturnClosedStatuses.Contains(request.Status)),
            ReturnFilterStatus.InProgress => query.Where(request => ReturnInProgressStatuses.Contains(request.Status)),
            ReturnFilterStatus.Shipped => query.Where(request => ReturnShippedStatuses.Contains(request.Status)),
            ReturnFilterStatus.Delivered => query.Where(request => request.Status == ReturnStatus.DeliveredToSeller),
            ReturnFilterStatus.Closed => query.Where(request => ReturnClosedStatuses.Contains(request.Status)),
            _ => query
        };

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim().ToLower();
            query = filter.SearchBy switch
            {
                ReturnSearchBy.BuyerUsername => query.Where(request =>
                    request.Order.Buyer != null && request.Order.Buyer.Username.ToLower().Contains(keyword)),
                ReturnSearchBy.OrderNumber => query.Where(request =>
                    request.Order.OrderNumber.ToLower().Contains(keyword)),
                ReturnSearchBy.ItemTitle => query.Where(request =>
                    request.Order.Items.Any(item => item.Title.ToLower().Contains(keyword))),
                ReturnSearchBy.TrackingNumber => query.Where(request =>
                    request.TrackingNumber != null && request.TrackingNumber.ToLower().Contains(keyword)),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orderedQuery = filter.SortBy switch
        {
            ReturnSortBy.Buyer => filter.SortDescending
                ? query.OrderByDescending(request => request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty)
                : query.OrderBy(request => request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty),
            ReturnSortBy.ReturnStatus => filter.SortDescending
                ? query.OrderByDescending(request => request.Status)
                : query.OrderBy(request => request.Status),
            ReturnSortBy.DueDate => filter.SortDescending
                ? query.OrderByDescending(request => request.BuyerReturnDueAt ?? DateTime.MaxValue)
                : query.OrderBy(request => request.BuyerReturnDueAt ?? DateTime.MaxValue),
            _ => filter.SortDescending
                ? query.OrderByDescending(request => request.RequestedAt)
                : query.OrderBy(request => request.RequestedAt)
        };

        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        var skip = (pageNumber - 1) * pageSize;

        var records = await orderedQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(request => new ReturnRequestSummaryRecord(
                request.Id,
                request.OrderId,
                request.Order.OrderNumber,
                request.Order.OrderedAt,
                request.RequestedAt,
                request.SellerRespondedAt,
                request.BuyerReturnDueAt,
                request.BuyerShippedAt,
                request.DeliveredAt,
                request.RefundIssuedAt,
                request.ClosedAt,
                request.Status,
                request.Reason,
                request.PreferredResolution,
                request.BuyerNote,
                request.SellerNote,
                request.ReturnCarrier,
                request.TrackingNumber,
                request.OrderTotalSnapshot.Amount,
                request.OrderTotalSnapshot.Currency,
                request.RefundAmount != null ? request.RefundAmount.Amount : (decimal?)null,
                request.RefundAmount != null ? request.RefundAmount.Currency : null,
                request.RestockingFee != null ? request.RestockingFee.Amount : (decimal?)null,
                request.RestockingFee != null ? request.RestockingFee.Currency : null,
                request.Order.Buyer != null ? request.Order.Buyer.Username : string.Empty,
                request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty,
                request.Order.Items.Select(item => item.Title).ToList(),
                request.Order.Items.Count,
                request.Order.PaidAt != null))
            .ToListAsync(cancellationToken);

        return (records, totalCount);
    }

    public async Task<ReturnRequestSummaryRecord?> GetReturnRequestAsync(
        Guid sellerId,
        Guid returnRequestId,
        CancellationToken cancellationToken)
    {
        return await DbContext.ReturnRequests
            .AsNoTracking()
            .Where(request => request.SellerId == sellerId && request.Id == returnRequestId)
            .Select(request => new ReturnRequestSummaryRecord(
                request.Id,
                request.OrderId,
                request.Order.OrderNumber,
                request.Order.OrderedAt,
                request.RequestedAt,
                request.SellerRespondedAt,
                request.BuyerReturnDueAt,
                request.BuyerShippedAt,
                request.DeliveredAt,
                request.RefundIssuedAt,
                request.ClosedAt,
                request.Status,
                request.Reason,
                request.PreferredResolution,
                request.BuyerNote,
                request.SellerNote,
                request.ReturnCarrier,
                request.TrackingNumber,
                request.OrderTotalSnapshot.Amount,
                request.OrderTotalSnapshot.Currency,
                request.RefundAmount != null ? request.RefundAmount.Amount : (decimal?)null,
                request.RefundAmount != null ? request.RefundAmount.Currency : null,
                request.RestockingFee != null ? request.RestockingFee.Amount : (decimal?)null,
                request.RestockingFee != null ? request.RestockingFee.Currency : null,
                request.Order.Buyer != null ? request.Order.Buyer.Username : string.Empty,
                request.Order.Buyer != null ? request.Order.Buyer.FullName : string.Empty,
                request.Order.Items.Select(item => item.Title).ToList(),
                request.Order.Items.Count,
                request.Order.PaidAt != null))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Order>> GetAwaitingShipmentOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        var awaitingStatuses = new[]
        {
            OrderStatusCodes.AwaitingShipment,
            OrderStatusCodes.AwaitingShipmentShipWithin24h,
            OrderStatusCodes.AwaitingShipmentOverdue
        };

        var orders = await DbContext.Orders
            .AsSplitQuery()
            .Include(order => order.Status)
                .ThenInclude(status => status.AllowedTransitions)
                .ThenInclude(transition => transition.ToStatus)
            .Include(order => order.StatusHistory)
            .Where(order => order.Status != null && awaitingStatuses.Contains(order.Status.Code))
            .ToListAsync(cancellationToken);

        return orders;
    }

    private static (DateTime Start, DateTime End) ResolvePeriod(ResolutionPeriod period, DateTime? from, DateTime? to)
    {
        var now = DateTime.UtcNow;

        return period switch
        {
            ResolutionPeriod.Last30Days => (now.AddDays(-30), now),
            ResolutionPeriod.Last90Days => (now.AddDays(-90), now),
            ResolutionPeriod.Last180Days => (now.AddDays(-180), now),
            ResolutionPeriod.ThisYear => (new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(now.Year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1)),
            ResolutionPeriod.Custom => ResolveCustomPeriod(from, to, now),
            _ => (now.AddDays(-90), now)
        };
    }

    private static (DateTime Start, DateTime End) ResolveCustomPeriod(DateTime? from, DateTime? to, DateTime now)
    {
        var start = from.HasValue ? NormalizeStart(from.Value) : now.AddDays(-90);
        var end = to.HasValue ? NormalizeEnd(to.Value) : now;

        if (start > end)
        {
            (start, end) = (end, start);
        }

        return (start, end);
    }

    private static DateTime NormalizeStart(DateTime value)
    {
        value = NormalizeKind(value);
        return value;
    }

    private static DateTime NormalizeEnd(DateTime value)
    {
        value = NormalizeKind(value);

        if (value.TimeOfDay == TimeSpan.Zero)
        {
            value = value.Date.AddDays(1).AddTicks(-1);
        }

        return value;
    }

    private static DateTime NormalizeKind(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => value
        };
    }
}
