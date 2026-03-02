namespace PRN232_EbayClone.Domain.Listings.ValueObjects;

public sealed record ListingId(Guid Value)
{
    public static ListingId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}