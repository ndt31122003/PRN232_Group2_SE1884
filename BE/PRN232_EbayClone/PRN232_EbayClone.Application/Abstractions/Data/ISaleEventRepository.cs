using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISaleEventRepository
{
    Task<SaleEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<SaleEvent>> GetBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<List<SaleEvent>> GetActiveSaleEventsAsync(CancellationToken cancellationToken = default);
    Task<List<SaleEvent>> GetActiveSaleEventsForListingAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<bool> HasBeenAppliedToOrdersAsync(Guid saleEventId, CancellationToken cancellationToken = default);
    Task<SaleEventPriceSnapshot?> GetPriceSnapshotAsync(Guid saleEventId, Guid listingId, CancellationToken cancellationToken = default);
    Task<SaleEventPerformanceMetrics?> GetPerformanceMetricsAsync(Guid saleEventId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task<List<TierPerformanceData>> GetTierPerformanceMetricsAsync(Guid saleEventId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken = default);
    Task AddAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default);
    Task UpdateAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default);
    Task DeleteAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default);
    Task AddPriceSnapshotAsync(SaleEventPriceSnapshot snapshot, CancellationToken cancellationToken = default);
}

public sealed record TierPerformanceData(
    Guid TierId,
    string? TierLabel,
    int Priority,
    int OrderCount,
    decimal TotalDiscountAmount,
    decimal TotalSalesRevenue
);
