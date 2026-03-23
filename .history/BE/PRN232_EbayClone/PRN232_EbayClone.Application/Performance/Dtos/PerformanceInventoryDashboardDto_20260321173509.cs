namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceInventoryDashboardDto(
    int TotalListings,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int LowStockListings,
    int OutOfStockListings,
    IReadOnlyList<PerformanceInventoryHealthDto> CriticalListings);

public sealed record PerformanceInventoryHealthDto(
    Guid ListingId,
    string Title,
    string Sku,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    int? ThresholdQuantity,
    DateTime LastUpdatedAt);