using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Application.Performance.Helpers; // ✅ THÊM DÒNG NÀY
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using PRN232_EbayClone.Infrastructure.Persistence;
using System.Text.Json;
using static PRN232_EbayClone.Application.Performance.ValueObjects.SellerStandards;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class PerformanceRepository : IPerformanceRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IDistributedCache _cache;
    // ❌ XÓA DÒNG NÀY:
    // private const int DefaultHandlingTimeHours = 48;
    
    // ✅ THAY BẰNG:
    private const int DefaultHandlingTimeDays = 2; // eBay standard: 2 business days

    public PerformanceRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory,
        IDistributedCache cache)
    {
        _context = context;
        _connectionFactory = connectionFactory;
        _cache = cache;
    }

    public async Task<IReadOnlyList<PerformanceOverviewRecord>> GetOverviewRecordsAsync(
        Guid sellerId,
        DateTime fromDateUtc,
        CancellationToken cancellationToken = default)
    {
        var normalizedFromUtc = fromDateUtc.Kind == DateTimeKind.Utc
            ? fromDateUtc
            : DateTime.SpecifyKind(fromDateUtc, DateTimeKind.Utc);

        var records = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Status)
            .Where(o => o.SellerId == sellerId && o.OrderedAt >= normalizedFromUtc)
            .Select(o => new PerformanceOverviewRecord(
                o.OrderedAt,
                o.PaidAt,
                o.Status.Code,
                // Total Sales = (SubTotal - Discount) + Shipping + Tax
                (o.SubTotal.Amount - o.DiscountAmount.Amount) + o.ShippingCost.Amount + o.TaxAmount.Amount,
                o.Total.Currency))
            .ToListAsync(cancellationToken);

        return records;
    }

    public async Task<IReadOnlyList<PerformancePaymentRecord>> GetPaymentRecordsAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default)
    {
        var normalizedFromUtc = fromUtc.Kind == DateTimeKind.Utc
            ? fromUtc
            : DateTime.SpecifyKind(fromUtc, DateTimeKind.Utc);

        var normalizedToUtc = toUtc.Kind == DateTimeKind.Utc
            ? toUtc
            : DateTime.SpecifyKind(toUtc, DateTimeKind.Utc);

        var records = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Status)
            .Include(o => o.Buyer)
            .Where(o =>
                o.SellerId == sellerId &&
                o.OrderedAt >= normalizedFromUtc &&
                o.OrderedAt <= normalizedToUtc)
            .Select(o => new PerformancePaymentRecord(
                o.Id,
                o.OrderNumber,
                o.OrderedAt,
                o.PaidAt,
                o.Status.Code,
                o.SubTotal.Amount,
                o.ShippingCost.Amount,
                o.TaxAmount.Amount,
                o.DiscountAmount.Amount,
                o.PlatformFee.Amount,
                o.Total.Amount,
                o.Total.Currency,
                o.Buyer != null ? o.Buyer.Id.Value : (Guid?)null,
                o.Buyer != null ? o.Buyer.FullName : null,
                o.Buyer != null ? o.Buyer.Username : null,
                0m))
            .ToListAsync(cancellationToken);

        if (records.Count == 0)
        {
            return records;
        }

        var orderIds = records
            .Select(record => record.OrderId)
            .Distinct()
            .ToList();

        var shippingLabelTotals = await _context.ShippingLabels
            .AsNoTracking()
            .Where(label =>
                orderIds.Contains(label.OrderId) &&
                label.PurchasedAt.UtcDateTime >= normalizedFromUtc &&
                label.PurchasedAt.UtcDateTime <= normalizedToUtc &&
                !label.IsVoided)
            .GroupBy(label => label.OrderId)
            .Select(group => new
            {
                OrderId = group.Key,
                Amount = group.Sum(label => label.Cost.Amount)
            })
            .ToDictionaryAsync(k => k.OrderId, v => v.Amount, cancellationToken);

        return records
            .Select(record => shippingLabelTotals.TryGetValue(record.OrderId, out var amount)
                ? record with { ShippingLabelAmount = amount }
                : record)
            .ToList();

    }

    public async Task<PerformanceTrafficAggregate> GetTrafficAggregateAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default)
    {
        if (toUtc < fromUtc)
        {
            throw new ArgumentException("The end date must be greater than or equal to the start date.", nameof(toUtc));
        }

        var ordersQuery = _context.Orders
            .AsNoTracking()
            .Where(o =>
                o.SellerId == sellerId &&
                o.OrderedAt >= fromUtc &&
                o.OrderedAt <= toUtc &&
                o.Status.Code != OrderStatusCodes.Draft);

        var orders = await ordersQuery
            .Select(o => new
            {
                o.Id,
                Amount = o.Total.Amount,
                Currency = o.Total.Currency
            })
            .ToListAsync(cancellationToken);

        if (orders.Count == 0)
        {
            return new PerformanceTrafficAggregate(0, 0, 0, 0m, "USD");
        }

        var orderIds = orders.Select(o => o.Id).ToList();

        var items = await _context.OrderItems
            .AsNoTracking()
            .Where(item => orderIds.Contains(EF.Property<Guid>(item, "OrderId")))
            .Select(item => new
            {
                OrderId = EF.Property<Guid>(item, "OrderId"),
                item.ListingId,
                item.Quantity
            })
            .ToListAsync(cancellationToken);

        var totalQuantity = items.Sum(i => i.Quantity);
        var distinctListings = items.Select(i => i.ListingId).Distinct().Count();
        var grossSales = orders.Sum(o => o.Amount);
        var currency = orders.First().Currency;

        return new PerformanceTrafficAggregate(
            orders.Count,
            distinctListings,
            totalQuantity,
            grossSales,
            currency);
    }

    public async Task<InventoryDashboardRecord> GetInventoryDashboardAsync(
        Guid sellerId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"performance:inventory-dashboard:{sellerId:N}";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrWhiteSpace(cached))
        {
            var cachedPayload = JsonSerializer.Deserialize<InventoryDashboardRecord>(cached);
            if (cachedPayload is not null)
            {
                return cachedPayload;
            }
        }

        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string summarySql = @"
