namespace PRN232_EbayClone.Domain.Listings.Inventory.Enums;

public enum InventoryReservationType : byte
{
    BuyItNow = 0,
    AuctionWon = 1
}

public enum InventoryReservationStatus : byte
{
    Active = 0,
    Committed = 1,
    Released = 2,
    Expired = 3
}

public enum InventoryAdjustmentType : byte
{
    Restock = 0,
    ManualDecrease = 1,
    Return = 2,
    Commit = 3,
    Release = 4
}
