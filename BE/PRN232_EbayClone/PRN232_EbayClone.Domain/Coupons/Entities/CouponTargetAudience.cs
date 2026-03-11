using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class CouponTargetAudience : AggregateRoot<Guid>
{
    public Guid CouponId { get; private set; }
    public Coupon? Coupon { get; private set; }
    public TargetAudienceUserType UserType { get; private set; }
    public Guid? LocationId { get; private set; }
    public int? MinAccountAgeDays { get; private set; }
    public decimal? MinTotalSpent { get; private set; }

    private CouponTargetAudience(Guid id) : base(id)
    {
    }

    public static Result<CouponTargetAudience> Create(
        Guid couponId,
        TargetAudienceUserType userType,
        Guid? locationId,
        int? minAccountAgeDays,
        decimal? minTotalSpent)
    {
        var audience = new CouponTargetAudience(Guid.NewGuid())
        {
            CouponId = couponId,
            UserType = userType,
            LocationId = locationId,
            MinAccountAgeDays = minAccountAgeDays,
            MinTotalSpent = minTotalSpent,
            CreatedAt = DateTime.UtcNow
        };

        return Validate(audience);
    }

    public Result Update(
        TargetAudienceUserType userType,
        Guid? locationId,
        int? minAccountAgeDays,
        decimal? minTotalSpent)
    {
        UserType = userType;
        LocationId = locationId;
        MinAccountAgeDays = minAccountAgeDays;
        MinTotalSpent = minTotalSpent;
        UpdatedAt = DateTime.UtcNow;

        return Validate(this);
    }

    private static Result<CouponTargetAudience> Validate(CouponTargetAudience audience)
    {
        if (audience.CouponId == Guid.Empty)
        {
            return new Error("CouponTargetAudience.InvalidCouponId", "Coupon ID is required.");
        }

        if (audience.MinAccountAgeDays < 0)
        {
            return new Error("CouponTargetAudience.InvalidMinAccountAge", "Minimum account age cannot be negative.");
        }

        if (audience.MinTotalSpent < 0)
        {
            return new Error("CouponTargetAudience.InvalidMinTotalSpent", "Minimum total spent cannot be negative.");
        }

        return Result.Success(audience);
    }
}
