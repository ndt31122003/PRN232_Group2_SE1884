using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Coupons.Errors;

public static class CouponErrors
{
    public static readonly Error NotFound = Error.Failure(
        "Coupon.NotFound",
        "Coupon was not found.");

    public static readonly Error EmptyName = Error.Failure(
        "Coupon.EmptyName",
        "Coupon name must not be empty.");

    public static readonly Error EmptyCode = Error.Failure(
        "Coupon.EmptyCode",
        "Coupon code must not be empty.");

    public static readonly Error InvalidCodeFormat = Error.Failure(
        "Coupon.InvalidCodeFormat",
        "Coupon code must contain only letters, digits, or hyphens.");

    public static readonly Error InvalidDateRange = Error.Failure(
        "Coupon.InvalidDateRange",
        "End date must be after start date.");

    public static readonly Error InvalidDiscountValue = Error.Failure(
        "Coupon.InvalidDiscountValue",
        "Discount value must be greater than zero.");

    public static readonly Error InvalidMaxDiscount = Error.Failure(
        "Coupon.InvalidMaxDiscount",
        "Maximum discount must be greater than zero.");

    public static readonly Error InvalidUsageLimit = Error.Failure(
        "Coupon.InvalidUsageLimit",
        "Usage limit must be greater than zero.");

    public static readonly Error InvalidUsagePerUser = Error.Failure(
        "Coupon.InvalidUsagePerUser",
        "Usage per user must be greater than zero.");

    public static readonly Error UsagePerUserExceedsLimit = Error.Failure(
        "Coupon.UsagePerUserExceedsLimit",
        "Usage per user cannot exceed the overall usage limit.");

    public static readonly Error InvalidCondition = Error.Failure(
        "Coupon.InvalidCondition",
        "Coupon condition data is invalid.");

    public static readonly Error CodeAlreadyExists = Error.Failure(
        "Coupon.CodeAlreadyExists",
        "Coupon code already exists.");

    public static readonly Error Unauthorized = Error.Failure(
        "Unauthorized",
        "You are not authorized to manage this coupon.");

    public static readonly Error CouponTypeNotFound = Error.Failure(
        "CouponType.NotFound",
        "Coupon type was not found.");

    public static readonly Error CouponTypeInactive = Error.Failure(
        "CouponType.Inactive",
        "Coupon type is inactive.");

    public static readonly Error ConditionsRequired = Error.Failure(
        "Coupon.ConditionsRequired",
        "At least one condition is required for this coupon type.");

    public static readonly Error ConditionsNotAllowed = Error.Failure(
        "Coupon.ConditionsNotAllowed",
        "Conditions are not allowed for this coupon type.");

    public static readonly Error InvalidTypeConfiguration = Error.Failure(
        "Coupon.InvalidTypeConfiguration",
        "Provided data does not satisfy the coupon type business rules.");
}
