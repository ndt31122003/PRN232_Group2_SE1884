using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Queries.GetSellerInventoryAlerts;

public sealed record GetSellerInventoryAlertsQuery() : IQuery<IReadOnlyList<InventoryAlertSettingsDto>>;

public sealed class GetSellerInventoryAlertsQueryHandler : IQueryHandler<GetSellerInventoryAlertsQuery, IReadOnlyList<InventoryAlertSettingsDto>>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetSellerInventoryAlertsQueryHandler(
        IInventoryRepository inventoryRepository,
        IListingRepository listingRepository,
        IUserContext userContext)
    {
        _inventoryRepository = inventoryRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyList<InventoryAlertSettingsDto>>> Handle(GetSellerInventoryAlertsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to access inventory alerts.");
        }

        var sellerId = new UserId(Guid.Parse(userId));
        var inventories = await _inventoryRepository.GetBySellerIdAsync(sellerId, cancellationToken);
        if (inventories.Count == 0)
        {
            return Array.Empty<InventoryAlertSettingsDto>();
        }

        var (activeListings, _) = await _listingRepository.GetActiveListingsAsync(userId, null, null, null, 1, 500, cancellationToken);
        var listingLookup = activeListings.ToDictionary(x => x.Id, x => x);

        var items = inventories
            .Select(inventory =>
            {
                listingLookup.TryGetValue(inventory.ListingId.Value, out var listing);

                return new InventoryAlertSettingsDto(
                    inventory.ListingId.Value,
                    listing?.Title ?? $"Listing {inventory.ListingId.Value}",
                    listing?.Sku ?? "N/A",
                    inventory.AvailableQuantity,
                    inventory.ReservedQuantity,
                    inventory.SoldQuantity,
                    inventory.ThresholdQuantity,
                    inventory.IsLowStock,
                    inventory.EmailNotificationsEnabled,
                    inventory.LastLowStockNotificationAt,
                    inventory.LastUpdatedAt);
            })
            .OrderBy(x => x.Title)
            .ToArray();

        return items;
    }
}