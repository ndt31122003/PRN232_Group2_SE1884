namespace PRN232_EbayClone.Application.Listings.Inventory.Services;

public interface IInventoryLowStockNotifier
{
    Task<bool> NotifyIfNeededAsync(
        PRN232_EbayClone.Domain.Listings.Inventory.Entities.Inventory inventory,
        string listingTitle,
        string listingSku,
        CancellationToken cancellationToken);
}