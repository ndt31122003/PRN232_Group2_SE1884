using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Coupons.Errors;

public static class CouponTypeErrors
{
    public static readonly Error EmptyName = Error.Failure(
        "CouponType.EmptyName",
        "Coupon type name must not be empty.");
}
