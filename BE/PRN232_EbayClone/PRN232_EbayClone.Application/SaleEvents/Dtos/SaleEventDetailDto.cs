using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventDetailDto(
    Guid Id,
    string Name,
    string? Description,
    SaleEventMode Mode,
    SaleEventStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    bool OfferFreeShipping,
    bool IncludeSkippedItems,
    bool BlockPriceIncreaseRevisions,
    decimal? HighlightPercentage,
    IReadOnlyCollection<SaleEventTierDto> Tiers);
