using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Vouchers.Entities;
using PRN232_EbayClone.Domain.Vouchers.Enums;

namespace PRN232_EbayClone.Application.Checkout;

public interface IDiscountValidationService
{
    Task<Result<DiscountValidationResult>> ValidateCouponAsync(string couponCode, Guid buyerId, IReadOnlyList<OrderItem> orderItems, decimal orderSubtotal, CancellationToken cancellationToken = default);
    Task<Result<DiscountValidationResult>> ValidateVoucherAsync(string voucherCode, Guid buyerId, IReadOnlyList<OrderItem> orderItems, decimal orderSubtotal, CancellationToken cancellationToken = default);
}

public sealed class DiscountValidationResult
{
    public DiscountType DiscountType { get; init; }
    public Guid? CouponId { get; init; }
    public Guid? VoucherId { get; init; }
    public decimal DiscountAmount { get; init; }
    public string Description { get; init; } = string.Empty;
    public bool IsValid { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> Warnings { get; init; } = [];
}

public enum DiscountType
{
    Coupon = 1,
    Voucher = 2
}

public sealed class DiscountValidationService : IDiscountValidationService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IVoucherRepository _voucherRepository;
    private readonly IUserRepository _userRepository;

    public DiscountValidationService(
        ICouponRepository couponRepository,
        IVoucherRepository voucherRepository,
        IUserRepository userRepository)
    {
        _couponRepository = couponRepository;
        _voucherRepository = voucherRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<DiscountValidationResult>> ValidateCouponAsync(
        string couponCode,
        Guid buyerId,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(couponCode))
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.EmptyCode", "Coupon code is required."));
        }

