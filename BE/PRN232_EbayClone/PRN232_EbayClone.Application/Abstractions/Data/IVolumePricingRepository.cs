using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IVolumePricingRepository
{
    Task<VolumePricing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<VolumePricing>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VolumePricing>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VolumePricing>> GetActiveForListingAsync(Guid listingId, DateTime currentDate, CancellationToken cancellationToken = default);
    Task AddAsync(VolumePricing pricing, CancellationToken cancellationToken = default);
    Task UpdateAsync(VolumePricing pricing, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
