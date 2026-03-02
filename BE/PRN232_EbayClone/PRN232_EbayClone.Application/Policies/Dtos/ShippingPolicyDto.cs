namespace PRN232_EbayClone.Application.Policies.Dtos;

public sealed record ShippingPolicyDto(
    Guid Id,
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays,
    bool IsDefault
);

