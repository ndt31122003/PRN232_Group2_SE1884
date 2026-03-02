namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderOverviewRecord(
    DateTime OrderedAt,
    DateTime? PaidAt,
    string StatusCode,
    decimal TotalAmount,
    string Currency
);
