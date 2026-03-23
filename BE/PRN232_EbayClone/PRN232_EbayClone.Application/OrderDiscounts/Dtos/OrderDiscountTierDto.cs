namespace PRN232_EbayClone.Application.OrderDiscounts.Dtos;

public sealed record OrderDiscountTierDto(
    Guid Id,
    decimal ThresholdValue,
    decimal DiscountValue,
    int TierOrder);
