using System;
using PRN232_EbayClone.Application.Performance.Records;

namespace PRN232_EbayClone.Application.Performance.Queries;

internal static class TrafficMetricCalculator
{
    private const int ImpressionPerListing = 120;
    private const int ImpressionPerOrder = 45;
    private const int ImpressionPerUnit = 30;
    private const int ViewPerListing = 40;
    private const int ViewPerOrder = 18;
    private const int ViewPerUnit = 8;

    internal static TrafficSnapshot FromAggregate(PerformanceTrafficAggregate aggregate)
    {
        var impressions = (aggregate.DistinctListings * ImpressionPerListing)
            + (aggregate.OrderCount * ImpressionPerOrder)
            + (aggregate.QuantitySold * ImpressionPerUnit);

        var listingViews = (aggregate.DistinctListings * ViewPerListing)
            + (aggregate.OrderCount * ViewPerOrder)
            + (aggregate.QuantitySold * ViewPerUnit);

        if (aggregate.QuantitySold > 0)
        {
            listingViews = Math.Max(listingViews, aggregate.QuantitySold * ViewPerUnit);
        }

        if (aggregate.OrderCount > 0 || aggregate.QuantitySold > 0)
        {
            listingViews = Math.Max(listingViews, aggregate.OrderCount * ViewPerOrder);
            impressions = Math.Max(impressions, listingViews + aggregate.OrderCount * ImpressionPerOrder);
        }

        var top20Impressions = (int)Math.Round(impressions * 0.62m, MidpointRounding.AwayFromZero);
        var nonSearchImpressions = Math.Max(0, impressions - top20Impressions);

        var ebayViews = (int)Math.Round(listingViews * 0.74m, MidpointRounding.AwayFromZero);
        var externalViews = Math.Max(0, listingViews - ebayViews);

        var clickThrough = listingViews == 0
            ? 0m
            : Math.Round((decimal)aggregate.QuantitySold / listingViews * 100m, 1, MidpointRounding.AwayFromZero);

        var conversion = impressions == 0
            ? 0m
            : Math.Round((decimal)aggregate.QuantitySold / impressions * 100m, 1, MidpointRounding.AwayFromZero);

        return new TrafficSnapshot(
            aggregate.OrderCount,
            aggregate.DistinctListings,
            aggregate.QuantitySold,
            aggregate.GrossSales,
            aggregate.Currency,
            impressions,
            top20Impressions,
            nonSearchImpressions,
            listingViews,
            ebayViews,
            externalViews,
            clickThrough,
            conversion);
    }

    internal sealed record TrafficSnapshot(
        int Orders,
        int DistinctListings,
        int QuantitySold,
        decimal GrossSales,
        string Currency,
        int Impressions,
        int Top20SearchImpressions,
        int NonSearchImpressions,
        int ListingViews,
        int EbayViews,
        int ExternalViews,
        decimal ClickThroughRate,
        decimal ConversionRate)
    {
        internal bool HasActivity => Orders > 0 || QuantitySold > 0 || DistinctListings > 0;
    }
}
