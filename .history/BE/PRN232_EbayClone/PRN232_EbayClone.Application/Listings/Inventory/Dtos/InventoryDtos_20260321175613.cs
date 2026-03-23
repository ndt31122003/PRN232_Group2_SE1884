using InventoryEntity = PRN232_EbayClone.Domain.Listings.Inventory.Entities.Inventory;
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;

namespace PRN232_EbayClone.Application.Listings.Inventory.Dtos;

public sealed record InventoryReservationDto(
    Guid ReservationId,
    Guid InventoryId,
    Guid BuyerId,
    Guid? OrderId,
    InventoryReservationType ReservationType,
    InventoryReservationStatus Status,
    int Quantity,
    DateTime ReservedAt,
    DateTime ExpiresAt,
    DateTime? ReleasedAt,
    DateTime? CommittedAt);

public sealed record InventoryDto(
    Guid InventoryId,
    Guid ListingId,
    Guid SellerId,
    int TotalQuantity,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int? ThresholdQuantity,
    bool IsLowStock,
    DateTime? LastLowStockNotificationAt,
    DateTime LastUpdatedAt,
    IReadOnlyCollection<InventoryReservationDto> Reservations);

public static class InventoryMappings
{
    public static InventoryDto ToDto(this InventoryEntity entity)
    {
        return new InventoryDto(
            entity.Id.Value,
            entity.ListingId.Value,
            entity.SellerId.Value,
            entity.TotalQuantity,
            entity.AvailableQuantity,
            entity.ReservedQuantity,
            entity.SoldQuantity,
            entity.ThresholdQuantity,
            entity.IsLowStock,
            entity.LastLowStockNotificationAt,
            entity.LastUpdatedAt,
            entity.Reservations
                .OrderByDescending(x => x.ReservedAt)
                .Select(x => new InventoryReservationDto(
                    x.Id.Value,
                    x.InventoryId.Value,
                    x.BuyerId.Value,
                    x.OrderId,
                    x.ReservationType,
                    x.Status,
                    x.Quantity,
                    x.ReservedAt,
                    x.ExpiresAt,
                    x.ReleasedAt,
                    x.CommittedAt))
                .ToArray());
    }
}