using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using System.Globalization;

namespace PRN232_EbayClone.Application.Research.Queries;

public sealed record GetProductResearchQuery(
    string? Keyword,
    int Days,
    int Page,
    int PageSize,
    ProductResearchFilters Filters) : IQuery<ProductResearchResponseDto>;

public sealed record ProductResearchFilters(
    ListingFormat? ListingFormat,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool FreeShippingOnly,
    Guid? CategoryId)
{
    public static ProductResearchFilters Empty { get; } = new(null, null, null, false, null);
}

public sealed class GetProductResearchQueryHandler(
    IListingRepository ListingRepository,
    IOrderRepository OrderRepository) 
    : IQueryHandler<GetProductResearchQuery, ProductResearchResponseDto>
{
    private const string DefaultCurrency = "USD";
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;
    private static readonly HashSet<int> AllowedDayRanges = new([7, 14, 30, 90]);

    public async Task<Result<ProductResearchResponseDto>> Handle(GetProductResearchQuery request, CancellationToken cancellationToken)
    {
        var sanitizedKeyword = string.IsNullOrWhiteSpace(request.Keyword)
            ? null
            : request.Keyword!.Trim();

        var rangeDays = AllowedDayRanges.Contains(request.Days) ? request.Days : 30;
        var page = request.Page > 0 ? request.Page : 1;
        var pageSize = request.PageSize > 0 ? Math.Min(request.PageSize, MaxPageSize) : DefaultPageSize;
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var rangeStart = today.AddDays(-(rangeDays - 1));
        var fromUtc = rangeStart.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var toUtc = today.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        var filters = request.Filters ?? ProductResearchFilters.Empty;

        var orderItems = await OrderRepository.GetProductResearchOrderItemsAsync(
            null,
            fromUtc,
            toUtc,
            sanitizedKeyword,
            filters.CategoryId,
            cancellationToken);

        var listingIds = orderItems
            .Select(item => item.ListingId)
            .Distinct()
            .ToList();

        var listingFormats = listingIds.Count > 0
            ? await ListingRepository.GetListingFormatsAsync(listingIds, cancellationToken)
            : new Dictionary<Guid, ListingFormat>();

        var activeListingsPage = await ListingRepository.GetProductResearchActiveListingsAsync(
            sanitizedKeyword,
            page,
            pageSize,
            filters.CategoryId,
            filters.ListingFormat,
            filters.MinPrice,
            filters.MaxPrice,
            cancellationToken);
        var filteredOrderItems = ApplySoldFilters(orderItems, filters, listingFormats);

        var range = new ProductResearchRangeDto(rangeStart, today, BuildRangeLabel(rangeDays));
        var soldPanel = BuildSoldPanel(range, filteredOrderItems, listingFormats, activeListingsPage.TotalCount);
        var activePanel = BuildActivePanel(range, activeListingsPage, page, pageSize);

        var response = new ProductResearchResponseDto(range, soldPanel, activePanel);
        return Result.Success(response);
    }

    private static ProductResearchPanelDto BuildSoldPanel(
        ProductResearchRangeDto range,
        IReadOnlyList<ProductResearchOrderItemRecord> orderItems,
        IReadOnlyDictionary<Guid, ListingFormat> listingFormats,
        int activeListingsTotalCount)
    {
        var soldListingCount = orderItems
            .Select(item => item.ListingId)
            .Distinct()
            .Count();

        if (orderItems.Count == 0)
        {
            var emptyPagination = new ProductResearchPaginationDto(1, 0, 0);

            return new ProductResearchPanelDto(
                Summary: new List<ProductResearchSummaryMetricDto>
                {
                    new("avgSoldPrice", "Avg sold price", "-"),
                    new("priceRange", "Sold price range", "-"),
                    new("avgShipping", "Avg shipping", "-"),
                    new("freeShipping", "Free shipping", "-"),
                    new("sellThrough", "Sell-through", FormatPercentage(CalculateSellThrough(0, activeListingsTotalCount))),
                    new("uniqueBuyers", "Unique buyers", "0")
                },
                Trend: BuildEmptyTrend(range),
                Listings: Array.Empty<ProductResearchListingDto>(),
        Pagination: emptyPagination);
        }

        var currency = orderItems.FirstOrDefault()?.UnitPriceCurrency ?? DefaultCurrency;
        var shippingCurrency = orderItems.FirstOrDefault()?.ShippingCurrency ?? currency;

        var totalQuantity = orderItems.Sum(item => item.Quantity);
        var totalSales = orderItems.Sum(item => item.TotalPriceAmount);
        var averageSoldPrice = totalQuantity > 0 ? totalSales / totalQuantity : (decimal?)null;
        var minPrice = orderItems.Min(item => item.UnitPriceAmount);
        var maxPrice = orderItems.Max(item => item.UnitPriceAmount);

        var ordersByOrderId = orderItems
            .GroupBy(item => item.OrderId)
            .Select(group => new
            {
                ShippingAmount = group.First().ShippingAmount,
                group.First().ShippingCurrency
            })
            .ToList();

        var averageShipping = ordersByOrderId.Count > 0
            ? ordersByOrderId.Average(order => order.ShippingAmount)
            : (decimal?)null;

        var freeShippingRate = ordersByOrderId.Count > 0
            ? ordersByOrderId.Count(order => order.ShippingAmount == 0m) / (decimal)ordersByOrderId.Count
            : (decimal?)null;

        var uniqueBuyers = orderItems.Select(item => item.BuyerId).Distinct().Count();
        var sellThrough = CalculateSellThrough(soldListingCount, activeListingsTotalCount);

        var summary = new List<ProductResearchSummaryMetricDto>
        {
            new("avgSoldPrice", "Avg sold price", FormatCurrency(averageSoldPrice, currency)),
            new("priceRange", "Sold price range", FormatRange(minPrice, maxPrice, currency)),
            new("avgShipping", "Avg shipping", FormatCurrency(averageShipping, shippingCurrency)),
            new("freeShipping", "Free shipping", FormatPercentage(freeShippingRate)),
            new("sellThrough", "Sell-through", FormatPercentage(sellThrough)),
            new("uniqueBuyers", "Unique buyers", uniqueBuyers.ToString("N0", CultureInfo.InvariantCulture))
        };

        var trend = BuildSoldTrend(range, orderItems);
        var listings = BuildSoldListings(orderItems, listingFormats, currency, shippingCurrency);
        var pagination = new ProductResearchPaginationDto(1, listings.Count, listings.Count);

        return new ProductResearchPanelDto(summary, trend, listings, pagination);
    }

    private static ProductResearchPanelDto BuildActivePanel(
        ProductResearchRangeDto range,
        ProductResearchActiveListingsPage activeListingsPage,
        int page,
        int pageSize)
    {
        var pagination = new ProductResearchPaginationDto(page, pageSize, activeListingsPage.TotalCount);

        if (activeListingsPage.TotalCount == 0)
        {
            return new ProductResearchPanelDto(
                Summary: new List<ProductResearchSummaryMetricDto>
                {
                    new("avgListingPrice", "Avg listing price", "-"),
                    new("priceRange", "Listing price range", "-"),
                    new("avgShipping", "Avg shipping", "-"),
                    new("freeShipping", "Free shipping", "-"),
                    new("totalListings", "Total active listings", "0"),
                    new("promoted", "Promoted listings", "0%")
                },
                Trend: BuildEmptyTrend(range),
                Listings: Array.Empty<ProductResearchListingDto>(),
                Pagination: pagination);
        }

        var currency = string.IsNullOrWhiteSpace(activeListingsPage.Currency)
            ? DefaultCurrency
            : activeListingsPage.Currency;
        var averagePrice = activeListingsPage.AveragePrice ?? (activeListingsPage.Items.Count > 0
            ? activeListingsPage.Items.Average(listing => listing.PriceAmount)
            : (decimal?)null);
        var minPrice = activeListingsPage.MinPrice ?? (activeListingsPage.Items.Count > 0
            ? activeListingsPage.Items.Min(listing => listing.PriceAmount)
            : (decimal?)null);
        var maxPrice = activeListingsPage.MaxPrice ?? (activeListingsPage.Items.Count > 0
            ? activeListingsPage.Items.Max(listing => listing.PriceAmount)
            : (decimal?)null);

        var summary = new List<ProductResearchSummaryMetricDto>
        {
            new("avgListingPrice", "Avg listing price", FormatCurrency(averagePrice, currency)),
            new("priceRange", "Listing price range", FormatRange(minPrice, maxPrice, currency)),
            new("avgShipping", "Avg shipping", "-"),
            new("freeShipping", "Free shipping", "-"),
            new("totalListings", "Total active listings", activeListingsPage.TotalCount.ToString("N0", CultureInfo.InvariantCulture)),
            new("promoted", "Promoted listings", "0%")
        };

        var trend = BuildFlatTrend(range, averagePrice ?? 0m);
        var listings = BuildActiveListings(activeListingsPage.Items, currency);

        return new ProductResearchPanelDto(summary, trend, listings, pagination);
    }

    private static IReadOnlyList<ProductResearchTrendPointDto> BuildSoldTrend(
        ProductResearchRangeDto range,
        IReadOnlyList<ProductResearchOrderItemRecord> orderItems)
    {
        var dailyAggregation = orderItems
            .GroupBy(item => DateOnly.FromDateTime((item.PaidAtUtc ?? item.OrderedAtUtc).Date))
            .ToDictionary(
                group => group.Key,
                group => new
                {
                    TotalAmount = group.Sum(item => item.TotalPriceAmount),
                    TotalQuantity = group.Sum(item => item.Quantity)
                });

        var trend = new List<ProductResearchTrendPointDto>();
        for (var date = range.From; date <= range.To; date = date.AddDays(1))
        {
            if (dailyAggregation.TryGetValue(date, out var aggregate) && aggregate.TotalQuantity > 0)
            {
                var avg = aggregate.TotalAmount / aggregate.TotalQuantity;
                trend.Add(new ProductResearchTrendPointDto(date, Math.Round(avg, 2, MidpointRounding.AwayFromZero)));
            }
            else
            {
                trend.Add(new ProductResearchTrendPointDto(date, 0m));
            }
        }

        return trend;
    }

    private static IReadOnlyList<ProductResearchTrendPointDto> BuildFlatTrend(
        ProductResearchRangeDto range,
        decimal averagePrice)
    {
        var rounded = Math.Round(averagePrice, 2, MidpointRounding.AwayFromZero);
        var points = new List<ProductResearchTrendPointDto>();
        for (var date = range.From; date <= range.To; date = date.AddDays(1))
        {
            points.Add(new ProductResearchTrendPointDto(date, rounded));
        }

        return points;
    }

    private static IReadOnlyList<ProductResearchTrendPointDto> BuildEmptyTrend(ProductResearchRangeDto range)
    {
        var points = new List<ProductResearchTrendPointDto>();
        for (var date = range.From; date <= range.To; date = date.AddDays(1))
        {
            points.Add(new ProductResearchTrendPointDto(date, 0m));
        }

        return points;
    }

    private static IReadOnlyList<ProductResearchListingDto> BuildSoldListings(
        IReadOnlyList<ProductResearchOrderItemRecord> orderItems,
        IReadOnlyDictionary<Guid, ListingFormat> listingFormats,
        string priceCurrency,
        string shippingCurrency)
    {
        return orderItems
            .GroupBy(item => item.ListingId)
            .Select(group =>
            {
                var totalQuantity = group.Sum(item => item.Quantity);
                var totalSales = group.Sum(item => item.TotalPriceAmount);
                var averagePrice = totalQuantity > 0 ? totalSales / totalQuantity : 0m;
                var latestItem = group
                    .OrderByDescending(item => item.PaidAtUtc ?? item.OrderedAtUtc)
                    .ThenByDescending(item => item.OrderedAtUtc)
                    .First();

                var shippingAverage = group
                    .GroupBy(item => item.OrderId)
                    .Select(orderGroup => orderGroup.First().ShippingAmount)
                    .DefaultIfEmpty(0m)
                    .Average();

                listingFormats.TryGetValue(group.Key, out var format);
                var pricingType = format switch
                {
                    ListingFormat.Auction => "Auction",
                    ListingFormat.FixedPrice => "Fixed price",
                    _ => null
                };

                return new ProductResearchListingDto(
                    group.Key,
                    latestItem.Title,
                    new ProductResearchMoneyDto(Math.Round(averagePrice, 2, MidpointRounding.AwayFromZero), priceCurrency ?? DefaultCurrency),
                    new ProductResearchMoneyDto(Math.Round(shippingAverage, 2, MidpointRounding.AwayFromZero), shippingCurrency ?? priceCurrency ?? DefaultCurrency),
                    totalQuantity,
                    new ProductResearchMoneyDto(Math.Round(totalSales, 2, MidpointRounding.AwayFromZero), priceCurrency ?? DefaultCurrency),
                    null,
                    null,
                    pricingType,
                    null,
                    latestItem.PaidAtUtc ?? latestItem.OrderedAtUtc,
                    null,
                    latestItem.ImageUrl);
            })
            .OrderByDescending(listing => listing.TotalSold)
            .ThenByDescending(listing => listing.LastSoldAt)
            .Take(20)
            .ToList();
    }

    private static IReadOnlyList<ProductResearchListingDto> BuildActiveListings(
        IReadOnlyList<ProductResearchActiveListingRecord> activeListings,
        string currency)
    {
        return activeListings
            .Select(listing => new ProductResearchListingDto(
                listing.ListingId,
                listing.Title,
                new ProductResearchMoneyDto(Math.Round(listing.PriceAmount, 2, MidpointRounding.AwayFromZero), currency ?? DefaultCurrency),
                null,
                null,
                null,
                listing.AvailableQuantity,
                listing.IsMultiVariation,
                listing.Format switch
                {
                    ListingFormat.Auction => "Auction",
                    ListingFormat.FixedPrice => "Fixed price",
                    _ => null
                },
                null,
                null,
                listing.StartDate ?? listing.CreatedAt,
                listing.ImageUrl))
            .ToList();
    }

    private static IReadOnlyList<ProductResearchOrderItemRecord> ApplySoldFilters(
        IEnumerable<ProductResearchOrderItemRecord> source,
        ProductResearchFilters filters,
        IReadOnlyDictionary<Guid, ListingFormat> listingFormats)
    {
        return source
            .Where(item => MatchesFormat(listingFormats, item.ListingId, filters.ListingFormat))
            .Where(item => filters.CategoryId is null || item.CategoryId == filters.CategoryId.Value)
            .Where(item => filters.MinPrice is null || item.UnitPriceAmount >= filters.MinPrice.Value)
            .Where(item => filters.MaxPrice is null || item.UnitPriceAmount <= filters.MaxPrice.Value)
            .Where(item => !filters.FreeShippingOnly || item.ShippingAmount == 0m)
            .ToList();
    }

    private static bool MatchesFormat(
        IReadOnlyDictionary<Guid, ListingFormat> listingFormats,
        Guid listingId,
        ListingFormat? filter)
    {
        if (filter is null)
        {
            return true;
        }

        return listingFormats.TryGetValue(listingId, out var format) && format == filter.Value;
    }

    private static decimal? CalculateSellThrough(int soldListings, int activeListings)
    {
        var denominator = soldListings + activeListings;
        if (denominator == 0)
        {
            return null;
        }

        return soldListings / (decimal)denominator;
    }

    private static string FormatCurrency(decimal? amount, string? currency)
    {
        if (!amount.HasValue)
        {
            return "-";
        }

        return FormatCurrencyValue(amount.Value, currency);
    }

    private static string FormatRange(decimal? min, decimal? max, string? currency)
    {
        if (!min.HasValue || !max.HasValue)
        {
            return "-";
        }

        var formattedMin = FormatCurrencyValue(min.Value, currency);
        var formattedMax = FormatCurrencyValue(max.Value, currency);
        return $"{formattedMin} - {formattedMax}";
    }

    private static string FormatPercentage(decimal? ratio)
    {
        if (!ratio.HasValue)
        {
            return "-";
        }

        var percent = Math.Round(ratio.Value * 100m, 1, MidpointRounding.AwayFromZero);
        return percent.ToString("0.0'%'", CultureInfo.InvariantCulture);
    }

    private static string FormatCurrencyValue(decimal amount, string? currency)
    {
        var info = CultureInfo.GetCultureInfo("en-US");
        var formatted = amount.ToString("C2", info);
        if (string.IsNullOrWhiteSpace(currency) || string.Equals(currency, "USD", StringComparison.OrdinalIgnoreCase))
        {
            return formatted;
        }

        return $"{formatted} {currency}";
    }

    private static string BuildRangeLabel(int days)
    {
        return days switch
        {
            7 => "Last 7 days",
            14 => "Last 14 days",
            90 => "Last 90 days",
            _ => "Last 30 days"
        };
    }
}
