namespace PRN232_EbayClone.Application.Performance.Records;

public sealed record PerformanceTrafficAggregate(
    int OrderCount,
    int DistinctListings,
    int QuantitySold,
    decimal GrossSales,
    string Currency
);
