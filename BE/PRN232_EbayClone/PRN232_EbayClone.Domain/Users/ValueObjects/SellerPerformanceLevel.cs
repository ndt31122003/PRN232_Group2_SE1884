namespace PRN232_EbayClone.Domain.Users.ValueObjects;

public sealed record SellerPerformanceLevel(string Name)
{
    public static readonly SellerPerformanceLevel BelowStandard = new(nameof(BelowStandard));
    public static readonly SellerPerformanceLevel AboveStandard = new(nameof(AboveStandard));
    public static readonly SellerPerformanceLevel TopRated = new(nameof(TopRated));

    public static SellerPerformanceLevel From(string name) => name switch
    {
        nameof(BelowStandard) => BelowStandard,
        nameof(AboveStandard) => AboveStandard,
        nameof(TopRated) => TopRated,
        _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Invalid seller performance level")
    };

    public override string ToString() => Name;
}