        var coupon = await _couponRepository.GetByCodeAsync(couponCode.Trim().ToUpperInvariant(), cancellationToken);
        if (coupon is null)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.NotFound", "Coupon not found."));
        }

        var validationResult = ValidateCoupon(coupon, buyerId, orderItems, orderSubtotal);
        return validationResult;
    }

    public async Task<Result<DiscountValidationResult>> ValidateVoucherAsync(
        string voucherCode,
        Guid buyerId,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(voucherCode))
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.EmptyCode", "Voucher code is required."));
        }

        var voucher = await _voucherRepository.GetByCodeAsync(voucherCode.Trim().ToUpperInvariant(), cancellationToken);
        if (voucher is null)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.NotFound", "Voucher not found."));
        }

        var validationResult = ValidateVoucher(voucher, buyerId, orderItems, orderSubtotal);
        return validationResult;
    }

    private static Result<DiscountValidationResult> ValidateCoupon(
        Coupon coupon,
        Guid buyerId,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal)
    {
        var warnings = new List<string>();

        if (!coupon.IsActive)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.Inactive", "Coupon is not active."));
        }

        if (coupon.StartDate > DateTime.UtcNow)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.NotStarted", "Coupon is not yet active."));
        }

        if (coupon.EndDate < DateTime.UtcNow)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.Expired", "Coupon has expired."));
        }

        if (coupon.MinimumOrderValue.HasValue && orderSubtotal < coupon.MinimumOrderValue.Value)
        {
            return Result.Failure<DiscountValidationResult>(new Error(
                "Discount.MinimumOrderNotMet",
                $"Minimum order value of {coupon.MinimumOrderValue.Value:C} not met. Current: {orderSubtotal:C}"));
        }

        var applicableItems = GetApplicableItemsForCoupon(coupon, orderItems);
        if (!applicableItems.Any())
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.NoApplicableItems", "No applicable items for this coupon."));
        }

        var applicableSubtotal = applicableItems.Sum(i => i.UnitPrice.Amount * i.Quantity);

        if (coupon.ApplicablePriceMin.HasValue && applicableSubtotal < coupon.ApplicablePriceMin.Value)
        {
            return Result.Failure<DiscountValidationResult>(new Error(
                "Discount.MinimumItemPriceNotMet",
                $"Minimum item price of {coupon.ApplicablePriceMin.Value:C} not met."));
        }

        if (coupon.ApplicablePriceMax.HasValue && applicableSubtotal > coupon.ApplicablePriceMax.Value)
        {
            warnings.Add($"Maximum item price is {coupon.ApplicablePriceMax.Value:C}. Excess items will not receive discount.");
        }

        if (coupon.UsageLimit.HasValue)
        {
            warnings.Add($"Coupon has limited usage. Remaining: {coupon.UsageLimit.Value}");
        }

        var discountAmount = CalculateCouponDiscount(coupon, applicableItems, applicableSubtotal);

        if (coupon.MaxDiscount.HasValue && discountAmount > coupon.MaxDiscount.Value)
        {
            discountAmount = coupon.MaxDiscount.Value;
        }

        var description = GenerateCouponDescription(coupon, discountAmount);

        return Result.Success(new DiscountValidationResult
        {
            DiscountType = DiscountType.Coupon,
            CouponId = coupon.Id,
            DiscountAmount = discountAmount,
            Description = description,
            IsValid = true,
            Warnings = warnings
        });
    }

    private static Result<DiscountValidationResult> ValidateVoucher(
        Voucher voucher,
        Guid buyerId,
        IReadOnlyList<OrderItem> orderItems,
        decimal orderSubtotal)
    {
        var warnings = new List<string>();

        if (!voucher.IsActive)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.Inactive", "Voucher is not active."));
        }

        if (voucher.ExpiryDate.HasValue && voucher.ExpiryDate.Value < DateTime.UtcNow)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.Expired", "Voucher has expired."));
        }

        if (voucher.CurrentBalance <= 0)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.InsufficientBalance", "Voucher has no balance remaining."));
        }

        if (!voucher.IsTransferable && voucher.AssignedUserId.HasValue && voucher.AssignedUserId.Value != buyerId)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.NotApplicable", "Voucher is not assigned to this user."));
        }

        if (orderSubtotal <= 0)
        {
            return Result.Failure<DiscountValidationResult>(new Error("Discount.InvalidOrder", "Order total must be greater than 0."));
        }

        var discountAmount = voucher.CurrentBalance;
        if (discountAmount > orderSubtotal)
        {
            discountAmount = orderSubtotal;
            warnings.Add($"Voucher balance exceeds order total. Remaining balance: {voucher.CurrentBalance - discountAmount:C}");
        }

        return Result.Success(new DiscountValidationResult
        {
            DiscountType = DiscountType.Voucher,
            VoucherId = voucher.Id,
            DiscountAmount = discountAmount,
            Description = $"Voucher: {voucher.Code} (-{discountAmount:C})",
            IsValid = true,
            Warnings = warnings
        });
    }

    private static IReadOnlyList<OrderItem> GetApplicableItemsForCoupon(Coupon coupon, IReadOnlyList<OrderItem> orderItems)
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

    private static decimal CalculateCouponDiscount(Coupon coupon, IReadOnlyList<OrderItem> applicableItems, decimal applicableSubtotal)
    {
        return coupon.DiscountUnit switch
        {
            CouponDiscountUnit.Percent => CalculatePercentDiscount(coupon, applicableItems, applicableSubtotal),
            CouponDiscountUnit.Amount => CalculateAmountDiscount(coupon, applicableItems, applicableSubtotal),
            _ => 0
        };
    }

    private static decimal CalculatePercentDiscount(Coupon coupon, IReadOnlyList<OrderItem> applicableItems, decimal applicableSubtotal)
    {
        var baseDiscount = applicableSubtotal * (coupon.DiscountValue / 100m);

        var totalQuantity = applicableItems.Sum(i => i.Quantity);

        if (coupon.Conditions.Any(c => c.SaveEveryItems.HasValue && c.SaveEveryItems > 0))
        {
            var thresholdCondition = coupon.Conditions.First(c => c.SaveEveryItems.HasValue);
            var threshold = thresholdCondition.SaveEveryItems.Value;
            var freeItemSets = totalQuantity / threshold;
            var discountPerItem = applicableSubtotal / totalQuantity;
            baseDiscount = freeItemSets * discountPerItem * (coupon.DiscountValue / 100m);
        }

        return baseDiscount;
    }

private static decimal CalculateAmountDiscount(Coupon coupon, IReadOnlyList<OrderItem> applicableItems, decimal applicableSubtotal)
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

        if (coupon.Conditions.Any(c => c.BuyQuantity.HasValue && c.GetQuantity.HasValue))
        {
            var condition = coupon.Conditions.First(c => c.BuyQuantity.HasValue && c.GetQuantity.HasValue);
            var totalQuantity = applicableItems.Sum(i => i.Quantity);
            var sets = totalQuantity / (condition.BuyQuantity.Value + condition.GetQuantity.Value);
            var getQuantity = condition.GetQuantity.Value;
            var discountPerItem = applicableSubtotal / totalQuantity;
            return sets * getQuantity * discountPerItem * ((condition.GetDiscountPercent ?? 100m) / 100m);
        }

        return coupon.DiscountValue;
    }

    private static string GenerateCouponDescription(Coupon coupon, decimal discountAmount)
    {
        return coupon.DiscountUnit switch
        {
            CouponDiscountUnit.Percent => $"{(int)coupon.DiscountValue}% off",
            CouponDiscountUnit.Amount => $"${coupon.DiscountValue:F2} off",
            _ => $"${discountAmount:F2} discount"
        };
    }
}
