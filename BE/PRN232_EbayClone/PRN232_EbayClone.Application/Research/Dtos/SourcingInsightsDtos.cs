namespace PRN232_EbayClone.Application.Research.Dtos;

public sealed record SourcingCategoryDto(
    string Id,
    string Name,
    string Group,
    string Tag,
    decimal OpportunityScore,
    long SearchVolume,
    long ActiveListings,
    decimal SearchToListingRatio,
    decimal SellThroughRate,
    int AverageDaysToFirstSale,
    decimal ReturnRate,
    decimal MarketShare);

public sealed record SourcingInsightsResponseDto(
    IReadOnlyList<SourcingCategoryDto> Categories,
    IReadOnlyCollection<string> SavedCategoryIds,
    int TotalCount);
