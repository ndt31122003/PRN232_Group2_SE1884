namespace PRN232_EbayClone.Domain.Users.ValueObjects;

public sealed record SellingLimitPolicy(int MaxActiveListings, decimal MaxTotalValue)
{
    public static SellingLimitPolicy For(SellerPerformanceLevel level) =>
        level.Name switch
        {
            nameof(SellerPerformanceLevel.BelowStandard) => new(100, 500000000m),
            nameof(SellerPerformanceLevel.AboveStandard) => new(1000, 5000000000m),
            nameof(SellerPerformanceLevel.TopRated) => new(10000, 50000000000m),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, "Unsupported seller level")
        };

    public bool CanList(int currentActiveCount, decimal currentTotalValue) =>
        currentActiveCount < MaxActiveListings && currentTotalValue < MaxTotalValue;
}
