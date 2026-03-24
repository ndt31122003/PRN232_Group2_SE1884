using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Represents a tier in a multi-tier order discount structure
/// </summary>
public sealed class OrderDiscountTier : Entity<Guid>
{
    public Guid OrderDiscountId { get; private set; }
    public decimal ThresholdValue { get; private set; }
    public decimal DiscountValue { get; private set; }
    public int TierOrder { get; private set; }

    private OrderDiscountTier(Guid id) : base(id) { }

    public static Result<OrderDiscountTier> Create(
        Guid orderDiscountId,
        decimal thresholdValue,
        decimal discountValue,
        int tierOrder)
    {
        if (thresholdValue <= 0)
            return Error.Validation("OrderDiscountTier.InvalidThreshold", "Threshold value must be greater than 0");

        if (discountValue <= 0)
            return Error.Validation("OrderDiscountTier.InvalidDiscountValue", "Discount value must be greater than 0");

        if (tierOrder < 0)
            return Error.Validation("OrderDiscountTier.InvalidTierOrder", "Tier order must be non-negative");

        var tier = new OrderDiscountTier(Guid.NewGuid())
        {
            OrderDiscountId = orderDiscountId,
            ThresholdValue = thresholdValue,
            DiscountValue = discountValue,
            TierOrder = tierOrder
        };

        return Result.Success(tier);
    }
}
