using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventTierDto(
    Guid Id,
    int Priority,
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    string? Label,
    IReadOnlyCollection<Guid> ListingIds);
