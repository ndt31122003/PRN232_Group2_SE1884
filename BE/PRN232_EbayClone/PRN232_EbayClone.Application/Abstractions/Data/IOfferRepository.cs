using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IOfferRepository : IRepository<Offer, Guid>
{
    Task<List<Offer>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<List<Offer>> GetBySellerIdAsync(string sellerId, Guid? listingId, CancellationToken cancellationToken = default);
}