SELECT
    COUNT(*)::int AS TotalListings,
    COALESCE(SUM(i.available_quantity), 0)::int AS AvailableQuantity,
    COALESCE(SUM(i.reserved_quantity), 0)::int AS ReservedQuantity,
    COALESCE(SUM(i.sold_quantity), 0)::int AS SoldQuantity,
    COUNT(*) FILTER (WHERE i.is_low_stock)::int AS LowStockListings,
    COUNT(*) FILTER (WHERE i.available_quantity <= 0)::int AS OutOfStockListings
FROM inventory i
WHERE i.seller_id = @SellerId;";

        const string criticalSql = @"
SELECT
    i.listing_id AS ListingId,
    COALESCE(l.title, '') AS Title,
    COALESCE(l.sku, '') AS Sku,
    i.available_quantity AS AvailableQuantity,
    i.reserved_quantity AS ReservedQuantity,
    i.sold_quantity AS SoldQuantity,
    i.threshold_quantity AS ThresholdQuantity,
    i.last_updated_at AS LastUpdatedAt
FROM inventory i
LEFT JOIN listings l ON l.id = i.listing_id
WHERE i.seller_id = @SellerId
  AND (i.available_quantity <= 0 OR i.is_low_stock)
ORDER BY i.available_quantity ASC, i.last_updated_at DESC
LIMIT 10;";

        var summary = await connection.QuerySingleAsync<InventoryDashboardSummaryRow>(
            new CommandDefinition(summarySql, new { SellerId = sellerId }, cancellationToken: cancellationToken));

        var criticalListings = (await connection.QueryAsync<InventoryHealthRecord>(
            new CommandDefinition(criticalSql, new { SellerId = sellerId }, cancellationToken: cancellationToken)))
            .ToList();

        var payload = new InventoryDashboardRecord(
            summary.TotalListings,
            summary.AvailableQuantity,
            summary.ReservedQuantity,
            summary.SoldQuantity,
            summary.LowStockListings,
            summary.OutOfStockListings,
            criticalListings);

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(payload),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        return payload;
    }

    public async Task<PerformanceSellerLevelRecord> GetSellerLevelAsync(
        Guid sellerId,
        DateTime asOfUtc,
        CancellationToken cancellationToken = default)
    {
        var normalizedAsOfUtc = asOfUtc.Kind == DateTimeKind.Utc
            ? asOfUtc
            : DateTime.SpecifyKind(asOfUtc, DateTimeKind.Utc);

        var lookbackStartUtc = normalizedAsOfUtc.AddMonths(-12);

        var orders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Status)
            .Where(o =>
                o.SellerId == sellerId &&
                o.OrderedAt >= lookbackStartUtc &&
                o.OrderedAt <= normalizedAsOfUtc &&
                    o.Status.Code != OrderStatusCodes.Draft)
            .Select(o => new
            {
                o.Id,
                o.OrderedAt,
                o.PaidAt,
                o.ShippedAt,
                o.CancelledAt,
                StatusCode = o.Status.Code,
                Amount = o.Total.Amount,
                Currency = o.Total.Currency,
                o.FulfillmentType
            })
            .ToListAsync(cancellationToken);

        if (orders.Count == 0)
        {
            return new PerformanceSellerLevelRecord(
                Region: "US",
                CurrentLevel: SellerPerformanceLevel.BelowStandard.Name,
                EvaluatedTodayLevel: SellerPerformanceLevel.BelowStandard.Name,
                TransactionDefectRate: 0m,
                LateShipmentRate: 0m,
                TrackingUploadedOnTimeRate: 0m,
                CasesClosedWithoutSellerResolution: 0,
                TransactionsLast12Months: 0,
                SalesLast12Months: 0m,
                Currency: "USD",
                NextEvaluationDate: CalculateNextEvaluationDate(normalizedAsOfUtc));
        }

        var fallbackCurrency = orders.First().Currency ?? "USD";

        var orderSnapshots = orders
            .Select(o => new OrderSnapshot(
                o.Id,
                NormalizeToUtc(o.OrderedAt),
                NormalizeToUtc(o.PaidAt),
                NormalizeToUtc(o.ShippedAt),
                NormalizeToUtc(o.CancelledAt),
                o.StatusCode,
                Math.Round(o.Amount, 2, MidpointRounding.AwayFromZero),
                string.IsNullOrWhiteSpace(o.Currency) ? fallbackCurrency : o.Currency,
                o.FulfillmentType))
            .ToList();

        var currency = orderSnapshots.Select(o => o.Currency).FirstOrDefault() ?? fallbackCurrency;
        var paidOrders = orderSnapshots
            .Where(o => o.PaidAtUtc.HasValue && o.PaidAtUtc.Value <= normalizedAsOfUtc)
            .ToList();

        if (paidOrders.Count == 0)
        {
            return new PerformanceSellerLevelRecord(
                Region: "US",
                CurrentLevel: SellerPerformanceLevel.BelowStandard.Name,
                EvaluatedTodayLevel: SellerPerformanceLevel.BelowStandard.Name,
                TransactionDefectRate: 0m,
                LateShipmentRate: 0m,
                TrackingUploadedOnTimeRate: 0m,
                CasesClosedWithoutSellerResolution: 0,
                TransactionsLast12Months: 0,
                SalesLast12Months: 0m,
                Currency: currency,
                NextEvaluationDate: CalculateNextEvaluationDate(normalizedAsOfUtc));
        }

        var orderIds = paidOrders.Select(o => o.OrderId).ToList();

        var shipments = await _context.OrderItemShipments
            .AsNoTracking()
            .Where(s => orderIds.Contains(s.OrderId) && !s.IsDeleted)
            .Select(s => new
            {
                s.OrderId,
                s.ShippedAt,
                s.CreatedAt
            })
            .ToListAsync(cancellationToken);

        var shipmentLookup = shipments
            .GroupBy(s => s.OrderId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => new ShipmentSnapshot(
            NormalizeToUtc(x.ShippedAt),
            NormalizeToUtc(x.CreatedAt)))
                    .OrderBy(s => s.ShipmentDateUtc)
                    .ToList());

        var returns = await _context.ReturnRequests
            .AsNoTracking()
            .Where(r =>
                r.SellerId == sellerId &&
                orderIds.Contains(r.OrderId) &&
                r.RequestedAt >= lookbackStartUtc &&
                r.RequestedAt <= normalizedAsOfUtc)
            .Select(r => new
            {
                r.OrderId,
                r.Reason,
                r.Status,
                r.RequestedAt,
                r.ClosedAt
            })
            .ToListAsync(cancellationToken);

        var cancellations = await _context.CancellationRequests
            .AsNoTracking()
            .Where(c =>
                c.SellerId == sellerId &&
                orderIds.Contains(c.OrderId) &&
                c.RequestedAt >= lookbackStartUtc &&
                c.RequestedAt <= normalizedAsOfUtc)
            .Select(c => new
            {
                c.OrderId,
                c.InitiatedBy,
                c.Reason,
                c.Status,
                c.RequestedAt,
                c.CompletedAt,
                c.AutoClosedAt
            })
            .ToListAsync(cancellationToken);

        var paidOrderIds = paidOrders.Select(o => o.OrderId).ToHashSet();

        var sellerFaultReturnReasons = new HashSet<ReturnReason>
        {
            ReturnReason.NotAsDescribed,
            ReturnReason.DamagedOrDefective,
            ReturnReason.WrongItemReceived,
            ReturnReason.MissingPartsOrAccessories,
            ReturnReason.NotReceived
        };

        var returnDefectOrderIds = new HashSet<Guid>();
        var casesClosedOrderIds = new HashSet<Guid>();

        foreach (var returnRecord in returns)
        {
            if (!sellerFaultReturnReasons.Contains(returnRecord.Reason))
            {
                continue;
            }

            returnDefectOrderIds.Add(returnRecord.OrderId);

            if (returnRecord.ClosedAt.HasValue)
            {
                var closedAtUtc = NormalizeToUtc(returnRecord.ClosedAt.Value);
                if (closedAtUtc <= normalizedAsOfUtc &&
                    (returnRecord.Status == ReturnStatus.RefundCompleted || returnRecord.Status == ReturnStatus.Closed))
                {
                    casesClosedOrderIds.Add(returnRecord.OrderId);
                }
            }
        }

        var cancellationDefectOrderIds = new HashSet<Guid>();

        foreach (var cancellation in cancellations)
        {
            var isSellerInitiated = cancellation.InitiatedBy == CancellationInitiator.Seller ||
                                    cancellation.InitiatedBy == CancellationInitiator.System;

            var isSellerFaultReason = cancellation.Reason == CancellationReason.OutOfStock ||
                                      cancellation.Reason == CancellationReason.PricingError ||
                                      cancellation.Reason == CancellationReason.DuplicateOrder;

            if (isSellerInitiated || (isSellerFaultReason && cancellation.InitiatedBy == CancellationInitiator.Buyer))
            {
                cancellationDefectOrderIds.Add(cancellation.OrderId);
            }

            if (cancellation.Status == CancellationStatus.AutoCancelled && cancellation.AutoClosedAt.HasValue)
            {
                var autoClosedAtUtc = NormalizeToUtc(cancellation.AutoClosedAt.Value);
                if (autoClosedAtUtc <= normalizedAsOfUtc)
                {
                    casesClosedOrderIds.Add(cancellation.OrderId);
                }
            }
            else if (cancellation.InitiatedBy == CancellationInitiator.System && cancellation.CompletedAt.HasValue)
            {
                var completedAtUtc = NormalizeToUtc(cancellation.CompletedAt.Value);
                if (completedAtUtc <= normalizedAsOfUtc)
                {
                    casesClosedOrderIds.Add(cancellation.OrderId);
                }
            }
        }

        var deliveryFailureOrderIds = new HashSet<Guid>(
            paidOrders
                .Where(o => string.Equals(o.StatusCode, OrderStatusCodes.DeliveryFailed, StringComparison.OrdinalIgnoreCase))
                .Select(o => o.OrderId));

        var defectOrderIds = new HashSet<Guid>(returnDefectOrderIds);
        defectOrderIds.UnionWith(cancellationDefectOrderIds);
        defectOrderIds.UnionWith(deliveryFailureOrderIds);
        defectOrderIds.UnionWith(casesClosedOrderIds);
        defectOrderIds.IntersectWith(paidOrderIds);

        casesClosedOrderIds.IntersectWith(paidOrderIds);

        var shippingCandidates = paidOrders
            .Where(o => o.Fulfillment != FulfillmentType.LocalPickup && !o.CancelledAtUtc.HasValue)
            .ToList();

        var shippingEvaluationCount = 0;
        var lateShipmentCount = 0;
        var trackingOnTimeCount = 0;

        foreach (var order in shippingCandidates)
        {
            var baseTime = order.PaidAtUtc ?? order.OrderedAtUtc;
            
            // ❌ XÓA DÒNG NÀY:
            // var handlingDeadline = baseTime.AddHours(DefaultHandlingTimeHours);
            
            // ✅ THAY BẰG:
            var handlingDeadline = BusinessDaysHelper.AddBusinessDays(baseTime, DefaultHandlingTimeDays);

            if (!shipmentLookup.TryGetValue(order.OrderId, out var orderShipments) || orderShipments.Count == 0)
            {
                if (normalizedAsOfUtc <= handlingDeadline)
                {
                    continue;
                }

                shippingEvaluationCount++;
                lateShipmentCount++;
                continue;
            }

            var firstShipmentAt = orderShipments.Min(s => s.ShipmentDateUtc);
            var trackingRecordedAt = orderShipments.Min(s => s.TrackingRecordedUtc);

            if (firstShipmentAt > normalizedAsOfUtc)
            {
                if (normalizedAsOfUtc <= handlingDeadline)
                {
                    continue;
                }

                shippingEvaluationCount++;
                lateShipmentCount++;
                continue;
            }

            shippingEvaluationCount++;

            if (firstShipmentAt > handlingDeadline)
            {
                lateShipmentCount++;
            }

            // ✅ ĐÃ CÓ: Tracking 1-day rule
            var oneDayAfterShipment = firstShipmentAt.AddDays(1);
            
            if (trackingRecordedAt <= handlingDeadline 
                && trackingRecordedAt <= oneDayAfterShipment 
                && trackingRecordedAt <= normalizedAsOfUtc)
            {
                trackingOnTimeCount++;
            }
        }
        
        static decimal ToPercentage(int count, int total)
        {
            if (total == 0 || count == 0)
            {
                return 0m;
            }

            var percentage = count / (decimal)total * 100m;
            return Math.Round(percentage, 2, MidpointRounding.AwayFromZero);
        }

        var transactionsLast12Months = paidOrders.Count;
        var salesLast12Months = paidOrders.Sum(o => o.TotalAmount);
        var transactionDefectRate = ToPercentage(defectOrderIds.Count, transactionsLast12Months);
        var lateShipmentRate = ToPercentage(lateShipmentCount, shippingEvaluationCount);
        var trackingUploadedOnTimeRate = ToPercentage(trackingOnTimeCount, shippingEvaluationCount);

        // ✅ THÊM: Tính Cases Closed Rate
        var casesClosedRate = ToPercentage(casesClosedOrderIds.Count, transactionsLast12Months);

        // ✅ SỬA: Áp dụng tất cả requirements
        var currentLevel = DetermineSellerLevel(
            transactionDefectRate, 
            lateShipmentRate, 
            trackingUploadedOnTimeRate,
            casesClosedRate); // ✅ THÊM PARAMETER

        // ✅ THAY ĐỔI: Kiểm tra minimum requirements đầy đủ
        currentLevel = ApplyMinimumRequirements(
            currentLevel, 
            transactionsLast12Months, 
            salesLast12Months);

        return new PerformanceSellerLevelRecord(
            Region: "US",
            CurrentLevel: currentLevel,
            EvaluatedTodayLevel: currentLevel,
            TransactionDefectRate: transactionDefectRate,
            LateShipmentRate: lateShipmentRate,
            TrackingUploadedOnTimeRate: trackingUploadedOnTimeRate,
            CasesClosedWithoutSellerResolution: casesClosedOrderIds.Count,
            TransactionsLast12Months: transactionsLast12Months,
            SalesLast12Months: Math.Round(salesLast12Months, 2, MidpointRounding.AwayFromZero),
            Currency: currency,
            NextEvaluationDate: CalculateNextEvaluationDate(normalizedAsOfUtc));
    }

    public async Task<ServiceMetricsSourceRecord> GetServiceMetricsSourceAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default)
    {
        var normalizedFromUtc = fromUtc.Kind == DateTimeKind.Utc
            ? fromUtc
            : DateTime.SpecifyKind(fromUtc, DateTimeKind.Utc);

        var normalizedToUtc = toUtc.Kind == DateTimeKind.Utc
            ? toUtc
            : DateTime.SpecifyKind(toUtc, DateTimeKind.Utc);

        if (normalizedToUtc < normalizedFromUtc)
        {
            throw new ArgumentException("The end date must be greater than or equal to the start date.", nameof(toUtc));
        }

        var orders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Status)
            .Where(o =>
                o.SellerId == sellerId &&
                o.OrderedAt >= normalizedFromUtc &&
                o.OrderedAt <= normalizedToUtc &&
                    o.Status.Code != OrderStatusCodes.Draft)
            .Select(o => new ServiceMetricsOrderRecord(
                o.Id,
                o.BuyerId.Value,
                o.OrderedAt,
                o.PaidAt,
                o.ShippedAt,
                o.DeliveredAt,
                o.CancelledAt,
                o.Status.Code,
                o.Total.Amount,
                o.Total.Currency))
            .ToListAsync(cancellationToken);

        if (orders.Count == 0)
        {
            return new ServiceMetricsSourceRecord(
                Array.Empty<ServiceMetricsOrderRecord>(),
                Array.Empty<ServiceMetricsReturnRecord>(),
                Array.Empty<ServiceMetricsCancellationRecord>());
        }

        var orderIds = orders
            .Select(o => o.OrderId)
            .Distinct()
            .ToList();

        var returns = await _context.ReturnRequests
            .AsNoTracking()
            .Where(r =>
                r.SellerId == sellerId &&
                r.RequestedAt >= normalizedFromUtc &&
                r.RequestedAt <= normalizedToUtc &&
                orderIds.Contains(r.OrderId))
            .Select(r => new ServiceMetricsReturnRecord(
                r.Id,
                r.OrderId,
                r.BuyerId,
                r.Reason,
                r.Status,
                r.RequestedAt,
                r.ClosedAt))
            .ToListAsync(cancellationToken);

        var cancellations = await _context.CancellationRequests
            .AsNoTracking()
            .Where(c =>
                c.SellerId == sellerId &&
                c.RequestedAt >= normalizedFromUtc &&
                c.RequestedAt <= normalizedToUtc &&
                orderIds.Contains(c.OrderId))
            .Select(c => new ServiceMetricsCancellationRecord(
                c.Id,
                c.OrderId,
                c.BuyerId,
                c.InitiatedBy,
                c.Reason,
                c.Status,
                c.RequestedAt,
                c.CompletedAt))
            .ToListAsync(cancellationToken);

        return new ServiceMetricsSourceRecord(orders, returns, cancellations);
    }

    private sealed record OrderSnapshot(
        Guid OrderId,
        DateTime OrderedAtUtc,
        DateTime? PaidAtUtc,
        DateTime? ShippedAtUtc,
        DateTime? CancelledAtUtc,
        string StatusCode,
        decimal TotalAmount,
        string Currency,
        FulfillmentType Fulfillment);

    private sealed record ShipmentSnapshot(
        DateTime ShipmentDateUtc,
        DateTime TrackingRecordedUtc);

    private static DateTime NormalizeToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };

    private static DateTime? NormalizeToUtc(DateTime? value) => value.HasValue ? NormalizeToUtc(value.Value) : null;

    private static DateTime NormalizeToUtc(DateTimeOffset value) => value.UtcDateTime;

    private static string DetermineSellerLevel(
        decimal defectRate, 
        decimal lateShipmentRate, 
        decimal trackingRate,
        decimal casesClosedRate) // ✅ THÊM PARAMETER
    {
        // Top Rated Seller: All thresholds must pass
        if (defectRate <= 0.5m 
            && lateShipmentRate <= 2m 
            && trackingRate >= 95m
            && casesClosedRate < 0.3m) // ✅ THÊM: eBay requirement < 0.3%
        {
            return SellerPerformanceLevel.TopRated.Name;
        }

        // Above Standard
        if (defectRate <= 2m 
            && lateShipmentRate <= 4m 
            && trackingRate >= 90m
            && casesClosedRate < 1m) // ✅ THÊM: eBay requirement < 1%
        {
            return SellerPerformanceLevel.AboveStandard.Name;
        }

        return SellerPerformanceLevel.BelowStandard.Name;
    }

    private static string ApplyMinimumRequirements(
    string calculatedLevel,
    int transactions,
    decimal sales)
    {
        // Only Top Rated requires minimum thresholds
        if (calculatedLevel != SellerPerformanceLevel.TopRated.Name)
            return calculatedLevel;
        
        // eBay requirement: Either 100 transactions + $1,000 sales OR 400 transactions
        bool meetsOption1 = transactions >= 100 && sales >= 1000m;
        bool meetsOption2 = transactions >= 400;
        
        if (!meetsOption1 && !meetsOption2)
        {
            // Downgrade to Above Standard if minimum not met
            return SellerPerformanceLevel.AboveStandard.Name;
        }
        
        return calculatedLevel;
    }

    private static DateOnly CalculateNextEvaluationDate(DateTime referenceUtc)
    {
        var referenceDate = DateOnly.FromDateTime(referenceUtc.Date);
        const int evaluationDay = 20;

        if (referenceDate.Day >= evaluationDay)
        {
            var nextMonth = referenceDate.AddMonths(1);
            return new DateOnly(nextMonth.Year, nextMonth.Month, evaluationDay);
        }

        return new DateOnly(referenceDate.Year, referenceDate.Month, evaluationDay);
    }
}

internal sealed record InventoryDashboardSummaryRow(
    int TotalListings,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int LowStockListings,
    int OutOfStockListings);
