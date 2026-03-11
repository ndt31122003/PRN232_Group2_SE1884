using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SaleEvents.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class SaleEventRepository : Repository<SaleEvent, Guid>, ISaleEventRepository
{
    public SaleEventRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<SaleEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<SaleEvent>()
            .Include(se => se.DiscountTiers)
                .ThenInclude(dt => dt.Listings)
            .AsNoTracking()
            .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
    }

    public Task<bool> NameExistsAsync(string sellerId, string normalizedName, Guid? excludeSaleEventId = null, CancellationToken cancellationToken = default)
    {
        var normalized = normalizedName.Trim().ToLowerInvariant();
        var sellerGuid = Guid.Parse(sellerId);
        var sellerUserId = new UserId(sellerGuid);

        return DbContext.Set<SaleEvent>()
            .AsNoTracking()
            .AnyAsync(se => se.SellerId == sellerUserId && se.Name.ToLower() == normalized && (!excludeSaleEventId.HasValue || se.Id != excludeSaleEventId.Value), cancellationToken);
    }

    public async Task<IReadOnlyList<SaleEvent>> GetSellerSaleEventsAsync(string sellerId, CancellationToken cancellationToken = default)
    {
        var sellerGuid = Guid.Parse(sellerId);
        var sellerUserId = new UserId(sellerGuid);

        return await DbContext.Set<SaleEvent>()
            .Where(se => se.SellerId == sellerUserId)
            .OrderByDescending(se => se.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SaleEvent?> GetByIdForSellerAsync(Guid saleEventId, string sellerId, CancellationToken cancellationToken = default)
    {
        var sellerGuid = Guid.Parse(sellerId);
        var sellerUserId = new UserId(sellerGuid);

        return await DbContext.Set<SaleEvent>()
            .Include(se => se.DiscountTiers)
                .ThenInclude(dt => dt.Listings)
            .AsNoTracking()
            .FirstOrDefaultAsync(se => se.Id == saleEventId && se.SellerId == sellerUserId, cancellationToken);
    }

    public async Task<SaleEvent?> GetByIdForSellerTrackingAsync(Guid saleEventId, string sellerId, CancellationToken cancellationToken = default)
    {
        var sellerGuid = Guid.Parse(sellerId);
        var sellerUserId = new UserId(sellerGuid);

        return await DbContext.Set<SaleEvent>()
            .Include(se => se.DiscountTiers)
                .ThenInclude(dt => dt.Listings)
            .FirstOrDefaultAsync(se => se.Id == saleEventId && se.SellerId == sellerUserId, cancellationToken);
    }
}
