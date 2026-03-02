using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Application.Performance.Records;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceServiceMetricsQuery(Guid SellerId, string? Period = "current")
    : IQuery<PerformanceServiceMetricsDto>;

public sealed class GetPerformanceServiceMetricsQueryHandler(
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetPerformanceServiceMetricsQuery, PerformanceServiceMetricsDto>
{
    public async Task<Result<PerformanceServiceMetricsDto>> Handle(
        GetPerformanceServiceMetricsQuery request,
        CancellationToken cancellationToken)
    {
        var normalizedPeriod = NormalizePeriod(request.Period);
        var nowUtc = DateTime.UtcNow;
        var period = ResolvePeriod(normalizedPeriod, nowUtc);

        var source = await PerformanceRepository.GetServiceMetricsSourceAsync(
            request.SellerId,
            period.FetchStartUtc,
            period.FetchEndUtc,
            cancellationToken);

        DateTime evaluationStartUtc;
        DateTime evaluationEndUtc;

        if (normalizedPeriod == "current")
        {
            evaluationEndUtc = nowUtc;
            evaluationStartUtc = DetermineEvaluationStart(source.Orders, evaluationEndUtc);
            if (evaluationStartUtc < period.RangeStartUtc)
            {
                evaluationStartUtc = period.RangeStartUtc;
            }
        }
        else
        {
            evaluationStartUtc = period.RangeStartUtc;
            evaluationEndUtc = period.RangeEndUtc;
        }

        var calculator = new ServiceMetricsCalculator(source, evaluationStartUtc, evaluationEndUtc);
        var snapshot = calculator.Compute();

        var dto = new PerformanceServiceMetricsDto(
            PeriodKey: normalizedPeriod,
            RangeStart: DateOnly.FromDateTime(evaluationStartUtc.Date),
            RangeEnd: DateOnly.FromDateTime(evaluationEndUtc.Date),
            ItemNotAsDescribed: snapshot.ItemNotAsDescribed,
            ItemNotReceived: snapshot.ItemNotReceived);

        return Result.Success(dto);
    }

    private static string NormalizePeriod(string? rawPeriod)
    {
        return string.IsNullOrWhiteSpace(rawPeriod)
            ? "current"
            : rawPeriod.Trim().ToLowerInvariant() switch
            {
                "previous" => "previous",
                "custom" => "custom",
                _ => "current"
            };
    }

    private static ServiceMetricsPeriod ResolvePeriod(string periodKey, DateTime nowUtc)
    {
        nowUtc = DateTime.SpecifyKind(nowUtc, DateTimeKind.Utc);

        return periodKey switch
        {
            "previous" =>
                new ServiceMetricsPeriod(
                    new DateTime(nowUtc.Year - 1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(nowUtc.Year - 1, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                    new DateTime(nowUtc.Year - 1, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    new DateTime(nowUtc.Year - 1, 12, 31, 23, 59, 59, DateTimeKind.Utc)),
            "custom" =>
                new ServiceMetricsPeriod(
                    nowUtc.AddMonths(-6),
                    nowUtc,
                    nowUtc.AddMonths(-6),
                    nowUtc),
            _ =>
                new ServiceMetricsPeriod(
                    nowUtc.AddMonths(-12),
                    nowUtc,
                    nowUtc.AddMonths(-12),
                    nowUtc)
        };
    }

    private static DateTime DetermineEvaluationStart(IReadOnlyCollection<ServiceMetricsOrderRecord> orders, DateTime rangeEndUtc)
    {
        if (orders.Count == 0)
        {
            return rangeEndUtc.AddMonths(-3);
        }

        var paidOrders = orders
            .Where(o => o.PaidAtUtc.HasValue)
            .ToList();

        if (paidOrders.Count == 0)
        {
            return rangeEndUtc.AddMonths(-3);
        }

        var threeMonthStart = rangeEndUtc.AddMonths(-3);
        var paidLastThreeMonths = paidOrders.Count(o => o.OrderedAtUtc >= threeMonthStart);
        if (paidLastThreeMonths >= 400)
        {
            return threeMonthStart;
        }

        var twelveMonthStart = rangeEndUtc.AddMonths(-12);
        var earliestOrder = paidOrders.Min(o => o.OrderedAtUtc);
        return earliestOrder > twelveMonthStart ? earliestOrder : twelveMonthStart;
    }

    private readonly record struct ServiceMetricsPeriod(
        DateTime RangeStartUtc,
        DateTime RangeEndUtc,
        DateTime FetchStartUtc,
        DateTime FetchEndUtc);
}