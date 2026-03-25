using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class VolumePricingRepository : Repository<VolumePricing, Guid>, IVolumePricingRepository
{
    public VolumePricingRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<VolumePricing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<VolumePricing>()
            .Include(v => v.Tiers)
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<VolumePricing>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<VolumePricing>()
            .Include(v => v.Tiers)
            .Where(v => v.SellerId == sellerId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VolumePricing>> GetByListingIdAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<VolumePricing>()
            .Include(v => v.Tiers)
            .Where(v => v.ListingId == listingId || v.ListingId == null)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VolumePricing>> GetActiveForListingAsync(Guid listingId, DateTime currentDate, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<VolumePricing>()
            .Include(v => v.Tiers)
            .Where(v => v.IsActive &&
                        v.StartDate <= currentDate &&
                        v.EndDate >= currentDate &&
                        (v.ListingId == null || v.ListingId == listingId))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(VolumePricing pricing, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<VolumePricing>().AddAsync(pricing, cancellationToken);
    }

    public Task UpdateAsync(VolumePricing pricing, CancellationToken cancellationToken = default)
    {
        DbContext.Set<VolumePricing>().Update(pricing);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pricing = await GetByIdAsync(id, cancellationToken);
        if (pricing != null)
        {
            DbContext.Set<VolumePricing>().Remove(pricing);
        }
    }
}
