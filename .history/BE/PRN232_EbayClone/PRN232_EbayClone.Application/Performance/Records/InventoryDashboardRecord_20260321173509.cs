namespace PRN232_EbayClone.Application.Performance.Records;

public sealed record InventoryHealthRecord(
    Guid ListingId,
    string Title,
    string Sku,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int? ThresholdQuantity,
    DateTime LastUpdatedAt);

public sealed record InventoryDashboardRecord(
    int TotalListings,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int LowStockListings,
    int OutOfStockListings,
    IReadOnlyList<InventoryHealthRecord> CriticalListings);