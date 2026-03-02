using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Performance.Records;

namespace PRN232_EbayClone.Application.Performance.Abstractions;

public interface IPerformanceRepository
{
    Task<IReadOnlyList<PerformanceOverviewRecord>> GetOverviewRecordsAsync(
        Guid sellerId,
        DateTime fromDateUtc,
        CancellationToken cancellationToken = default);

    Task<PerformanceTrafficAggregate> GetTrafficAggregateAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PerformancePaymentRecord>> GetPaymentRecordsAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default);

    Task<PerformanceSellerLevelRecord> GetSellerLevelAsync(
        Guid sellerId,
        DateTime asOfUtc,
        CancellationToken cancellationToken = default);

    Task<ServiceMetricsSourceRecord> GetServiceMetricsSourceAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default);
}
