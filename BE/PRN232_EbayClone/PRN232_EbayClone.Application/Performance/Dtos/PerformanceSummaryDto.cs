namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceSummaryDto(
    PerformanceSalesSummaryDto Sales,
    IReadOnlyList<PerformanceSellingCostDto> SellingCosts,
    IReadOnlyList<PerformanceTrafficMetricDto> Traffic,
    PerformanceSellerLevelDto SellerLevel
);

public sealed record PerformanceSalesSummaryDto(
    string ChartCaption,
    string Note,
    IReadOnlyList<PerformanceSalesPeriodDto> Periods
);

public sealed record PerformanceSalesPeriodDto(
    string Id,
    string Label,
    decimal Amount
);

public sealed record PerformanceSellingCostDto(
    string Id,
    string Label,
    decimal Amount,
    bool Accent,
    IReadOnlyList<PerformanceSellingCostDto>? Children
);

public sealed record PerformanceTrafficMetricDto(
    string Id,
    string Label,
    string Value,
    string Change
);

public sealed record PerformanceSellerLevelDto(
    string Title,
    string Message
);
