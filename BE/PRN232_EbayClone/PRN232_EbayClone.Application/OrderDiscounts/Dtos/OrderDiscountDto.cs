using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.OrderDiscounts.Dtos;

public sealed record OrderDiscountDto(
    Guid Id,
    string Name,
    string? Description,
    OrderDiscountThresholdType ThresholdType,
    decimal? ThresholdAmount,
    int? ThresholdQuantity,
    decimal DiscountValue,
    DiscountUnit DiscountUnit,
    decimal? MaxDiscount,
    bool ApplyToAllItems,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    IReadOnlyList<OrderDiscountTierDto> Tiers,
    int IncludedItemCount,
    int ExcludedItemCount,
    int IncludedCategoryCount,
    int ExcludedCategoryCount);
