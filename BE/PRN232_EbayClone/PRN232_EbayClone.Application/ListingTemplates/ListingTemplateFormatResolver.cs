using System.Globalization;
using System.Text.Json;

namespace PRN232_EbayClone.Application.ListingTemplates;

internal static class ListingTemplateFormatResolver
{
    private const string AuctionLabel = "Auction";
    private const string BuyItNowLabel = "Buy It Now";

    public static string Resolve(string payloadJson)
    {
        if (string.IsNullOrWhiteSpace(payloadJson))
        {
            return AuctionLabel;
        }

        try
        {
            using var document = JsonDocument.Parse(payloadJson);
            var formatCode = FindFormatCode(document.RootElement);

            return MapLabel(formatCode);
        }
        catch (JsonException)
        {
            return AuctionLabel;
        }
    }

    private static string MapLabel(string? formatCode)
    {
        if (string.IsNullOrWhiteSpace(formatCode))
        {
            return AuctionLabel;
        }

        var normalized = formatCode.Trim().ToLowerInvariant();

        return normalized switch
        {
            "2" or "buyitnow" or "buy_it_now" or "fixedprice" or "fixed_price" => BuyItNowLabel,
            _ => AuctionLabel
        };
    }

    private static string? FindFormatCode(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        if (TryReadString(element, "format", out var formatCode))
        {
            return formatCode;
        }

        if (TryReadString(element, "priceFormat", out formatCode))
        {
            return formatCode;
        }

        if (TryReadString(element, "type", out formatCode))
        {
            return formatCode;
        }

        foreach (var propertyName in new[] { "listingRequest", "payload", "formState" })
        {
            if (!element.TryGetProperty(propertyName, out var child) || child.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            var nested = FindFormatCode(child);
            if (!string.IsNullOrWhiteSpace(nested))
            {
                return nested;
            }
        }

        return null;
    }

    private static bool TryReadString(JsonElement element, string propertyName, out string? value)
    {
        value = null;

        if (!element.TryGetProperty(propertyName, out var property))
        {
            return false;
        }

        switch (property.ValueKind)
        {
            case JsonValueKind.String:
                value = property.GetString();
                return true;
            case JsonValueKind.Number:
                if (property.TryGetInt32(out var intValue))
                {
                    value = intValue.ToString(CultureInfo.InvariantCulture);
                    return true;
                }

                value = property.GetDouble().ToString(CultureInfo.InvariantCulture);
                return true;
            default:
                return false;
        }
    }
}
