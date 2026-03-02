namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceTrafficDto(
    IReadOnlyList<PerformanceTrafficMetricDto> Metrics,
    IReadOnlyList<string> ChartTabs,
    string ChartType,
    IReadOnlyList<PerformanceTrafficSourceDto> Sources,
    string ListingsEmpty
);

public sealed record PerformanceTrafficSourceDto(
    string Id,
    string Label,
    string Subtitle,
    string Value,
    string Change
);
