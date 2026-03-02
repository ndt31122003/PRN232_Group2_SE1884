using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Listings.Common;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Application.Reports.Downloads.Abstractions;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Reports.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Infrastructure.Reports;

public sealed class ReportDownloadGenerator : IReportDownloadGenerator
{
    private const int DefaultMaxRows = 5000;

    private static readonly IReadOnlyDictionary<string, ListingStatus[]> ListingStatusesByType =
        new Dictionary<string, ListingStatus[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["all-active"] = new[] { ListingStatus.Active },
            ["all-active-listings"] = new[] { ListingStatus.Active },
            ["unsold"] = new[] { ListingStatus.Ended },
            ["unsold-listings"] = new[] { ListingStatus.Ended },
            ["scheduled"] = new[] { ListingStatus.Scheduled },
            ["all-scheduled-listings"] = new[] { ListingStatus.Scheduled }
        };

    private static readonly IReadOnlyDictionary<string, string[]> OrderStatusesByType =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["all-orders"] = Array.Empty<string>(),
            ["awaiting-payment"] = new[] { OrderStatusCodes.AwaitingPayment },
            ["awaiting-shipment"] = new[] { OrderStatusCodes.AwaitingShipment },
            ["awaiting-shipment-overdue"] = new[] { OrderStatusCodes.AwaitingShipmentOverdue },
            ["awaiting-shipment-24h"] = new[] { OrderStatusCodes.AwaitingShipmentShipWithin24h },
            ["awaiting-shipment-ship-within-24-hours"] = new[] { OrderStatusCodes.AwaitingShipmentShipWithin24h },
            ["awaiting-expedited"] = new[] { OrderStatusCodes.AwaitingExpeditedShipment },
            ["awaiting-expedited-shipment"] = new[] { OrderStatusCodes.AwaitingExpeditedShipment },
            ["paid-shipped"] = new[] { OrderStatusCodes.PaidAndShipped },
            ["paid-and-shipped"] = new[] { OrderStatusCodes.PaidAndShipped },
            ["paid-awaiting-feedback"] = new[] { OrderStatusCodes.PaidAwaitingFeedback },
            ["paid-awaiting-your-feedback"] = new[] { OrderStatusCodes.PaidAwaitingFeedback },
            ["shipped-awaiting-feedback"] = new[] { OrderStatusCodes.ShippedAwaitingFeedback },
            ["shipped-awaiting-your-feedback"] = new[] { OrderStatusCodes.ShippedAwaitingFeedback }
        };

    private readonly IListingRepository _listingRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<ReportDownloadGenerator> _logger;

    public ReportDownloadGenerator(
        IListingRepository listingRepository,
        IOrderRepository orderRepository,
        ILogger<ReportDownloadGenerator> logger)
    {
        _listingRepository = listingRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<GeneratedReportFile>> GenerateAsync(
        Guid userId,
        string source,
        string type,
        ReportDateRange? dateRange,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return Error.Failure("ReportDownload.MissingSource", "Source is required to generate the report.");
        }

        if (string.IsNullOrWhiteSpace(type))
        {
            return Error.Failure("ReportDownload.MissingType", "Type is required to generate the report.");
        }

        var normalizedSource = Normalize(source);

        try
        {
            return normalizedSource switch
            {
                "listings" => await GenerateListingsAsync(userId, type, cancellationToken),
                "orders" => await GenerateOrdersAsync(userId, type, dateRange, cancellationToken),
                _ => Error.Failure("ReportDownload.UnsupportedSource", $"Unsupported report source '{source}'.")
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to generate report download for user {UserId}, source {Source}, type {Type}",
                userId,
                source,
                type);

            return Error.Failure("ReportDownload.GenerationFailed", "Unable to generate the report at this time.");
        }
    }

    private async Task<Result<GeneratedReportFile>> GenerateListingsAsync(
        Guid userId,
        string type,
        CancellationToken cancellationToken)
    {
        var typeSlug = ToSlug(type);
        if (!ListingStatusesByType.TryGetValue(typeSlug, out var statuses))
        {
            statuses = new[] { ListingStatus.Active };
        }

        var ownerId = userId.ToString();
        var rows = (await _listingRepository.GetListingsForExportAsync(
            ownerId,
            listingIds: null,
            statuses,
            searchTerm: null,
            DefaultMaxRows,
            cancellationToken)).ToList();

        if (typeSlug.Equals("unsold", StringComparison.OrdinalIgnoreCase) || typeSlug.Equals("unsold-listings", StringComparison.OrdinalIgnoreCase))
        {
            rows = rows
                .Where(row => row.Status == ListingStatus.Ended)
                .ToList();
        }

        var csvContent = ListingCsvFormatter.BuildExportCsv(rows);
        var fileName = BuildFileName("listings", typeSlug);
        var bytes = WriteCsvToBytes(csvContent);

        return Result.Success(new GeneratedReportFile(fileName, "text/csv", bytes));
    }

    private async Task<Result<GeneratedReportFile>> GenerateOrdersAsync(
        Guid userId,
        string type,
        ReportDateRange? dateRange,
        CancellationToken cancellationToken)
    {
        var typeSlug = ToSlug(type);
        OrderStatusesByType.TryGetValue(typeSlug, out var statusCodes);

        var (fromUtc, toUtc) = ResolveDateRange(dateRange);

        var orders = (await _orderRepository.GetOrdersForSellerAsync(userId, fromUtc, toUtc, cancellationToken)).ToList();

        if (statusCodes is { Length: > 0 })
        {
            orders = orders
                .Where(order => order.Status != null
                    && statusCodes.Contains(order.Status.Code, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        if (orders.Count > DefaultMaxRows)
        {
            orders = orders
                .OrderByDescending(order => order.OrderedAt)
                .Take(DefaultMaxRows)
                .ToList();
        }

        var csvContent = BuildOrdersCsv(orders);
        var fileName = BuildFileName("orders", typeSlug);
        var bytes = WriteCsvToBytes(csvContent);

        return Result.Success(new GeneratedReportFile(fileName, "text/csv", bytes));
    }

    private static (DateTime FromUtc, DateTime ToUtc) ResolveDateRange(ReportDateRange? dateRange)
    {
        var defaultEnd = DateTime.UtcNow;
        var defaultStart = defaultEnd.AddDays(-90);

        if (dateRange is null)
        {
            return (defaultStart, defaultEnd);
        }

        var startUtc = dateRange.StartUtc ?? defaultStart;
        var endUtc = dateRange.EndUtc ?? defaultEnd;

        if (endUtc < startUtc)
        {
            (startUtc, endUtc) = (endUtc, startUtc);
        }

        return (startUtc, endUtc);
    }

    private static string BuildOrdersCsv(IEnumerable<Order> orders)
    {
        var builder = new StringBuilder();
        builder.AppendLine("OrderNumber,Status,Buyer,Email,Total,OrderedAt,PaidAt,ShippedAt,ItemCount,Items");

        foreach (var order in orders)
        {
            var buyerName = order.Buyer?.FullName ?? order.Buyer?.Username ?? string.Empty;
            var buyerEmail = order.Buyer?.Email is null ? string.Empty : order.Buyer.Email.Value;
            var totalAmount = (order.Total?.Amount ?? 0m).ToString("F2", CultureInfo.InvariantCulture);
            var orderedAt = order.OrderedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var paidAt = order.PaidAt?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? string.Empty;
            var shippedAt = order.ShippedAt?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? string.Empty;
            var itemCount = order.Items?.Sum(item => item.Quantity) ?? 0;
            var itemsSummary = order.Items is null
                ? string.Empty
                : string.Join(" | ", order.Items.Select(item => $"{item.Title} (x{item.Quantity})"));

            var fields = new[]
            {
                CsvEscape(order.OrderNumber),
                CsvEscape(order.Status?.Name ?? string.Empty),
                CsvEscape(buyerName),
                CsvEscape(buyerEmail),
                CsvEscape(totalAmount),
                CsvEscape(orderedAt),
                CsvEscape(paidAt),
                CsvEscape(shippedAt),
                CsvEscape(itemCount),
                CsvEscape(itemsSummary)
            };

            builder.AppendLine(string.Join(',', fields));
        }

        return builder.ToString();
    }

    private static byte[] WriteCsvToBytes(string content)
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true), leaveOpen: true))
        {
            writer.Write(content);
            writer.Flush();
        }

        return stream.ToArray();
    }

    private static string CsvEscape(object? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var text = value switch
        {
            decimal decimalValue => decimalValue.ToString(CultureInfo.InvariantCulture),
            double doubleValue => doubleValue.ToString(CultureInfo.InvariantCulture),
            float floatValue => floatValue.ToString(CultureInfo.InvariantCulture),
            int intValue => intValue.ToString(CultureInfo.InvariantCulture),
            long longValue => longValue.ToString(CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty
        };

        if (text.Contains('"') || text.Contains(',') || text.Contains('\n') || text.Contains('\r'))
        {
            text = text.Replace("\"", "\"\"", StringComparison.Ordinal);
            return $"\"{text}\"";
        }

        return text;
    }

    private static string BuildFileName(string source, string typeSlug)
    {
        var safeSource = ToSlug(source);
        var safeType = string.IsNullOrWhiteSpace(typeSlug) ? "all" : typeSlug;
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
        return $"{safeSource}-{safeType}-{timestamp}.csv";
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }

    private static string ToSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var normalized = value.Trim()
            .Replace('—', '-')
            .Replace('–', '-')
            .ToLowerInvariant();

        var builder = new StringBuilder(normalized.Length);
        var previousDash = false;

        foreach (var character in normalized)
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
                previousDash = false;
            }
            else if (!previousDash)
            {
                builder.Append('-');
                previousDash = true;
            }
        }

        return builder.ToString().Trim('-');
    }
}
