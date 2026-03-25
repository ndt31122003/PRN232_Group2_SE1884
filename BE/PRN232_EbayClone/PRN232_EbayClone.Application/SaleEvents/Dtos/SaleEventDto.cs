using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventDto(
    Guid Id,
    string Name,
    string? Description,
    string? BuyerMessageLabel,
    SaleEventMode Mode,
    SaleEventStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    bool OfferFreeShipping,
    bool BlockPriceIncreaseRevisions,
    bool IncludeSkippedItems,
    decimal? HighlightPercentage,
    IReadOnlyList<SaleEventDiscountTierDto> DiscountTiers,
    int TotalListingsCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record SaleEventDiscountTierDto(
    Guid Id,
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    int Priority,
    string? Label,
    int ListingCount
);

public sealed record SaleEventPerformanceDto(
    Guid SaleEventId,
    string SaleEventName,
    int OrderCount,
    decimal TotalDiscountAmount,
    decimal TotalSalesRevenue,
    int TotalItemsSold,
    decimal AverageDiscountPerOrder,
    decimal AverageOrderValue,
    DateTime LastUpdated,
    IReadOnlyList<TierPerformanceDto> TierPerformance
);

public sealed record TierPerformanceDto(
    Guid TierId,
    string? TierLabel,
    int Priority,
    int OrderCount,
    decimal TotalDiscountAmount,
    decimal TotalSalesRevenue
);

public sealed record ListingDto(
    Guid Id,
    string Title,
    decimal Price,
    string Currency,
    Guid CategoryId,
    string CategoryName,
    bool IsActive,
    DateTime CreatedAt
);
