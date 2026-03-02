using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventSummaryDto(
    Guid Id,
    string Name,
    SaleEventStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    int TierCount,
    int ListingCount,
    bool OfferFreeShipping,
    bool IncludeSkippedItems,
    bool BlockPriceIncreaseRevisions,
    decimal? HighlightPercentage);
