using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IInventoryRepository : IRepository<Inventory, InventoryId>
{
    Task<Inventory?> GetByListingIdAsync(ListingId listingId, CancellationToken cancellationToken = default);
    Task<Inventory?> GetByListingIdForUpdateAsync(ListingId listingId, CancellationToken cancellationToken = default);
    Task<Inventory?> GetByReservationIdAsync(InventoryReservationId reservationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForListingAsync(ListingId listingId, CancellationToken cancellationToken = default);
}