using System;

namespace PRN232_EbayClone.Application.Performance.Records;

public sealed record PerformanceOverviewRecord(
    DateTime OrderedAt,
    DateTime? PaidAt,
    string StatusCode,
    decimal TotalAmount,
    string Currency
);
