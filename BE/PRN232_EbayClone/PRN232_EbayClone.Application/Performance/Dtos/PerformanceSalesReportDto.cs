namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceSalesReportDto(
    string UpdatedAt,
    string ReportRange,
    string CompareRange,
    PerformanceBuyerInsightsDto BuyerInsights,
    string EmptySalesMessage,
    string EmptyListingsMessage
);

public sealed record PerformanceBuyerInsightsDto(
    int TotalBuyers,
    string Change,
    int OneTime,
    int Repeat,
    string PercentOfTotal
);
