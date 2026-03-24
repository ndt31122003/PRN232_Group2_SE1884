using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IBidRepository : IRepository<Bid, Guid>
{
    Task<List<Bid>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<Bid?> GetHighestBidByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<List<Bid>> GetBySellerIdAsync(string sellerId, Guid? listingId, CancellationToken cancellationToken = default);
}
