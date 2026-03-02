using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Domain.SaleEvents.ValueObjects;

public sealed record SaleEventDiscountTierDefinition(
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    int Priority,
    string? Label,
    IReadOnlyCollection<Guid> ListingIds);
