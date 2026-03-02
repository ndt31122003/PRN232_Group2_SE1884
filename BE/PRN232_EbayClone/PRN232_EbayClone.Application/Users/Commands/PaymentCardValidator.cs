using System;
using System.Globalization;
using System.Linq;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.Errors;

namespace PRN232_EbayClone.Application.Users.Commands;

internal static class PaymentCardValidator
{
    public static Result<PaymentCardSnapshot> Inspect(
        string holderName,
        string cardNumber,
        string expiryMonth,
        string expiryYear,
        string cvv)
    {
        if (string.IsNullOrWhiteSpace(holderName))
        {
            return UserErrors.InvalidPaymentCard;
        }

        var sanitizedNumber = new string(cardNumber.Where(char.IsDigit).ToArray());
        if (sanitizedNumber.Length is < 12 or > 19)
        {
            return UserErrors.InvalidPaymentCard;
        }

        if (!PassesLuhnCheck(sanitizedNumber))
        {
            return UserErrors.InvalidPaymentCard;
        }

        if (!TryParseExpiry(expiryMonth, expiryYear, out var month, out var year))
        {
            return UserErrors.InvalidPaymentCard;
        }

        if (!TryValidateCvv(cvv, sanitizedNumber))
        {
            return UserErrors.InvalidPaymentCard;
        }

        var brand = DetectBrand(sanitizedNumber);
        var last4 = sanitizedNumber[^4..];
        var masked = MaskCardNumber(sanitizedNumber);

        return Result.Success(new PaymentCardSnapshot(
            holderName.Trim(),
            brand,
            last4,
            masked,
            month,
            year));
    }

    private static bool PassesLuhnCheck(string cardNumber)
    {
        var sum = 0;
        var shouldDouble = false;

        for (var i = cardNumber.Length - 1; i >= 0; i--)
        {
            var digit = cardNumber[i] - '0';
            if (shouldDouble)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            shouldDouble = !shouldDouble;
        }

        return sum % 10 == 0;
    }

    private static bool TryParseExpiry(string monthInput, string yearInput, out int month, out int year)
    {
        month = 0;
        year = 0;

        var monthSanitized = new string((monthInput ?? string.Empty).Where(char.IsDigit).ToArray());
        if (!int.TryParse(monthSanitized, NumberStyles.None, CultureInfo.InvariantCulture, out month))
        {
            return false;
        }

        if (month is < 1 or > 12)
        {
            return false;
        }

        var yearSanitized = new string((yearInput ?? string.Empty).Where(char.IsDigit).ToArray());
        if (!int.TryParse(yearSanitized, NumberStyles.None, CultureInfo.InvariantCulture, out year))
        {
            return false;
        }

        if (yearSanitized.Length == 2)
        {
            year += 2000;
        }

        var now = DateTime.UtcNow;
        var currentYear = now.Year;
        var currentMonth = now.Month;

        if (year < currentYear)
        {
            return false;
        }

        if (year == currentYear && month < currentMonth)
        {
            return false;
        }

        if (year > currentYear + 20)
        {
            return false;
        }

        return true;
    }

    private static bool TryValidateCvv(string cvv, string cardNumber)
    {
        var digits = new string((cvv ?? string.Empty).Where(char.IsDigit).ToArray());
        if (digits.Length is < 3 or > 4)
        {
            return false;
        }

        var brand = DetectBrand(cardNumber);
        if (brand == "American Express")
        {
            return digits.Length == 4;
        }

        return digits.Length == 3;
    }

    private static string DetectBrand(string cardNumber)
    {
        if (cardNumber.StartsWith("4"))
        {
            return "Visa";
        }

        if (cardNumber.StartsWith("34") || cardNumber.StartsWith("37"))
        {
            return "American Express";
        }

        var firstTwo = int.Parse(cardNumber.Substring(0, 2), CultureInfo.InvariantCulture);
        if (firstTwo is >= 51 and <= 55)
        {
            return "Mastercard";
        }

        var firstFour = int.Parse(cardNumber.Substring(0, 4), CultureInfo.InvariantCulture);
        if (firstFour is >= 2221 and <= 2720)
        {
            return "Mastercard";
        }

        if (cardNumber.StartsWith("6011") || cardNumber.StartsWith("65"))
        {
            return "Discover";
        }

        return "Unknown";
    }

    private static string MaskCardNumber(string cardNumber)
    {
        var characters = cardNumber
            .Select((digit, index) => index < cardNumber.Length - 4 ? '*' : digit)
            .ToArray();

        var masked = new string(characters);
        return Group(masked);
    }

    private static string Group(string digits)
    {
        var groups = digits
            .Select((digit, index) => new { digit, index })
            .GroupBy(item => item.index / 4)
            .Select(group => new string(group.Select(item => item.digit).ToArray()));

        return string.Join(" ", groups);
    }
}

internal sealed record PaymentCardSnapshot(
    string CardholderName,
    string CardBrand,
    string CardLast4,
    string MaskedCardNumber,
    int ExpiryMonth,
    int ExpiryYear);
