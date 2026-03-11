using Dapper;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Dtos;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.SaleEvents.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using System.Data;
using System.Globalization;
using System.Text.Json;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ListingRepository :
    Repository<Listing, Guid>,
    IListingRepository
{
    private const string DefaultCurrency = "USD";

    public ListingRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    private static IQueryable<TListing> FilterByOwner<TListing>(IQueryable<TListing> query, string ownerId)
        where TListing : Listing
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);
        return query.Where(listing => listing.CreatedBy == ownerId);
    }

    public override async Task<Listing?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var fixedPriceListing = await DbContext.Listings
            .OfType<FixedPriceListing>()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        if (fixedPriceListing is not null)
            return fixedPriceListing;

        var auctionListing = await DbContext.Listings
            .OfType<AuctionListing>()
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

        return auctionListing;
    }


    public Task<List<Listing>> GetListingsToActivateAsync(DateTime now, int batchSize, CancellationToken cancellationToken)
    {
        return DbContext.Listings
            .Where(l => l.Status == ListingStatus.Scheduled
                        && l.ScheduledStartTime <= now
                        && l.StartDate == null)
            .OrderBy(l => l.ScheduledStartTime)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }
    public Task<List<Listing>> GetListingsToEndAsync(DateTime now, int batchSize, CancellationToken cancellationToken)
    {
        return DbContext.Listings
            .Where(l => l.Status == ListingStatus.Active
                && l.EndDate != null
                && l.EndDate <= now)
            .OrderBy(l => l.EndDate)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IReadOnlyList<ActiveListingDto> Items, int TotalCount)> GetActiveListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var baseQuery = ApplySearchFilter(
            FilterByOwner(DbContext.Listings.AsNoTracking().Where(l => l.Status == ListingStatus.Active), ownerId),
            searchTerm);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Array.Empty<ActiveListingDto>(), 0);
        }

        var orderedQuery = baseQuery
            .OrderByDescending(l => l.UpdatedAt ?? l.CreatedAt)
            .ThenBy(l => l.Id);

        var listingIds = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var discountLookup = await GetSaleEventDiscountLabelsAsync(listingIds, ownerId, cancellationToken);

        var soldQuantities = await DbContext.OrderItems
            .AsNoTracking()
            .Where(oi => listingIds.Contains(oi.ListingId))
            .GroupBy(oi => oi.ListingId)
            .Select(g => new { ListingId = g.Key, Quantity = g.Sum(oi => oi.Quantity) })
            .ToDictionaryAsync(x => x.ListingId, x => x.Quantity, cancellationToken);

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var dtoLookup = new Dictionary<Guid, ActiveListingDto>(listingIds.Count);

        foreach (var listing in fixedPriceListings)
        {
            var availableQuantity = listing.Type == ListingType.Single
                ? listing.Pricing.Quantity
                : listing.Variations.Sum(v => v.Quantity);

            var soldQuantity = soldQuantities.TryGetValue(listing.Id, out var sold)
                ? sold
                : 0;

            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;
            var discountLabel = discountLookup.TryGetValue(listing.Id, out var label) ? label : null;

            dtoLookup[listing.Id] = new ActiveListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                availableQuantity,
                soldQuantity,
                listing.Duration,
                listing.Pricing.Price,
                discountLabel,
                listing.Pricing.Price,
                0m,
                0m,
                listing.StartDate,
                listing.EndDate);
        }

        foreach (var listing in auctionListings)
        {
            var soldQuantity = soldQuantities.TryGetValue(listing.Id, out var sold)
                ? sold
                : 0;

            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;
            var discountLabel = discountLookup.TryGetValue(listing.Id, out var label) ? label : null;

            dtoLookup[listing.Id] = new ActiveListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                listing.Pricing.Quantity,
                soldQuantity,
                listing.Duration,
                listing.Pricing.StartPrice,
                discountLabel,
                listing.Pricing.StartPrice,
                listing.Pricing.ReservePrice ?? 0m,
                0m,
                listing.StartDate,
                listing.EndDate);
        }

        var orderedDtos = listingIds
            .Where(dtoLookup.ContainsKey)
            .Select(id => dtoLookup[id])
            .ToList();

        return (orderedDtos, totalCount);
    }

    public async Task<(IReadOnlyList<DraftListingDto> Items, int TotalCount)> GetDraftListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var baseQuery = ApplySearchFilter(
            FilterByOwner(DbContext.Listings.AsNoTracking().Where(l => l.Status == ListingStatus.Draft), ownerId),
            searchTerm);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Array.Empty<DraftListingDto>(), 0);
        }

        var listingIds = await baseQuery
            .OrderByDescending(l => l.UpdatedAt ?? l.CreatedAt)
            .ThenBy(l => l.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var dtoLookup = new Dictionary<Guid, DraftListingDto>(listingIds.Count);

        foreach (var listing in fixedPriceListings)
        {
            var availableQuantity = listing.Type == ListingType.Single
                ? listing.Pricing.Quantity
                : listing.Variations.Sum(v => v.Quantity);

            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;

            dtoLookup[listing.Id] = new DraftListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                availableQuantity,
                listing.Pricing.Price,
                listing.Pricing.Price,
                0m,
                listing.Duration,
                listing.CreatedAt,
                listing.UpdatedAt,
                listing.DraftExpiredAt);
        }

        foreach (var listing in auctionListings)
        {
            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;

            dtoLookup[listing.Id] = new DraftListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                listing.Pricing.Quantity,
                listing.Pricing.StartPrice,
                listing.Pricing.BuyItNowPrice ?? 0m,
                0m,
                listing.Duration,
                listing.CreatedAt,
                listing.UpdatedAt,
                listing.DraftExpiredAt);
        }

        var orderedDtos = listingIds
            .Where(dtoLookup.ContainsKey)
            .Select(id => dtoLookup[id])
            .ToList();

        return (orderedDtos, totalCount);
    }

    public async Task<(IReadOnlyList<ScheduledListingDto> Items, int TotalCount)> GetScheduledListingsAsync(string ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var baseQuery = ApplySearchFilter(
            FilterByOwner(DbContext.Listings.AsNoTracking().Where(l => l.Status == ListingStatus.Scheduled), ownerId),
            searchTerm);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Array.Empty<ScheduledListingDto>(), 0);
        }

        var listingIds = await baseQuery
            .OrderBy(l => l.ScheduledStartTime ?? l.CreatedAt)
            .ThenBy(l => l.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var dtoLookup = new Dictionary<Guid, ScheduledListingDto>(listingIds.Count);

        foreach (var listing in fixedPriceListings)
        {
            var availableQuantity = listing.Type == ListingType.Single
                ? listing.Pricing.Quantity
                : listing.Variations.Sum(v => v.Quantity);

            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;

            dtoLookup[listing.Id] = new ScheduledListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                availableQuantity,
                listing.Duration,
                listing.Pricing.Price,
                listing.Pricing.Price,
                0m,
                0m,
                listing.ScheduledStartTime);
        }

        foreach (var listing in auctionListings)
        {
            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;

            dtoLookup[listing.Id] = new ScheduledListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                listing.Pricing.Quantity,
                listing.Duration,
                listing.Pricing.StartPrice,
                listing.Pricing.StartPrice,
                listing.Pricing.ReservePrice ?? 0m,
                0m,
                listing.ScheduledStartTime);
        }

        var orderedDtos = listingIds
            .Where(dtoLookup.ContainsKey)
            .Select(id => dtoLookup[id])
            .ToList();

        return (orderedDtos, totalCount);
    }

    public async Task<(IReadOnlyList<EndedListingDto> Items, int TotalCount)> GetEndedListingsAsync(
        string ownerId,
        string? searchTerm,
        SoldStatus? soldStatus,
        RelistStatus? relistStatus,
        DateTime? fromDate,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var baseQuery = ApplySearchFilter(
            FilterByOwner(DbContext.Listings.AsNoTracking().Where(l => l.Status == ListingStatus.Ended), ownerId),
            searchTerm);

        if (fromDate.HasValue)
        {
            baseQuery = baseQuery.Where(l => l.EndDate >= fromDate);
        }

        var query = baseQuery.Select(l => new
        {
            Listing = l,
            SoldQuantity = DbContext.OrderItems
                .Where(oi => oi.ListingId == l.Id)
                .Sum(oi => (int?)oi.Quantity) ?? 0,
            HasRelisted = DbContext.Listings
                .Any(other => other.Id != l.Id
                               && other.Sku == l.Sku
                               && other.Status != ListingStatus.Ended)
        });

        if (soldStatus.HasValue)
        {
            query = soldStatus.Value switch
            {
                SoldStatus.Sold => query.Where(x => x.SoldQuantity > 0),
                SoldStatus.Unsold => query.Where(x => x.SoldQuantity == 0),
                _ => query
            };
        }

        if (relistStatus.HasValue)
        {
            query = relistStatus.Value switch
            {
                RelistStatus.Relisted => query.Where(x => x.HasRelisted),
                RelistStatus.NotRelisted => query.Where(x => !x.HasRelisted),
                _ => query
            };
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Array.Empty<EndedListingDto>(), 0);
        }

        var pageData = await query
            .OrderByDescending(x => x.Listing.EndDate ?? x.Listing.CreatedAt)
            .ThenBy(x => x.Listing.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new { x.Listing.Id, x.SoldQuantity, x.HasRelisted })
            .ToListAsync(cancellationToken);

        var listingIds = pageData.Select(x => x.Id).ToList();
        var soldDict = pageData.ToDictionary(x => x.Id, x => x.SoldQuantity);
        var relistedSet = pageData.Where(x => x.HasRelisted).Select(x => x.Id).ToHashSet();

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Where(l => listingIds.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var dtoLookup = new Dictionary<Guid, EndedListingDto>(listingIds.Count);

        foreach (var listing in fixedPriceListings)
        {
            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;
            var soldQuantity = soldDict.TryGetValue(listing.Id, out var sold) ? sold : 0;
            var soldState = soldQuantity > 0 ? SoldStatus.Sold : SoldStatus.Unsold;
            var relistState = relistedSet.Contains(listing.Id) ? RelistStatus.Relisted : RelistStatus.NotRelisted;
            var availableQuantity = listing.Type == ListingType.Single
                ? listing.Pricing.Quantity
                : listing.Variations.Sum(v => v.Quantity);

            dtoLookup[listing.Id] = new EndedListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                availableQuantity,
                listing.Duration,
                soldState,
                relistState,
                listing.Pricing.Price,
                listing.Pricing.Price,
                0m,
                listing.StartDate,
                listing.EndDate);
        }

        foreach (var listing in auctionListings)
        {
            var thumbnail = GetPrimaryImageUrl(listing) ?? string.Empty;
            var soldQuantity = soldDict.TryGetValue(listing.Id, out var sold) ? sold : 0;
            var soldState = soldQuantity > 0 ? SoldStatus.Sold : SoldStatus.Unsold;
            var relistState = relistedSet.Contains(listing.Id) ? RelistStatus.Relisted : RelistStatus.NotRelisted;

            dtoLookup[listing.Id] = new EndedListingDto(
                listing.Id,
                listing.Title,
                thumbnail,
                listing.Sku,
                listing.Format,
                listing.Pricing.Quantity,
                listing.Duration,
                soldState,
                relistState,
                listing.Pricing.StartPrice,
                listing.Pricing.StartPrice,
                listing.Pricing.ReservePrice ?? 0m,
                listing.StartDate,
                listing.EndDate);
        }

        var orderedDtos = listingIds
            .Where(dtoLookup.ContainsKey)
            .Select(id => dtoLookup[id])
            .ToList();

        return (orderedDtos, totalCount);
    }

    public async Task<IReadOnlyList<ListingExportRow>> GetListingsForExportAsync(
        string ownerId,
        IReadOnlyCollection<Guid>? listingIds,
        IReadOnlyCollection<ListingStatus>? statuses,
        string? searchTerm,
        int maxRows,
        CancellationToken cancellationToken)
    {
        maxRows = Math.Clamp(maxRows, 1, 5000);

        IQueryable<Listing> query = FilterByOwner(DbContext.Listings.AsNoTracking(), ownerId);

        if (listingIds is not null && listingIds.Count > 0)
        {
            query = query.Where(l => listingIds.Contains(l.Id));
        }
        else if (statuses is not null && statuses.Count > 0)
        {
            query = query.Where(l => statuses.Contains(l.Status));
        }
        else
        {
            query = query.Where(l => l.Status == ListingStatus.Active
                                      || l.Status == ListingStatus.Draft
                                      || l.Status == ListingStatus.Scheduled);
        }

        query = ApplySearchFilter(query, searchTerm);

        var listingIdsToLoad = await query
            .OrderByDescending(l => l.UpdatedAt ?? l.CreatedAt)
            .ThenBy(l => l.Id)
            .Take(maxRows)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        if (listingIdsToLoad.Count == 0)
        {
            return Array.Empty<ListingExportRow>();
        }

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Variations)
            .Where(l => listingIdsToLoad.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Where(l => listingIdsToLoad.Contains(l.Id))
            .ToListAsync(cancellationToken);

        var exportLookup = new Dictionary<Guid, ListingExportRow>(listingIdsToLoad.Count);

        foreach (var listing in fixedPriceListings)
        {
            var isMultiVariation = listing.Type == ListingType.MultiVariation;
            int? quantity = null;
            decimal? price = null;
            var variationSummary = default(string?);

            if (isMultiVariation)
            {
                quantity = listing.Variations.Sum(v => v.Quantity);
                variationSummary = string.Join("; ", listing.Variations.Select(v => $"{v.Sku}: {v.Price} x {v.Quantity}"));
            }
            else if (listing.Pricing is not null)
            {
                quantity = listing.Pricing.Quantity;
                price = listing.Pricing.Price;
            }

            exportLookup[listing.Id] = new ListingExportRow(
                listing.Id,
                listing.Status,
                listing.Format,
                listing.Title,
                listing.Sku,
                quantity,
                price,
                null,
                null,
                null,
                listing.ScheduledStartTime,
                isMultiVariation,
                variationSummary);
        }

        foreach (var listing in auctionListings)
        {
            exportLookup[listing.Id] = new ListingExportRow(
                listing.Id,
                listing.Status,
                listing.Format,
                listing.Title,
                listing.Sku,
                listing.Pricing.Quantity,
                null,
                listing.Pricing.StartPrice,
                listing.Pricing.BuyItNowPrice,
                listing.Pricing.ReservePrice,
                listing.ScheduledStartTime,
                false,
                null);
        }

        var orderedRows = listingIdsToLoad
            .Where(exportLookup.ContainsKey)
            .Select(id => exportLookup[id])
            .ToList();

        return orderedRows;
    }

    private async Task<Dictionary<Guid, string>> GetSaleEventDiscountLabelsAsync(
        IReadOnlyCollection<Guid> listingIds,
        string ownerId,
        CancellationToken cancellationToken)
    {
        if (listingIds is null || listingIds.Count == 0 || string.IsNullOrWhiteSpace(ownerId))
        {
            return new Dictionary<Guid, string>();
        }

        if (!Guid.TryParse(ownerId, out var ownerGuid))
        {
            return new Dictionary<Guid, string>();
        }

        var sellerId = new UserId(ownerGuid);

        var discountAssignments = await (
                from listing in DbContext.SaleEventListings.AsNoTracking()
                join tier in DbContext.SaleEventDiscountTiers.AsNoTracking() on listing.DiscountTierId equals tier.Id
                join saleEvent in DbContext.SaleEvents.AsNoTracking() on listing.SaleEventId equals saleEvent.Id
                where listingIds.Contains(listing.ListingId)
                      && saleEvent.SellerId == sellerId
                      && saleEvent.Mode == SaleEventMode.DiscountAndSaleEvent
                      && (saleEvent.Status == SaleEventStatus.Active || saleEvent.Status == SaleEventStatus.Scheduled)
                select new
                {
                    listing.ListingId,
                    tier.DiscountType,
                    tier.DiscountValue,
                    tier.Label,
                    saleEvent.Name,
                    saleEvent.Status,
                    saleEvent.StartDate
                })
            .ToListAsync(cancellationToken);

        if (discountAssignments.Count == 0)
        {
            return new Dictionary<Guid, string>();
        }

        var culture = CultureInfo.GetCultureInfo("en-US");

        var chosenAssignments = discountAssignments
            .GroupBy(x => x.ListingId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .OrderByDescending(x => x.Status == SaleEventStatus.Active)
                    .ThenByDescending(x => x.StartDate)
                    .ThenByDescending(x => x.DiscountValue)
                    .First());

        var result = new Dictionary<Guid, string>(chosenAssignments.Count);

        foreach (var (listingId, assignment) in chosenAssignments)
        {
            var discountText = assignment.DiscountType switch
            {
                SaleEventDiscountType.Percent => $"{assignment.DiscountValue:0.##}% off",
                SaleEventDiscountType.Amount => $"{assignment.DiscountValue.ToString("C", culture)} off",
                _ => assignment.DiscountValue.ToString("0.##")
            };

            var metadata = new List<string>(3);

            if (assignment.Status == SaleEventStatus.Scheduled)
            {
                metadata.Add("Scheduled");
            }

            if (!string.IsNullOrWhiteSpace(assignment.Name))
            {
                metadata.Add(assignment.Name);
            }

            if (!string.IsNullOrWhiteSpace(assignment.Label))
            {
                metadata.Add(assignment.Label!);
            }

            var suffix = metadata.Count > 0
                ? $" ({string.Join(" • ", metadata)})"
                : string.Empty;

            result[listingId] = discountText + suffix;
        }

        return result;
    }

    public async Task<ProductResearchActiveListingsPage> GetProductResearchActiveListingsAsync(
        string? searchTerm,
        int page,
        int pageSize,
        Guid? categoryId,
        ListingFormat? format,
        decimal? minPrice,
        decimal? maxPrice,
        CancellationToken cancellationToken)
    {

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var offset = (page - 1) * pageSize;
        var searchPattern = string.IsNullOrWhiteSpace(searchTerm)
            ? null
            : $"%{searchTerm.Trim()}%";

        var connection = DbContext.Database.GetDbConnection();
        var shouldClose = connection.State != ConnectionState.Open;
        if (shouldClose)
        {
            await connection.OpenAsync(cancellationToken);
        }

        const string sql = """
WITH base AS (
    SELECT
        l.id,
        l.title,
        l.sku,
        l.listing_format,
        l.start_date,
        l.created_at,
        l.category_id,
        CASE
            WHEN l.listing_format = @FixedPriceFormat AND l.type = @MultiVariationType THEN COALESCE(mv.total_value / NULLIF(mv.total_quantity, 0), 0)
            WHEN l.listing_format = @AuctionFormat THEN COALESCE(l.pricing_buy_it_now_price, l.pricing_start_price, 0)
            ELSE COALESCE(l.pricing_price, 0)
        END AS price_amount,
        CASE WHEN l.listing_format = @FixedPriceFormat AND l.type = @MultiVariationType THEN TRUE ELSE FALSE END AS is_multi_variation,
        CASE WHEN l.listing_format = @FixedPriceFormat AND l.type = @MultiVariationType THEN COALESCE(mv.total_quantity, 0) ELSE COALESCE(l.pricing_quantity, 0) END AS available_quantity,
        img.url AS image_url
    FROM listing l
    LEFT JOIN (
        SELECT listing_id,
               SUM(price * quantity) AS total_value,
               SUM(quantity) AS total_quantity
        FROM variation
        GROUP BY listing_id
    ) mv ON mv.listing_id = l.id
    LEFT JOIN LATERAL (
        SELECT li.url
        FROM listing_image li
        WHERE li.listing_id = l.id
        ORDER BY li.is_primary DESC, li.id
        LIMIT 1
    ) img ON TRUE
    WHERE l.status = @ActiveStatus
      AND (@SearchTerm IS NULL OR l.title ILIKE @SearchTerm OR l.sku ILIKE @SearchTerm)
      AND (@CategoryId IS NULL OR l.category_id = @CategoryId)
      AND (@Format IS NULL OR l.listing_format = @Format)
),
filtered AS (
    SELECT *
    FROM base
    WHERE (@MinPrice IS NULL OR price_amount >= @MinPrice)
      AND (@MaxPrice IS NULL OR price_amount <= @MaxPrice)
),
metrics AS (
    SELECT
        COUNT(*) AS "TotalCount",
        AVG(price_amount) AS "AveragePrice",
        MIN(price_amount) AS "MinPrice",
        MAX(price_amount) AS "MaxPrice"
    FROM filtered
),
ordered AS (
    SELECT
        f.*,
        ROW_NUMBER() OVER (ORDER BY f.start_date DESC NULLS LAST, f.created_at DESC, f.id) AS row_number
    FROM filtered f
),
paged AS (
    SELECT
        o.id AS "ListingId",
        o.title AS "Title",
        o.sku AS "Sku",
        o.listing_format AS "Format",
        o.price_amount AS "PriceAmount",
        @DefaultCurrency AS "PriceCurrency",
        o.start_date AS "StartDate",
        o.created_at AS "CreatedAt",
        o.image_url AS "ImageUrl",
        o.is_multi_variation AS "IsMultiVariation",
        o.available_quantity AS "AvailableQuantity",
        o.category_id AS "CategoryId"
    FROM ordered o
    WHERE o.row_number > @Offset AND o.row_number <= (@Offset + @PageSize)
    ORDER BY o.row_number
)
SELECT jsonb_build_object(
    'metrics', (SELECT row_to_json(m) FROM metrics m),
    'listings', (SELECT jsonb_agg(p) FROM paged p)
) AS result;
""";

        var parameters = new
        {
            SearchTerm = searchPattern,
            CategoryId = categoryId,
            Format = format.HasValue ? (int?)format.Value : null,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            ActiveStatus = (int)ListingStatus.Active,
            FixedPriceFormat = (int)ListingFormat.FixedPrice,
            AuctionFormat = (int)ListingFormat.Auction,
            MultiVariationType = (int)ListingType.MultiVariation,
            DefaultCurrency,
            PageSize = pageSize,
            Offset = offset
        };

        using var conn = await ConnectionFactory.CreateConnectionAsync();

        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var reader = await conn.ExecuteScalarAsync<string>(command);

        if (string.IsNullOrWhiteSpace(reader))
            return new ProductResearchActiveListingsPage([], 0, null, null, null, DefaultCurrency);

        var json = JsonDocument.Parse(reader);
        var root = json.RootElement;

        var metricsElement = root.GetProperty("metrics");
        var listingsElement = root.GetProperty("listings");

        var metrics = JsonSerializer
            .Deserialize<ProductResearchActiveListingMetricsRow>(metricsElement.GetRawText());
        var listings = JsonSerializer
            .Deserialize<List<ProductResearchActiveListingRecord>>(listingsElement.GetRawText()) ?? [];

        return new ProductResearchActiveListingsPage(
            listings,
            metrics?.TotalCount ?? 0,
            metrics?.AveragePrice,
            metrics?.MinPrice,
            metrics?.MaxPrice,
            DefaultCurrency
        );
    }

    private sealed record ProductResearchActiveListingMetricsRow(int TotalCount, decimal? AveragePrice, decimal? MinPrice, decimal? MaxPrice);

    public async Task<IReadOnlyDictionary<Guid, ListingFormat>> GetListingFormatsAsync(
        IReadOnlyCollection<Guid> listingIds,
        CancellationToken cancellationToken)
    {
        if (listingIds is null || listingIds.Count == 0)
        {
            return new Dictionary<Guid, ListingFormat>();
        }

        var formats = await DbContext.Listings
            .AsNoTracking()
            .Where(listing => listingIds.Contains(listing.Id))
            .Select(listing => new { listing.Id, listing.Format })
            .ToDictionaryAsync(x => x.Id, x => x.Format, cancellationToken);

        return formats;
    }

    public async Task<(IReadOnlyList<SaleEventEligibleListingDto> Items, int TotalCount)> GetEligibleListingsForSaleEventAsync(
        string ownerId,
        string? searchTerm,
        Guid? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int? minDaysOnSite,
        bool excludeAlreadyAssigned,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);

        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 200);

        var baseQuery = ApplySearchFilter(
            FilterByOwner(DbContext.Listings.AsNoTracking().Where(l => l.Status == ListingStatus.Active), ownerId),
            searchTerm);

        if (categoryId.HasValue)
        {
            baseQuery = baseQuery.Where(l => l.CategoryId == categoryId);
        }

        if (minDaysOnSite.HasValue && minDaysOnSite.Value > 0)
        {
            var cutoff = DateTime.UtcNow.AddDays(-minDaysOnSite.Value);
            baseQuery = baseQuery.Where(l => (l.StartDate ?? l.CreatedAt) <= cutoff);
        }

        HashSet<Guid>? assignedListingSet = null;
        if (excludeAlreadyAssigned)
        {
            var sellerGuid = Guid.Parse(ownerId);
            var sellerId = new UserId(sellerGuid);

            var assignedListingIds = await (from saleEventListing in DbContext.SaleEventListings.AsNoTracking()
                                             join saleEvent in DbContext.SaleEvents.AsNoTracking()
                                                 on saleEventListing.SaleEventId equals saleEvent.Id
                                             where saleEvent.SellerId == sellerId &&
                                                   (saleEvent.Status == SaleEventStatus.Scheduled ||
                                                    saleEvent.Status == SaleEventStatus.Active)
                                             select saleEventListing.ListingId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (assignedListingIds.Count > 0)
            {
                assignedListingSet = assignedListingIds.ToHashSet();
                baseQuery = baseQuery.Where(l => !assignedListingSet.Contains(l.Id));
            }
        }

        if (minPrice.HasValue || maxPrice.HasValue)
        {
            var fixedPriceIdsQuery = DbContext.FixedPriceListings
                .AsNoTracking()
                .Where(l => l.Status == ListingStatus.Active && l.CreatedBy == ownerId);

            fixedPriceIdsQuery = ApplySearchFilter(fixedPriceIdsQuery, searchTerm);

            if (categoryId.HasValue)
            {
                fixedPriceIdsQuery = fixedPriceIdsQuery.Where(l => l.CategoryId == categoryId);
            }

            if (minDaysOnSite.HasValue && minDaysOnSite.Value > 0)
            {
                var cutoff = DateTime.UtcNow.AddDays(-minDaysOnSite.Value);
                fixedPriceIdsQuery = fixedPriceIdsQuery.Where(l => (l.StartDate ?? l.CreatedAt) <= cutoff);
            }

            if (minPrice.HasValue)
            {
                var min = minPrice.Value;
                fixedPriceIdsQuery = fixedPriceIdsQuery.Where(l =>
                    (l.Type == ListingType.Single && l.Pricing.Price >= min) ||
                    (l.Type == ListingType.MultiVariation && l.Variations.Any(v => v.Price >= min)));
            }

            if (maxPrice.HasValue)
            {
                var max = maxPrice.Value;
                fixedPriceIdsQuery = fixedPriceIdsQuery.Where(l =>
                    (l.Type == ListingType.Single && l.Pricing.Price <= max) ||
                    (l.Type == ListingType.MultiVariation && l.Variations.Any(v => v.Price <= max)));
            }

            var auctionIdsQuery = DbContext.AuctionListings
                .AsNoTracking()
                .Where(l => l.Status == ListingStatus.Active && l.CreatedBy == ownerId);

            auctionIdsQuery = ApplySearchFilter(auctionIdsQuery, searchTerm);

            if (categoryId.HasValue)
            {
                auctionIdsQuery = auctionIdsQuery.Where(l => l.CategoryId == categoryId);
            }

            if (minDaysOnSite.HasValue && minDaysOnSite.Value > 0)
            {
                var cutoff = DateTime.UtcNow.AddDays(-minDaysOnSite.Value);
                auctionIdsQuery = auctionIdsQuery.Where(l => (l.StartDate ?? l.CreatedAt) <= cutoff);
            }

            if (minPrice.HasValue)
            {
                auctionIdsQuery = auctionIdsQuery.Where(l => l.Pricing.StartPrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                auctionIdsQuery = auctionIdsQuery.Where(l => l.Pricing.StartPrice <= maxPrice.Value);
            }

            var filteredIds = await fixedPriceIdsQuery
                .Select(l => l.Id)
                .Union(auctionIdsQuery.Select(l => l.Id))
                .ToListAsync(cancellationToken);

            if (filteredIds.Count == 0)
            {
                return (Array.Empty<SaleEventEligibleListingDto>(), 0);
            }

            var filteredSet = filteredIds.ToHashSet();
            baseQuery = baseQuery.Where(l => filteredSet.Contains(l.Id));
        }

        var totalCount = await baseQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return (Array.Empty<SaleEventEligibleListingDto>(), 0);
        }

        var orderedQuery = baseQuery
            .OrderByDescending(l => l.UpdatedAt ?? l.CreatedAt)
            .ThenBy(l => l.Id);

        var listingIds = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var fixedPriceListings = await DbContext.FixedPriceListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Variations)
            .Where(l => listingIds.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id, l => l, cancellationToken);

        var auctionListings = await DbContext.AuctionListings
            .AsNoTracking()
            .Include(l => l.Images)
            .Where(l => listingIds.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id, l => l, cancellationToken);

        var results = new List<SaleEventEligibleListingDto>(listingIds.Count);
        var assignedLookup = assignedListingSet ?? new HashSet<Guid>();

        foreach (var listingId in listingIds)
        {
            if (fixedPriceListings.TryGetValue(listingId, out var fixedListing))
            {
                var basePrice = fixedListing.Type == ListingType.Single
                    ? fixedListing.Pricing.Price
                    : fixedListing.Variations.OrderBy(v => v.Price).Select(v => v.Price).FirstOrDefault();

                var thumbnail = GetPrimaryImageUrl(fixedListing);

                results.Add(new SaleEventEligibleListingDto(
                    fixedListing.Id,
                    fixedListing.Title,
                    fixedListing.Sku,
                    thumbnail,
                    fixedListing.Format,
                    fixedListing.CategoryId,
                    basePrice,
                    fixedListing.CreatedAt,
                    fixedListing.StartDate,
                    fixedListing.EndDate,
                    assignedLookup.Contains(fixedListing.Id)));

                continue;
            }

            if (auctionListings.TryGetValue(listingId, out var auctionListing))
            {
                var thumbnail = GetPrimaryImageUrl(auctionListing);

                results.Add(new SaleEventEligibleListingDto(
                    auctionListing.Id,
                    auctionListing.Title,
                    auctionListing.Sku,
                    thumbnail,
                    auctionListing.Format,
                    auctionListing.CategoryId,
                    auctionListing.Pricing.StartPrice,
                    auctionListing.CreatedAt,
                    auctionListing.StartDate,
                    auctionListing.EndDate,
                    assignedLookup.Contains(auctionListing.Id)));
            }
        }

        return (results, totalCount);
    }

    public async Task<ListingOverviewSnapshot> GetOverviewSnapshotAsync(string ownerId, DateTime todayUtc, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerId);

        var listingInfos = await FilterByOwner(DbContext.Listings.AsNoTracking(), ownerId)
            .Select(l => new
            {
                l.Status,
                l.Format,
                l.EndDate
            })
            .ToListAsync(cancellationToken);

        var statusCounts = listingInfos
            .GroupBy(l => l.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        var today = DateOnly.FromDateTime(todayUtc.Date);
        var auctionsEndingToday = listingInfos.Count(l =>
            l.Format == ListingFormat.Auction &&
            l.EndDate.HasValue &&
            DateOnly.FromDateTime(l.EndDate.Value.Date) == today);

        statusCounts.TryGetValue(ListingStatus.Ended, out var endedCount);

        return new ListingOverviewSnapshot(
            statusCounts,
            auctionsEndingToday,
            BuyItNowRenewingToday: 0,
            WithReserveMet: 0,
            WithQuestions: 0,
            WithOpenOffers: 0,
            UnsoldNotRelisted: endedCount
        );
    }

    private static IQueryable<TListing> ApplySearchFilter<TListing>(IQueryable<TListing> query, string? searchTerm)
        where TListing : Listing
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return query;
        }

        var term = $"%{searchTerm.Trim()}%";

        return query.Where(l => EF.Functions.ILike(l.Title, term) || EF.Functions.ILike(l.Sku, term));
    }

    private static string? GetPrimaryImageUrl(Listing listing)
    {
        var primary = listing.Images.FirstOrDefault(img => img.IsPrimary);
        return primary?.Url ?? listing.Images.FirstOrDefault()?.Url;
    }
}
