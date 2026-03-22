using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class OfferRepository : Repository<Offer, Guid>, IOfferRepository
{
    public OfferRepository(ApplicationDbContext dbContext, IDbConnectionFactory connectionFactory) 
        : base(dbContext, connectionFactory)
    {
    }

    public override Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Offers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public Task<List<Offer>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Offers
            .Where(o => o.ListingId == listingId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Offer>> GetBySellerIdAsync(string sellerId, Guid? listingId, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Offers
            .AsNoTracking()
            .Include(o => o.Listing)
            .ThenInclude(l => (l as FixedPriceListing)!.Pricing)
            .Include(o => o.Listing)
            .ThenInclude(l => l.Images)
            .Where(o => o.Listing.CreatedBy == sellerId);

        if (listingId.HasValue)
        {
            query = query.Where(o => o.ListingId == listingId.Value);
        }

        return query
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
