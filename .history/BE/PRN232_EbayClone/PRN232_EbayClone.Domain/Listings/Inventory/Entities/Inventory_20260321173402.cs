using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Entities;

public sealed class Inventory : AggregateRoot<InventoryId>
{
    // Properties
    public ListingId ListingId { get; private set; }
    public UserId SellerId { get; private set; }
    
    public int TotalQuantity { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int SoldQuantity { get; private set; }
    
    public int? ThresholdQuantity { get; private set; }
    public bool IsLowStock { get; private set; }
    public DateTime? LastLowStockNotificationAt { get; private set; }
    
    public DateTime LastUpdatedAt { get; private set; }
    
    private readonly List<InventoryReservation> _reservations = [];
    public IReadOnlyCollection<InventoryReservation> Reservations => _reservations.AsReadOnly();
    
    private Inventory(InventoryId id) : base(id)
    {
    }
    
    // UC1.1: Initialize Inventory
    public static Result<Inventory> Create(
        ListingId listingId,
        UserId sellerId,
        int initialQuantity)
    {
        if (initialQuantity < 0)
            return InventoryErrors.InvalidQuantity;
        
        var inventory = new Inventory(InventoryId.New())
        {
            ListingId = listingId,
            SellerId = sellerId,
            TotalQuantity = initialQuantity,
            AvailableQuantity = initialQuantity,
            ReservedQuantity = 0,
            SoldQuantity = 0,
            ThresholdQuantity = null,
            IsLowStock = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = null,
            UpdatedAt = null,
            UpdatedBy = null,
            LastUpdatedAt = DateTime.UtcNow
        };
        
        return inventory;
    }
    
    // UC1.2: Restock
    public Result Restock(int quantityToAdd, string? reason = null)
    {
        if (quantityToAdd <= 0)
            return InventoryErrors.InvalidQuantity;
        
        TotalQuantity += quantityToAdd;
        AvailableQuantity += quantityToAdd;
        LastUpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    // UC1.2: Adjust Stock (Manual Decrease)
    public Result AdjustStock(int quantityToDecrease, string? reason = null)
    {
        if (quantityToDecrease <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToDecrease)
            return InventoryErrors.InsufficientAvailableStock;
        
        TotalQuantity -= quantityToDecrease;
        AvailableQuantity -= quantityToDecrease;
        LastUpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    // UC1.3: Set Low-Stock Threshold
    public Result SetLowStockThreshold(int threshold)
    {
        if (threshold <= 0)
            return InventoryErrors.InvalidThreshold;
        
        ThresholdQuantity = threshold;
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    private void UpdateLowStockStatus()
    {
        if (ThresholdQuantity.HasValue && AvailableQuantity <= ThresholdQuantity.Value)
        {
            IsLowStock = true;
            if (!LastLowStockNotificationAt.HasValue)
            {
                LastLowStockNotificationAt = DateTime.UtcNow;
            }
        }
        else
        {
            IsLowStock = false;
        }
    }
    
    // UC2.2: Reserve Stock
    public Result ReserveStock(
        int quantityToReserve,
        UserId buyerId,
        DateTime expiresAt,
        InventoryReservationType reservationType = InventoryReservationType.BuyItNow,
        Guid? orderId = null)
    {
        if (quantityToReserve <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToReserve)
            return InventoryErrors.OutOfStock;
        
        AvailableQuantity -= quantityToReserve;
        ReservedQuantity += quantityToReserve;
        LastUpdatedAt = DateTime.UtcNow;
        
        var reservation = InventoryReservation.Create(
            InventoryReservationId.New(),
            Id,
            orderId,
            buyerId,
            reservationType,
            quantityToReserve,
            expiresAt);
        
        _reservations.Add(reservation);
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.4: Commit Stock
    public Result CommitStock(int quantityToCommit, InventoryReservationId reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        if (!reservation.IsActive)
            return InventoryErrors.InvalidStateTransition;
        
        SoldQuantity += quantityToCommit;
        ReservedQuantity -= quantityToCommit;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Commit();
        
        return Result.Success();
    }
    
    // UC2.5: Release Stock
    public Result ReleaseStock(int quantityToRelease, InventoryReservationId reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        if (ReservedQuantity < quantityToRelease)
            return InventoryErrors.InvalidQuantity;
        
        AvailableQuantity += quantityToRelease;
        ReservedQuantity -= quantityToRelease;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Release();
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.6: Restock from Return
    public Result RestockFromReturn(int quantityToRestock)
    {
        if (quantityToRestock <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (SoldQuantity < quantityToRestock)
            return InventoryErrors.InvalidQuantity;
        
        TotalQuantity += quantityToRestock;
        AvailableQuantity += quantityToRestock;
        SoldQuantity -= quantityToRestock;
        LastUpdatedAt = DateTime.UtcNow;
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
}
