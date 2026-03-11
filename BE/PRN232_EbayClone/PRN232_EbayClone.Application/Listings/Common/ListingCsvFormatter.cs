using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using PRN232_EbayClone.Application.Listings.Commands;
using PRN232_EbayClone.Application.Listings.Queries;

namespace PRN232_EbayClone.Application.Listings.Common;

public static class ListingCsvFormatter
{
    public static string BuildExportCsv(IEnumerable<ListingExportRow> rows)
    {
        var builder = new StringBuilder();
        builder.AppendLine("ListingId,Status,Format,Title,Sku,Quantity,Price,StartPrice,BuyItNowPrice,ReservePrice,ScheduledStartTime,IsMultiVariation,VariationSummary");

        foreach (var row in rows)
        {
            var fields = new[]
            {
                CsvEscape(row.ListingId),
                CsvEscape(row.Status.ToString()),
                CsvEscape(row.Format.ToString()),
                CsvEscape(row.Title),
                CsvEscape(row.Sku),
                CsvEscape(row.Quantity),
                CsvEscape(row.Price),
                CsvEscape(row.StartPrice),
                CsvEscape(row.BuyItNowPrice),
                CsvEscape(row.ReservePrice),
                CsvEscape(row.ScheduledStartTime?.ToString("o", CultureInfo.InvariantCulture)),
                CsvEscape(row.IsMultiVariation),
                CsvEscape(row.VariationSummary)
            };

            builder.AppendLine(string.Join(",", fields));
        }

        return builder.ToString();
    }

    public static (List<ListingImportRow> Rows, List<ImportFailure> Failures) ParseImportCsv(string content)
    {
        var rows = new List<ListingImportRow>();
        var failures = new List<ImportFailure>();

        if (string.IsNullOrWhiteSpace(content))
        {
            return (rows, failures);
        }

        var lines = content
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        if (lines.Count == 0)
        {
            return (rows, failures);
        }

        var headerColumns = SplitCsvLine(lines[0]);
        var headerIndex = headerColumns
            .Select((header, index) => new { header, index })
            .ToDictionary(x => x.header.Trim().ToLowerInvariant(), x => x.index);

        if (!headerIndex.TryGetValue("listingid", out var listingIdIndex))
        {
            failures.Add(new ImportFailure(1, null, "Thiếu cột ListingId."));
            return (rows, failures);
        }

        headerIndex.TryGetValue("price", out var priceIndex);
        headerIndex.TryGetValue("quantity", out var quantityIndex);
        headerIndex.TryGetValue("startprice", out var startPriceIndex);
        headerIndex.TryGetValue("buyitnowprice", out var buyItNowPriceIndex);
        headerIndex.TryGetValue("reserveprice", out var reservePriceIndex);

        for (var i = 1; i < lines.Count; i++)
        {
            var lineNumber = i + 1;
            var columns = SplitCsvLine(lines[i]);
            var listingIdText = GetColumn(columns, listingIdIndex);

            if (!Guid.TryParse(listingIdText, out var listingId))
            {
                failures.Add(new ImportFailure(lineNumber, null, "ListingId không hợp lệ."));
                continue;
            }

            var (price, priceError) = ParseNullableDecimal(columns, priceIndex);
            if (priceError is not null)
            {
                failures.Add(new ImportFailure(lineNumber, listingId, priceError));
                continue;
            }

            var (startPrice, startPriceError) = ParseNullableDecimal(columns, startPriceIndex);
            if (startPriceError is not null)
            {
                failures.Add(new ImportFailure(lineNumber, listingId, startPriceError));
                continue;
            }

            var (buyItNowPrice, buyItNowError) = ParseNullableDecimal(columns, buyItNowPriceIndex);
            if (buyItNowError is not null)
            {
                failures.Add(new ImportFailure(lineNumber, listingId, buyItNowError));
                continue;
            }

            var (reservePrice, reserveError) = ParseNullableDecimal(columns, reservePriceIndex);
            if (reserveError is not null)
            {
                failures.Add(new ImportFailure(lineNumber, listingId, reserveError));
                continue;
            }

            var (quantity, quantityError) = ParseNullableInt(columns, quantityIndex);
            if (quantityError is not null)
            {
                failures.Add(new ImportFailure(lineNumber, listingId, quantityError));
                continue;
            }

            rows.Add(new ListingImportRow(
                lineNumber,
                listingId,
                price,
                quantity,
                startPrice,
                buyItNowPrice,
                reservePrice));
        }

        return (rows, failures);
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
            bool boolValue => boolValue ? "true" : "false",
            _ => value.ToString() ?? string.Empty
        };

        if (text.Contains('"') || text.Contains(',') || text.Contains('\n') || text.Contains('\r'))
        {
            text = text.Replace("\"", "\"\"", StringComparison.Ordinal);
            return $"\"{text}\"";
        }

        return text;
    }

    private static string[] SplitCsvLine(string line)
    {
        var result = new List<string>();
        var builder = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (c == '\"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
                {
                    builder.Append('\"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(builder.ToString());
                builder.Clear();
            }
            else
            {
                builder.Append(c);
            }
        }

        result.Add(builder.ToString());
        return result.Select(value => value.Trim()).ToArray();
    }

    private static string GetColumn(string[] columns, int index)
    {
        if (index < 0 || index >= columns.Length)
        {
            return string.Empty;
        }

        return columns[index].Trim();
    }

    private static (decimal?, string?) ParseNullableDecimal(string[] columns, int? index)
    {
        if (index is null || index < 0)
        {
            return (null, null);
        }

        var raw = GetColumn(columns, index.Value);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return (null, null);
        }

        if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
        {
            return (value, null);
        }

        return (null, $"Giá trị '{raw}' không hợp lệ.");
    }

    private static (int?, string?) ParseNullableInt(string[] columns, int? index)
    {
        if (index is null || index < 0)
        {
            return (null, null);
        }

        var raw = GetColumn(columns, index.Value);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return (null, null);
        }

        if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
        {
            return (value, null);
        }

        return (null, $"Giá trị '{raw}' không hợp lệ.");
    }
}
