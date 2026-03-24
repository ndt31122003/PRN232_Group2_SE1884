using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class BidRepository : Repository<Bid, Guid>, IBidRepository
{
    public BidRepository(ApplicationDbContext dbContext, IDbConnectionFactory connectionFactory) 
        : base(dbContext, connectionFactory)
    {
    }

    public override Task<Bid?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Bids.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public Task<List<Bid>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Bids
            .Where(b => b.ListingId == listingId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Bid?> GetHighestBidByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Bids
            .Where(b => b.ListingId == listingId)
            .OrderByDescending(b => b.Amount)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<List<Bid>> GetBySellerIdAsync(string sellerId, Guid? listingId, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Bids
            .AsNoTracking()
            .Include(b => b.Listing)
            .ThenInclude(l => (l as AuctionListing)!.Pricing)
            .Include(b => b.Listing)
            .ThenInclude(l => l.Images)
            .Where(b => b.Listing.CreatedBy == sellerId);

        if (listingId.HasValue)
        {
            query = query.Where(b => b.ListingId == listingId.Value);
        }

        return query.OrderByDescending(b => b.CreatedAt).ToListAsync(cancellationToken);
    }
}
