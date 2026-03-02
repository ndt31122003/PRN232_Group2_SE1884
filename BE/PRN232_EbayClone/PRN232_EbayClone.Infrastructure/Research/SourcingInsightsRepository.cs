using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using PRN232_EbayClone.Application.Research.Abstractions;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Research;

internal sealed class SourcingInsightsRepository(IDbConnectionFactory ConnectionFactory) : ISourcingInsightsRepository
{
    private const int MaxSavedCategories = 150;

    private static readonly SemaphoreSlim SchemaSemaphore = new(1, 1);
    private static bool _schemaEnsured;

    public async Task<IReadOnlyList<SourcingCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var rawConnection = await ConnectionFactory.CreateConnectionAsync();

        if (rawConnection is not DbConnection connection)
        {
            rawConnection.Dispose();
            throw new InvalidOperationException("Expected DbConnection from connection factory.");
        }

        await using (connection)
        {
            var command = new CommandDefinition(
                SqlGetCategoryMetrics,
                new
                {
                    ActiveStatus = (int)ListingStatus.Active,
                    FromDate = DateTime.UtcNow.AddDays(-60),
                    CancelledStatus = OrderStatusCodes.Cancelled,
                    DeliveryFailedStatus = OrderStatusCodes.DeliveryFailed
                },
                cancellationToken: cancellationToken);

            var rows = (await connection.QueryAsync<CategoryMetricsRow>(command)).ToList();

            if (rows.Count == 0)
            {
                return Array.Empty<SourcingCategoryDto>();
            }

            var metrics = rows.Select(row => BuildIntermediate(row)).ToList();

            var totalSoldUnits = metrics.Sum(item => item.SoldUnits);
            var maxRatio = metrics.Max(item => item.SearchToListingRatio);
            var maxSellThrough = metrics.Max(item => item.SellThroughRate);
            var maxReturnRate = metrics.Max(item => item.ReturnRate);

            var result = new List<SourcingCategoryDto>(metrics.Count);

            foreach (var item in metrics)
            {
                var marketShare = totalSoldUnits > 0
                    ? Math.Round((decimal)item.SoldUnits / totalSoldUnits * 100m, 2, MidpointRounding.AwayFromZero)
                    : 0m;

                var ratioScore = maxRatio > 0m ? item.SearchToListingRatio / maxRatio : 0m;
                var sellScore = maxSellThrough > 0m ? item.SellThroughRate / maxSellThrough : 0m;
                var returnScore = maxReturnRate > 0m ? 1m - Math.Min(item.ReturnRate / maxReturnRate, 1m) : 1m;

                var opportunityScore = ClampDecimal(
                    0.45m * ratioScore +
                    0.45m * sellScore +
                    0.10m * returnScore,
                    0m,
                    1m);

                var roundedOpportunity = Math.Round(opportunityScore, 2, MidpointRounding.AwayFromZero);

                result.Add(new SourcingCategoryDto(
                    item.Id.ToString(),
                    item.Name,
                    item.Group,
                    GetOpportunityTag(roundedOpportunity),
                    roundedOpportunity,
                    item.SearchVolume,
                    item.ActiveListings,
                    Math.Round(item.SearchToListingRatio, 2, MidpointRounding.AwayFromZero),
                    Math.Round(item.SellThroughRate, 2, MidpointRounding.AwayFromZero),
                    item.AverageDaysToFirstSale,
                    Math.Round(item.ReturnRate, 2, MidpointRounding.AwayFromZero),
                    marketShare));
            }

            return result;
        }
    }

    public async Task<IReadOnlyCollection<string>> GetSavedCategoryIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var rawConnection = await ConnectionFactory.CreateConnectionAsync();

        if (rawConnection is not DbConnection connection)
        {
            rawConnection.Dispose();
            throw new InvalidOperationException("Expected DbConnection from connection factory.");
        }

        await using (connection)
        {
            await EnsureSchemaAsync(connection, cancellationToken);

            var command = new CommandDefinition(
                """
                SELECT category_id
                FROM research_saved_category
                WHERE user_id = @UserId
                ORDER BY created_at DESC;
                """,
                new { UserId = userId },
                cancellationToken: cancellationToken);

            var rows = await connection.QueryAsync<string>(command);
            return rows.ToList();
        }
    }

    public async Task SaveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            return;
        }

        var rawConnection = await ConnectionFactory.CreateConnectionAsync();

        if (rawConnection is not DbConnection connection)
        {
            rawConnection.Dispose();
            throw new InvalidOperationException("Expected DbConnection from connection factory.");
        }

        await using (connection)
        {
            await EnsureSchemaAsync(connection, cancellationToken);

            await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

            var existingCount = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
                "SELECT COUNT(*) FROM research_saved_category WHERE user_id = @UserId;",
                new { UserId = userId },
                transaction,
                cancellationToken: cancellationToken));

            if (existingCount >= MaxSavedCategories)
            {
                await transaction.RollbackAsync(cancellationToken);
                return;
            }

            await connection.ExecuteAsync(new CommandDefinition(
                """
                INSERT INTO research_saved_category (user_id, category_id, created_at)
                VALUES (@UserId, @CategoryId, NOW())
                ON CONFLICT (user_id, category_id) DO NOTHING;
                """,
                new
                {
                    UserId = userId,
                    CategoryId = categoryId.Trim()
                },
                transaction,
                cancellationToken: cancellationToken));

            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task RemoveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(categoryId))
        {
            return;
        }

        var rawConnection = await ConnectionFactory.CreateConnectionAsync();

        if (rawConnection is not DbConnection connection)
        {
            rawConnection.Dispose();
            throw new InvalidOperationException("Expected DbConnection from connection factory.");
        }

        await using (connection)
        {
            await EnsureSchemaAsync(connection, cancellationToken);

            await connection.ExecuteAsync(new CommandDefinition(
                """
                DELETE FROM research_saved_category
                WHERE user_id = @UserId AND category_id = @CategoryId;
                """,
                new
                {
                    UserId = userId,
                    CategoryId = categoryId.Trim()
                },
                cancellationToken: cancellationToken));
        }
    }

    private static IntermediateMetrics BuildIntermediate(CategoryMetricsRow row)
    {
        var group = string.IsNullOrWhiteSpace(row.ParentName)
            ? "across Marketplace"
            : $"in {row.ParentName}";

        var searchVolume = ComputeSearchVolume(row);

        var searchRatio = row.ActiveListings > 0
            ? (decimal)searchVolume / row.ActiveListings
            : 0m;

        var sellThrough = row.ActiveListings > 0
            ? (decimal)row.SoldUnits / row.ActiveListings
            : 0m;

        var returnRate = row.OrderCount > 0
            ? (decimal)row.ReturnCount / row.OrderCount
            : 0m;

        var averageDays = row.AvgDaysToFirstSale.HasValue
            ? (int)Math.Max(0, Math.Round(row.AvgDaysToFirstSale.Value, MidpointRounding.AwayFromZero))
            : (row.SoldUnits > 0 ? 7 : 0);

        return new IntermediateMetrics(
            row.CategoryId,
            row.CategoryName,
            group,
            Math.Max(row.ActiveListings, 0),
            Math.Max(row.SoldUnits, 0),
            searchVolume,
            searchRatio,
            sellThrough,
            returnRate,
            averageDays);
    }

    private static long ComputeSearchVolume(CategoryMetricsRow row)
    {
        var baseVolume =
            row.SoldUnits * 120L +
            row.OrderCount * 80L +
            row.ActiveListings * 20L;

        var salesBoost = (long)Math.Round((double)row.GrossSales * 2d, MidpointRounding.AwayFromZero);

        var total = baseVolume + salesBoost;
        return total < 0 ? 0 : total;
    }

    private static string GetOpportunityTag(decimal score)
    {
        if (score >= 0.80m)
        {
            return "Good opportunity";
        }

        if (score >= 0.60m)
        {
            return "Emerging";
        }

        if (score >= 0.40m)
        {
            return "Competitive";
        }

        return "Watchlist";
    }

    private static decimal ClampDecimal(decimal value, decimal min, decimal max)
    {
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }

        return value;
    }

    private static async Task EnsureSchemaAsync(IDbConnection connection, CancellationToken cancellationToken)
    {
        if (_schemaEnsured)
        {
            return;
        }

        await SchemaSemaphore.WaitAsync(cancellationToken);

        try
        {
            if (_schemaEnsured)
            {
                return;
            }

            var command = new CommandDefinition(
                """
                CREATE TABLE IF NOT EXISTS research_saved_category (
                    user_id uuid NOT NULL,
                    category_id text NOT NULL,
                    created_at timestamp with time zone NOT NULL DEFAULT NOW(),
                    CONSTRAINT pk_research_saved_category PRIMARY KEY (user_id, category_id)
                );
                """,
                cancellationToken: cancellationToken);

            await connection.ExecuteAsync(command);

            _schemaEnsured = true;
        }
        finally
        {
            SchemaSemaphore.Release();
        }
    }

    private const string SqlGetCategoryMetrics = """
WITH active_listings AS (
    SELECT
        category_id,
        COUNT(*)::bigint AS ActiveListings
    FROM listing
    WHERE status = @ActiveStatus
    GROUP BY category_id
),
recent_orders AS (
    SELECT
        l.category_id,
        oi.listing_id,
        SUM(oi.quantity)::bigint AS SoldUnits,
        COUNT(DISTINCT o.id)::bigint AS OrderCount,
        SUM(CASE WHEN os.code IN (@CancelledStatus, @DeliveryFailedStatus) THEN 1 ELSE 0 END)::bigint AS ReturnCount,
        COALESCE(SUM(oi.total_price_amount), 0)::numeric AS GrossSales,
        MIN(o.ordered_at) AS FirstOrderAt,
        MIN(l.start_date) AS ListingStart
    FROM order_items oi
    JOIN orders o ON o.id = oi.order_id
    JOIN order_statuses os ON os.id = o.status_id
    JOIN listing l ON l.id = oi.listing_id
    WHERE o.ordered_at >= @FromDate
    GROUP BY l.category_id, oi.listing_id
),
aggregated AS (
    SELECT
        category_id,
        SUM(SoldUnits)::bigint AS SoldUnits,
        SUM(OrderCount)::bigint AS OrderCount,
        SUM(ReturnCount)::bigint AS ReturnCount,
        COALESCE(SUM(GrossSales), 0)::numeric AS GrossSales,
        AVG(EXTRACT(EPOCH FROM (FirstOrderAt - ListingStart)) / 86400.0)
            FILTER (WHERE FirstOrderAt IS NOT NULL AND ListingStart IS NOT NULL AND FirstOrderAt >= ListingStart) AS AvgDaysToFirstSale
    FROM recent_orders
    GROUP BY category_id
)
SELECT
    c.id AS CategoryId,
    c.name AS CategoryName,
    parent.name AS ParentName,
    COALESCE(al.ActiveListings, 0)::bigint AS ActiveListings,
    COALESCE(agg.SoldUnits, 0)::bigint AS SoldUnits,
    COALESCE(agg.OrderCount, 0)::bigint AS OrderCount,
    COALESCE(agg.ReturnCount, 0)::bigint AS ReturnCount,
    COALESCE(agg.GrossSales, 0)::numeric AS GrossSales,
    agg.AvgDaysToFirstSale
FROM category c
LEFT JOIN category parent ON parent.id = c.parent_id
LEFT JOIN active_listings al ON al.category_id = c.id
LEFT JOIN aggregated agg ON agg.category_id = c.id
WHERE NOT EXISTS (
    SELECT 1 FROM category child WHERE child.parent_id = c.id
)
ORDER BY c.name;
""";

    private readonly record struct CategoryMetricsRow(
        Guid CategoryId,
        string CategoryName,
        string? ParentName,
        long ActiveListings,
        long SoldUnits,
        long OrderCount,
        long ReturnCount,
        decimal GrossSales,
        double? AvgDaysToFirstSale);

    private readonly record struct IntermediateMetrics(
        Guid Id,
        string Name,
        string Group,
        long ActiveListings,
        long SoldUnits,
        long SearchVolume,
        decimal SearchToListingRatio,
        decimal SellThroughRate,
        decimal ReturnRate,
        int AverageDaysToFirstSale);
}
