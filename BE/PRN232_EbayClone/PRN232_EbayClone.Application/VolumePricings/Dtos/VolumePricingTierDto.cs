using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.VolumePricings.Dtos;

public sealed record VolumePricingTierDto(
    Guid Id,
    int MinQuantity,
    decimal DiscountValue,
    DiscountUnit DiscountUnit);
