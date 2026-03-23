using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.ValueObjects;

/// <summary>
/// Result of discount calculation including applied tier and eligible items
/// </summary>
public sealed record DiscountCalculationResult(
    Money DiscountAmount,
    OrderDiscountTier? AppliedTier,
    IReadOnlyList<Guid> EligibleItemIds,
    IReadOnlyList<Guid> ExcludedItemIds,
    string? IneligibilityReason);
