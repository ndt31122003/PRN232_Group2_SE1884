using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Domain.Discounts.ValueObjects;

/// <summary>
/// Defines a discount tier configuration for sale event creation
/// </summary>
public record SaleEventDiscountTierDefinition(
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    int Priority,
    string? Label,
    List<Guid> ListingIds
);

/// <summary>
/// Result of sale price calculation for a listing
/// </summary>
public record SalePriceCalculationResult(
    decimal SalePrice,
    decimal OriginalPrice,
    decimal DiscountAmount,
    string? AppliedTierLabel,
    string? IneligibilityReason
)
{
    public bool IsEligible => IneligibilityReason == null;
}
