using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Coupons;

public interface ICouponCalculationService
{
    Result<CouponCalculationResult> CalculateDiscount(
        Coupon coupon,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal);
}

public sealed class CouponCalculationResult
{
    public decimal DiscountAmount { get; init; }
    public decimal FinalAmount { get; init; }
    public string Description { get; init; } = string.Empty;
}

public sealed class CouponCalculationService : ICouponCalculationService
{
    public Result<CouponCalculationResult> CalculateDiscount(
        Coupon coupon,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal)
    {
        if (!coupon.IsActive)
        {
            return new Error("Coupon.Inactive", "Coupon is not active");
        }

        if (coupon.StartDate > DateTime.UtcNow)
        {
            return new Error("Coupon.NotStarted", "Coupon has not started yet");
        }

        if (coupon.EndDate < DateTime.UtcNow)
        {
            return new Error("Coupon.Expired", "Coupon has expired");
        }

        if (coupon.MinimumOrderValue.HasValue && orderSubtotal < coupon.MinimumOrderValue.Value)
        {
            return new Error("Coupon.MinimumOrderNotMet", 
                $"Minimum order value of {coupon.MinimumOrderValue.Value:C} not met");
        }

        var applicableItems = GetApplicableItems(coupon, orderItems);
        if (!applicableItems.Any())
        {
            return new Error("Coupon.NoApplicableItems", "No applicable items for this coupon");
        }

        var discountAmount = CalculateDiscountAmount(coupon, applicableItems, orderSubtotal);
        
        if (coupon.MaxDiscount.HasValue && discountAmount > coupon.MaxDiscount.Value)
        {
            discountAmount = coupon.MaxDiscount.Value;
        }

        var finalAmount = orderSubtotal - discountAmount;
        
        return Result.Success(new CouponCalculationResult
        {
            DiscountAmount = discountAmount,
            FinalAmount = finalAmount > 0 ? finalAmount : 0,
            Description = GenerateDescription(coupon, discountAmount)
        });
    }

    private static IReadOnlyList<OrderItem> GetApplicableItems(Coupon coupon, IReadOnlyList<OrderItem> orderItems)
    {
        var applicableItems = orderItems.ToList();

        if (coupon.CategoryId.HasValue)
        {
            applicableItems = applicableItems
                .Where(i => i.CategoryId == coupon.CategoryId.Value)
                .ToList();
        }

        if (coupon.ExcludedCategories.Any())
        {
            var excludedCategoryIds = coupon.ExcludedCategories.Select(c => c.CategoryId).ToHashSet();
            applicableItems = applicableItems
                .Where(i => i.CategoryId.HasValue && !excludedCategoryIds.Contains(i.CategoryId.Value))
                .ToList();
        }

        if (coupon.ExcludedItems.Any())
        {
            var excludedListingIds = coupon.ExcludedItems.Select(e => e.ItemId).ToHashSet();
            applicableItems = applicableItems
                .Where(i => !excludedListingIds.Contains(i.ListingId))
                .ToList();
        }

        return applicableItems;
    }

    private static decimal CalculateDiscountAmount(
        Coupon coupon,
        IReadOnlyList<OrderItem> applicableItems,
        decimal orderSubtotal)
    {
        var applicableSubtotal = applicableItems.Sum(i => i.UnitPrice.Amount * i.Quantity);

        return coupon.DiscountUnit switch
        {
            CouponDiscountUnit.Percent => CalculatePercentDiscount(coupon, applicableSubtotal, applicableItems),
            CouponDiscountUnit.Amount => CalculateAmountDiscount(coupon, applicableSubtotal, applicableItems),
            _ => 0
        };
    }

    private static decimal CalculatePercentDiscount(
        Coupon coupon,
        decimal applicableSubtotal,
        IReadOnlyList<OrderItem> applicableItems)
    {
        var baseDiscount = applicableSubtotal * (coupon.DiscountValue / 100m);

        var totalQuantity = applicableItems.Sum(i => i.Quantity);

        if (coupon.Conditions.Any(c => c.SaveEveryItems.HasValue && c.SaveEveryItems > 0))
        {
            var thresholdCondition = coupon.Conditions.First(c => c.SaveEveryItems.HasValue);
            var threshold = thresholdCondition.SaveEveryItems.Value;
            var freeItems = totalQuantity / threshold;
            var discountPerItem = applicableSubtotal / totalQuantity;
            baseDiscount = freeItems * discountPerItem * (coupon.DiscountValue / 100m);
        }

        return baseDiscount;
    }

    private static decimal CalculateAmountDiscount(
        Coupon coupon,
        decimal applicableSubtotal,
        IReadOnlyList<OrderItem> applicableItems)
    {
        if (coupon.Conditions.Any(c => c.SaveEveryAmount.HasValue && c.SaveEveryAmount > 0))
        {
            var thresholdCondition = coupon.Conditions.First(c => c.SaveEveryAmount.HasValue);
            var threshold = thresholdCondition.SaveEveryAmount.Value;
            var tiers = (int)(applicableSubtotal / threshold);
            return tiers * coupon.DiscountValue;
        }

        if (coupon.Conditions.Any(c => c.SaveEveryItems.HasValue && c.SaveEveryItems > 0))
        {
            var thresholdCondition = coupon.Conditions.First(c => c.SaveEveryItems.HasValue);
            var threshold = thresholdCondition.SaveEveryItems.Value;
            var totalQuantity = applicableItems.Sum(i => i.Quantity);
            var sets = totalQuantity / threshold;
            return sets * coupon.DiscountValue;
        }

        return coupon.DiscountValue;
    }

    private static string GenerateDescription(Coupon coupon, decimal discountAmount)
    {
        return coupon.DiscountUnit switch
        {
            CouponDiscountUnit.Percent => $"{(int)coupon.DiscountValue}% off",
            CouponDiscountUnit.Amount => $"${coupon.DiscountValue:F2} off",
            _ => $"${discountAmount:F2} discount"
        };
    }
}
