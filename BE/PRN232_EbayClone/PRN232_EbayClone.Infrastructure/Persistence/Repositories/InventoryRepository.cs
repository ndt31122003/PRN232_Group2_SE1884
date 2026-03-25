using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class InventoryRepository : Repository<Inventory, InventoryId>, IInventoryRepository
{
    public InventoryRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<Inventory?> GetByIdAsync(InventoryId id, CancellationToken cancellationToken = default)
    {
        return DbContext.Inventories
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Inventory?> GetByListingIdAsync(ListingId listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Inventories
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(x => x.ListingId == listingId, cancellationToken);
    }

    public Task<Inventory?> GetByListingIdForUpdateAsync(ListingId listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Inventories
            .FromSqlInterpolated($"SELECT * FROM inventory WHERE listing_id = {listingId.Value} FOR UPDATE")
            .Include(x => x.Reservations)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<Inventory?> GetByReservationIdAsync(InventoryReservationId reservationId, CancellationToken cancellationToken = default)
    {
        return DbContext.Inventories
            .Include(x => x.Reservations)
            .FirstOrDefaultAsync(
                x => x.Reservations.Any(r => r.Id == reservationId),
                cancellationToken);
    }

    public async Task<IReadOnlyList<Inventory>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Inventories
            .AsNoTracking()
            .Include(x => x.Reservations)
            .Where(x => x.SellerId == sellerId)
            .OrderByDescending(x => x.LastUpdatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsForListingAsync(ListingId listingId, CancellationToken cancellationToken = default)
    {
        return DbContext.Inventories.AnyAsync(x => x.ListingId == listingId, cancellationToken);
    }
}