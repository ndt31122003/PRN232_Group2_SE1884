using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Entities;

public sealed class InventoryAdjustment : Entity<Guid>
{
    public InventoryId InventoryId { get; private set; }
    public UserId SellerId { get; private set; }
    public InventoryAdjustmentType AdjustmentType { get; private set; }
    public int QuantityChange { get; private set; }  // Positive/negative
    public string? Reason { get; private set; }
    public DateTime AdjustedAt { get; private set; }
    public string? AdjustedBy { get; private set; }
    
    private InventoryAdjustment(Guid id) : base(id) { }
    
    public static InventoryAdjustment Create(
        InventoryId inventoryId,
        UserId sellerId,
        InventoryAdjustmentType type,
        int quantityChange,
        string? reason = null,
        string? adjustedBy = null)
    {
        return new InventoryAdjustment(Guid.NewGuid())
        {
            InventoryId = inventoryId,
            SellerId = sellerId,
            AdjustmentType = type,
            QuantityChange = quantityChange,
            Reason = reason,
            AdjustedAt = DateTime.UtcNow,
            AdjustedBy = adjustedBy
        };
    }
}
