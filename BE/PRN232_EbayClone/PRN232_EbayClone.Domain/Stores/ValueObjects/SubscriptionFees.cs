using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Stores.ValueObjects;

public sealed record SubscriptionFees(
    Money MonthlyFee,
    decimal FinalValueFeePercentage,
    int ListingLimit)
{
    public static SubscriptionFees ForBasic()
        => CreateBasic();

    public static SubscriptionFees ForPremium()
        => CreatePremium();

    public static SubscriptionFees ForAnchor()
        => CreateAnchor();

    private static SubscriptionFees CreateBasic()
        => new(
            Money.Create(0m, "USD").Value,
            12.9m,
            250);

    private static SubscriptionFees CreatePremium()
        => new(
            Money.Create(21.95m, "USD").Value,
            10.9m,
            1000);

    private static SubscriptionFees CreateAnchor()
        => new(
            Money.Create(299.95m, "USD").Value,
            9.9m,
            int.MaxValue);
}

