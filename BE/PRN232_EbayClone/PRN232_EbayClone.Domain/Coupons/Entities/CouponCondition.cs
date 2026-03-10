using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class CouponCondition : Entity<Guid>
{
    public Guid CouponId { get; private set; }
    public Coupon? Coupon { get; private set; }
    public int? BuyQuantity { get; private set; }
    public int? GetQuantity { get; private set; }
    public decimal? GetDiscountPercent { get; private set; }
    public decimal? SaveEveryAmount { get; private set; }
    public int? SaveEveryItems { get; private set; }
    public string? ConditionDescription { get; private set; }

    private CouponCondition(Guid id) : base(id)
    {
    }

    public static Result<CouponCondition> Create(
        Guid couponId,
        int? buyQuantity,
        int? getQuantity,
        decimal? getDiscountPercent,
        decimal? saveEveryAmount,
        int? saveEveryItems,
        string? conditionDescription)
    {
        var condition = new CouponCondition(Guid.NewGuid())
        {
            CouponId = couponId,
            BuyQuantity = buyQuantity,
            GetQuantity = getQuantity,
            GetDiscountPercent = getDiscountPercent,
            SaveEveryAmount = saveEveryAmount,
            SaveEveryItems = saveEveryItems,
            ConditionDescription = conditionDescription?.Trim()
        };

        return Validate(condition);
    }

    public Result Update(
        int? buyQuantity,
        int? getQuantity,
        decimal? getDiscountPercent,
        decimal? saveEveryAmount,
        int? saveEveryItems,
        string? conditionDescription)
    {
        BuyQuantity = buyQuantity;
        GetQuantity = getQuantity;
        GetDiscountPercent = getDiscountPercent;
        SaveEveryAmount = saveEveryAmount;
        SaveEveryItems = saveEveryItems;
        ConditionDescription = conditionDescription?.Trim();

        return Validate(this);
    }

    private static Result<CouponCondition> Validate(CouponCondition condition)
    {
        if (condition.GetDiscountPercent is < 0 or > 100)
        {
            return CouponErrors.InvalidCondition;
        }

        if (condition.SaveEveryAmount is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        if (condition.SaveEveryItems is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        if (condition.BuyQuantity is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        if (condition.GetQuantity is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        return Result.Success(condition);
    }
}
