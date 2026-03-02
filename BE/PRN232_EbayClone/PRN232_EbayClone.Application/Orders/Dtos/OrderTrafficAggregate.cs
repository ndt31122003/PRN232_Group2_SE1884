namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderTrafficAggregate(
    int OrderCount,
    int DistinctListings,
    int QuantitySold,
    decimal GrossSales,
    string Currency
);
