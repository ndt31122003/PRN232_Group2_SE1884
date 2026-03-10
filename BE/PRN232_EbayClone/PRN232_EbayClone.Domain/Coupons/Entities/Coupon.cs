using System.Text.RegularExpressions;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class Coupon : AggregateRoot<Guid>
{
    private readonly List<CouponCondition> _conditions = [];

    public Guid CouponTypeId { get; private set; }
    public CouponType CouponType { get; private set; } = null!;
    public Guid? CategoryId { get; private set; }
    public UserId? SellerId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public decimal DiscountValue { get; private set; }
    public CouponDiscountUnit DiscountUnit { get; private set; }
    public decimal? MaxDiscount { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int? UsageLimit { get; private set; }
    public int? UsagePerUser { get; private set; }
    public decimal? MinimumOrderValue { get; private set; }
    public decimal? ApplicablePriceMin { get; private set; }
    public decimal? ApplicablePriceMax { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<CouponCondition> Conditions => _conditions.AsReadOnly();

    private Coupon(Guid id) : base(id)
    {
    }

    public static Result<Coupon> Create(
        Guid couponTypeId,
    Guid? categoryId,
    UserId? sellerId,
        string name,
        string code,
        decimal discountValue,
        CouponDiscountUnit discountUnit,
        decimal? maxDiscount,
        DateTime startDate,
        DateTime endDate,
        int? usageLimit,
        int? usagePerUser,
        decimal? minimumOrderValue,
        decimal? applicablePriceMin,
        decimal? applicablePriceMax,
        bool isActive)
    {
        var coupon = new Coupon(Guid.NewGuid())
        {
            CouponTypeId = couponTypeId,
            CategoryId = categoryId,
            SellerId = sellerId,
            Name = name.Trim(),
            Code = code.Trim().ToUpperInvariant(),
            DiscountValue = discountValue,
            DiscountUnit = discountUnit,
            MaxDiscount = maxDiscount,
            StartDate = startDate,
            EndDate = endDate,
            UsageLimit = usageLimit,
            UsagePerUser = usagePerUser,
            MinimumOrderValue = minimumOrderValue,
            ApplicablePriceMin = applicablePriceMin,
            ApplicablePriceMax = applicablePriceMax,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow
        };

        return Validate(coupon);
    }

    public Result Update(
        Guid couponTypeId,
    Guid? categoryId,
    UserId? sellerId,
        string name,
        string code,
        decimal discountValue,
        CouponDiscountUnit discountUnit,
        decimal? maxDiscount,
        DateTime startDate,
        DateTime endDate,
        int? usageLimit,
        int? usagePerUser,
        decimal? minimumOrderValue,
        decimal? applicablePriceMin,
        decimal? applicablePriceMax,
        bool isActive)
    {
        CouponTypeId = couponTypeId;
        CategoryId = categoryId;
        SellerId = sellerId;
        Name = name.Trim();
        Code = code.Trim().ToUpperInvariant();
        DiscountValue = discountValue;
        DiscountUnit = discountUnit;
        MaxDiscount = maxDiscount;
        StartDate = startDate;
        EndDate = endDate;
        UsageLimit = usageLimit;
        UsagePerUser = usagePerUser;
        MinimumOrderValue = minimumOrderValue;
        ApplicablePriceMin = applicablePriceMin;
        ApplicablePriceMax = applicablePriceMax;
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;

        return Validate(this);
    }

    public Result AddCondition(CouponCondition condition)
    {
        if (condition.CouponId != Id)
        {
            return CouponErrors.InvalidCondition;
        }

        _conditions.Add(condition);
        return Result.Success();
    }

    public Result RemoveCondition(Guid conditionId)
    {
        var existing = _conditions.FirstOrDefault(x => x.Id == conditionId);
        if (existing is null)
        {
            return CouponErrors.InvalidCondition;
        }

        _conditions.Remove(existing);
        return Result.Success();
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static Result<Coupon> Validate(Coupon coupon)
    {
        if (string.IsNullOrWhiteSpace(coupon.Name))
        {
            return CouponErrors.EmptyName;
        }

        if (string.IsNullOrWhiteSpace(coupon.Code))
        {
            return CouponErrors.EmptyCode;
        }

        if (!Regex.IsMatch(coupon.Code, "^[A-Z0-9-]+$"))
        {
            return CouponErrors.InvalidCodeFormat;
        }

        if (coupon.EndDate <= coupon.StartDate)
        {
            return CouponErrors.InvalidDateRange;
        }

        if (coupon.DiscountValue <= 0)
        {
            return CouponErrors.InvalidDiscountValue;
        }

        if (coupon.MaxDiscount is <= 0)
        {
            return CouponErrors.InvalidMaxDiscount;
        }

        if (coupon.UsageLimit is <= 0)
        {
            return CouponErrors.InvalidUsageLimit;
        }

        if (coupon.UsagePerUser is <= 0)
        {
            return CouponErrors.InvalidUsagePerUser;
        }

        if (coupon.UsageLimit.HasValue && coupon.UsagePerUser.HasValue && coupon.UsagePerUser > coupon.UsageLimit)
        {
            return CouponErrors.UsagePerUserExceedsLimit;
        }

        if (coupon.MinimumOrderValue is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        if (coupon.ApplicablePriceMin is < 0 || coupon.ApplicablePriceMax is < 0)
        {
            return CouponErrors.InvalidCondition;
        }

        if (coupon.ApplicablePriceMin.HasValue && coupon.ApplicablePriceMax.HasValue &&
            coupon.ApplicablePriceMin > coupon.ApplicablePriceMax)
        {
            return CouponErrors.InvalidCondition;
        }

        return Result.Success(coupon);
    }
}
