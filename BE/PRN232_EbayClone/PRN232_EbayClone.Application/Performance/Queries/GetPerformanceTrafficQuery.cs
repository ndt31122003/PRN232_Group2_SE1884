using System;
using System.Collections.Generic;
using System.Globalization;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using TrafficSnapshot = PRN232_EbayClone.Application.Performance.Queries.TrafficMetricCalculator.TrafficSnapshot;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceTrafficQuery(Guid SellerId, string? Period) : IQuery<PerformanceTrafficDto>;

public sealed class GetPerformanceTrafficQueryHandler(
    IUserRepository UserRepository,
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetPerformanceTrafficQuery, PerformanceTrafficDto>
{
    public async Task<Result<PerformanceTrafficDto>> Handle(GetPerformanceTrafficQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var user = await UserRepository.GetByIdAsync(sellerId, cancellationToken);
        if (user is null)
        {
            return Error.Failure("Performance.Traffic.UserNotFound", "Seller not found.");
        }

        var nowUtc = DateTime.UtcNow;
        var normalizedPeriod = NormalizePeriod(request.Period);
        var periodRange = BuildRange(normalizedPeriod, nowUtc);
        var compareRange = BuildPriorRange(periodRange);

        var currentAggregate = await PerformanceRepository.GetTrafficAggregateAsync(
            request.SellerId,
            ToUtcDateTime(periodRange.Start, TimeOnly.MinValue),
            ToUtcDateTime(periodRange.End, TimeOnly.MaxValue),
            cancellationToken);

        var previousAggregate = await PerformanceRepository.GetTrafficAggregateAsync(
            request.SellerId,
            ToUtcDateTime(compareRange.Start, TimeOnly.MinValue),
            ToUtcDateTime(compareRange.End, TimeOnly.MaxValue),
            cancellationToken);

        var currentSnapshot = TrafficMetricCalculator.FromAggregate(currentAggregate);
        var previousSnapshot = TrafficMetricCalculator.FromAggregate(previousAggregate);

        var changeSuffix = normalizedPeriod == "last_90_days" ? "vs prior 90 days" : "vs prior 30 days";
        var metrics = BuildMetrics(currentSnapshot, previousSnapshot, changeSuffix);
        var sources = BuildSources(currentSnapshot, previousSnapshot, changeSuffix);

        var listingsMessage = currentSnapshot.HasActivity
            ? "Listing-level traffic insights are coming soon."
            : "We couldn't find any sales in the selected time period. Try changing the date range or filters.";

        var traffic = new PerformanceTrafficDto(
            metrics,
            new[] { "Impressions", "Listing views", "Quantity sold" },
            currentSnapshot.HasActivity ? "Line chart" : "Bar chart",
            sources,
            listingsMessage);

        return Result.Success(traffic);
    }

    private static string NormalizePeriod(string? period)
    {
        return string.IsNullOrWhiteSpace(period)
            ? "past_30_days"
            : period.Trim().ToLowerInvariant();
    }

    private static (DateOnly Start, DateOnly End, int LengthInDays) BuildRange(string period, DateTime nowUtc)
    {
        var today = DateOnly.FromDateTime(nowUtc.Date);
        return period switch
        {
            "last_90_days" => (today.AddDays(-89), today, 90),
            _ => (today.AddDays(-29), today, 30)
        };
    }

    private static (DateOnly Start, DateOnly End, int LengthInDays) BuildPriorRange((DateOnly Start, DateOnly End, int LengthInDays) currentRange)
    {
        var compareEnd = currentRange.Start.AddDays(-1);
        var compareStart = compareEnd.AddDays(-(currentRange.LengthInDays - 1));
        return (compareStart, compareEnd, currentRange.LengthInDays);
    }

    private static IReadOnlyList<PerformanceTrafficMetricDto> BuildMetrics(TrafficSnapshot current, TrafficSnapshot previous, string changeSuffix)
    {
        var impressionsChange = FormatPercentageChange(current.Impressions, previous.Impressions);
        var listingViewsChange = FormatPercentageChange(current.ListingViews, previous.ListingViews);
        var quantityChange = FormatPercentageChange(current.QuantitySold, previous.QuantitySold);
        var ctrChange = FormatPercentageChange(current.ClickThroughRate, previous.ClickThroughRate);
        var conversionChange = FormatPercentageChange(current.ConversionRate, previous.ConversionRate);

        return new List<PerformanceTrafficMetricDto>
        {
            new("impressions", "Impressions", FormatNumber(current.Impressions), $"{impressionsChange} {changeSuffix}"),
            new("listingViews", "Listing views", FormatNumber(current.ListingViews), $"{listingViewsChange} {changeSuffix}"),
            new("quantitySold", "Quantity sold", FormatNumber(current.QuantitySold), $"{quantityChange} {changeSuffix}"),
            new("ctr", "Click-through rate", FormatRate(current.ClickThroughRate), $"{ctrChange} {changeSuffix}"),
            new("conversion", "Sales conversion rate", FormatRate(current.ConversionRate), $"{conversionChange} {changeSuffix}")
        };
    }

    private static IReadOnlyList<PerformanceTrafficSourceDto> BuildSources(TrafficSnapshot current, TrafficSnapshot previous, string changeSuffix)
    {
        var (organicCurrent, promotedCurrent, offsiteCurrent) = SplitImpressions(current.Impressions);
        var (organicPrevious, promotedPrevious, offsitePrevious) = SplitImpressions(previous.Impressions);

        var organicChange = FormatPercentageChange(organicCurrent, organicPrevious);
        var promotedChange = FormatPercentageChange(promotedCurrent, promotedPrevious);
        var offsiteChange = FormatPercentageChange(offsiteCurrent, offsitePrevious);

        return new List<PerformanceTrafficSourceDto>
        {
            new("organic", "Organic", "impressions on eBay", $"{FormatNumber(organicCurrent)} impressions", $"{organicChange} {changeSuffix}"),
            new("promoted", "Promoted Listings", "impressions on eBay and the eBay Network", $"{FormatNumber(promotedCurrent)} impressions", $"{promotedChange} {changeSuffix}"),
            new("offsite", "Promoted Offsite", "impressions off eBay", $"{FormatNumber(offsiteCurrent)} impressions", $"{offsiteChange} {changeSuffix}")
        };
    }

    private static (int Organic, int Promoted, int Offsite) SplitImpressions(int impressions)
    {
        if (impressions <= 0)
        {
            return (0, 0, 0);
        }

        var organic = (int)Math.Round(impressions * 0.58m, MidpointRounding.AwayFromZero);
        var promoted = (int)Math.Round(impressions * 0.28m, MidpointRounding.AwayFromZero);
        var offsite = Math.Max(0, impressions - organic - promoted);
        return (organic, promoted, offsite);
    }

    private static string FormatNumber(int value)
        => value.ToString("N0", CultureInfo.InvariantCulture);

    private static string FormatRate(decimal percentage)
    {
        var rounded = Math.Round(percentage, 1, MidpointRounding.AwayFromZero);
        return $"{rounded:0.0}%";
    }

    private static string FormatPercentageChange(int current, int previous)
    {
        if (previous == 0)
        {
            return current == 0 ? "0.0%" : "N/A";
        }

        var change = (current - previous) / (decimal)previous * 100m;
        var rounded = Math.Round(change, 1, MidpointRounding.AwayFromZero);
        return $"{(rounded >= 0 ? "+" : string.Empty)}{rounded}%";
    }

    private static string FormatPercentageChange(decimal current, decimal previous)
    {
        if (previous == 0m)
        {
            return current == 0m ? "0.0%" : "N/A";
        }

        var change = (current - previous) / previous * 100m;
        var rounded = Math.Round(change, 1, MidpointRounding.AwayFromZero);
        return $"{(rounded >= 0 ? "+" : string.Empty)}{rounded}%";
    }

    private static DateTime ToUtcDateTime(DateOnly date, TimeOnly time)
        => DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Utc);
}
