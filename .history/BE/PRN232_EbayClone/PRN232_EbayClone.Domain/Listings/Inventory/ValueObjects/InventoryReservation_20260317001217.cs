using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryReservation
{
    public InventoryReservationId Id { get; private set; }
    public InventoryId InventoryId { get; private set; }
    public Guid? OrderId { get; private set; }
    public UserId BuyerId { get; private set; }
    public InventoryReservationType ReservationType { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ReservedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }
    public DateTime? CommittedAt { get; private set; }
    
    public InventoryReservationStatus Status
    {
        get
        {
            if (CommittedAt.HasValue) return InventoryReservationStatus.Committed;
            if (ReleasedAt.HasValue) return InventoryReservationStatus.Released;
            if (DateTime.UtcNow > ExpiresAt) return InventoryReservationStatus.Expired;
            return InventoryReservationStatus.Active;
        }
    }
    
    public bool IsActive => Status == InventoryReservationStatus.Active;
    
    private InventoryReservation()
    {
        Id = null!;
        InventoryId = null!;
        BuyerId = null!;
    }
    
    public static InventoryReservation Create(
        InventoryReservationId id,
        InventoryId inventoryId,
        Guid? orderId,
        UserId buyerId,
        InventoryReservationType type,
        int quantity,
        DateTime expiresAt)
    {
        return new InventoryReservation
        {
            Id = id,
            InventoryId = inventoryId,
            OrderId = orderId,
            BuyerId = buyerId,
            ReservationType = type,
            Quantity = quantity,
            ReservedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            ReleasedAt = null,
            CommittedAt = null
        };
    }
    
    public void Commit() => CommittedAt = DateTime.UtcNow;
    public void Release() => ReleasedAt = DateTime.UtcNow;
    
    public override bool Equals(object? obj)
    {
        if (obj is not InventoryReservation other) return false;
        return Id.Equals(other.Id);
    }
    
    public override int GetHashCode() => Id.GetHashCode();
}
