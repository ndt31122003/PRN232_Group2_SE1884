using PRN232_EbayClone.Domain.SaleEvents.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISaleEventRepository : IRepository<SaleEvent, Guid>
{
    Task<bool> NameExistsAsync(string sellerId, string normalizedName, Guid? excludeSaleEventId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SaleEvent>> GetSellerSaleEventsAsync(string sellerId, CancellationToken cancellationToken = default);
    Task<SaleEvent?> GetByIdForSellerAsync(Guid saleEventId, string sellerId, CancellationToken cancellationToken = default);
    Task<SaleEvent?> GetByIdForSellerTrackingAsync(Guid saleEventId, string sellerId, CancellationToken cancellationToken = default);
}
