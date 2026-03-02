namespace PRN232_EbayClone.Domain.Users.ValueObjects;

public sealed record SellingLimitPolicy(int MaxActiveListings, decimal MaxTotalValue)
{
    public static SellingLimitPolicy For(SellerPerformanceLevel level) =>
        level.Name switch
        {
            nameof(SellerPerformanceLevel.BelowStandard) => new(10, 500m),
            nameof(SellerPerformanceLevel.AboveStandard) => new(100, 5000m),
            nameof(SellerPerformanceLevel.TopRated) => new(1000, 50000m),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, "Unsupported seller level")
        };

    public bool CanList(int currentActiveCount, decimal currentTotalValue) =>
        currentActiveCount < MaxActiveListings && currentTotalValue < MaxTotalValue;
}
