using PRN232_EbayClone.Domain.Shared.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Shared.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency)
    {
        if (amount < 0)
            return MoneyErrors.NegativeAmount;

        if (string.IsNullOrWhiteSpace(currency))
            return MoneyErrors.InvalidCurrency;

        var normalizedAmount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        var normalizedCurrency = currency.ToUpper();

        return new Money(normalizedAmount, normalizedCurrency);
    }

    public static Result<Money> Zero(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            return MoneyErrors.InvalidCurrency;

        return new Money(0, currency.ToUpper());
    }

    // --- Arithmetic ---
    public static Result<Money> operator +(Money a, Money b)
    {
        if (!EnsureSameCurrency(a, b))
            return MoneyErrors.DifferentCurrency;

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Result<Money> operator -(Money a, Money b)
    {
        if (!EnsureSameCurrency(a, b))
            return MoneyErrors.DifferentCurrency;

        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static Result<Money> operator *(Money a, decimal factor)
        => new Money(a.Amount * factor, a.Currency);

    public static Result<Money> operator /(Money a, decimal divisor)
    {
        if (divisor == 0)
            return MoneyErrors.DivideByZero;

        return new Money(a.Amount / divisor, a.Currency);
    }

    // --- Comparison ---
    private static bool EnsureSameCurrency(Money a, Money b)
        => a.Currency == b.Currency;

    public static bool operator >(Money a, Money b)
        => EnsureSameCurrency(a, b) && a.Amount > b.Amount;

    public static bool operator <(Money a, Money b)
        => EnsureSameCurrency(a, b) && a.Amount < b.Amount;

    public static bool operator >=(Money a, Money b)
        => EnsureSameCurrency(a, b) && a.Amount >= b.Amount;

    public static bool operator <=(Money a, Money b)
        => EnsureSameCurrency(a, b) && a.Amount <= b.Amount;

    public override string ToString()
        => $"{Amount:0.00} {Currency}";

    public static implicit operator decimal(Money money) => money.Amount;
}
