using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceServiceMetricsDto(
    string PeriodKey,
    DateOnly RangeStart,
    DateOnly RangeEnd,
    ServiceMetricDto ItemNotAsDescribed,
    ServiceMetricDto ItemNotReceived);

public sealed record ServiceMetricDto(
    string MetricId,
    int TotalTransactions,
    int IssueCount,
    decimal Rate,
    string RateLabel,
    decimal PeerRate,
    string PeerRateLabel,
    string Classification,
    int UniqueBuyerCount,
    IReadOnlyList<ServiceMetricReasonDto> Reasons);

public sealed record ServiceMetricReasonDto(
    string Id,
    string Label,
    int Count);