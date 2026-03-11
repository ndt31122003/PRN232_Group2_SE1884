using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class CouponTypeSeed
{
    public static readonly Guid ExtraPercentOffId = Guid.Parse("9E1D4EA5-5B09-48BE-BE90-E2790F6BA537");
    public static readonly Guid ExtraPercentOffYOrMoreItemsId = Guid.Parse("0D0C32FE-349C-4857-B20A-2D3F8DB91ED4");
    public static readonly Guid ExtraAmountOffAmountOrMoreId = Guid.Parse("CFA2E0F1-B720-4590-A7D4-4CE0844F9671");
    public static readonly Guid BuyXGetYAtPercentOffId = Guid.Parse("7EAA19CF-6B36-4A1C-B7B5-A9ABCB7EEFF2");
    public static readonly Guid BuyXGetYFreeId = Guid.Parse("ED9D5151-6F8C-4628-A5A9-4C24867E5673");
    public static readonly Guid ExtraPercentOffAmountOrMoreId = Guid.Parse("2C5A6A6A-FE7E-4813-A134-70572B5AB90A");
    public static readonly Guid ExtraAmountOffItemsThresholdId = Guid.Parse("773F8D9B-EB8E-4FF4-A21E-4BB2FA5407F4");
    public static readonly Guid ExtraAmountOffId = Guid.Parse("990C28B3-753E-41B1-A798-965CF46B7DCD");
    public static readonly Guid ExtraAmountOffEachItemId = Guid.Parse("7A5A0B7A-ED8F-4B91-A7C3-59E5363B76F3");
    public static readonly Guid SaveAmountForEveryItemsId = Guid.Parse("3B980145-62B6-4AE6-9CF8-7838BC7B84E0");
    public static readonly Guid SaveAmountForEveryAmountId = Guid.Parse("51F2ED38-06BB-496E-B5CB-7AA3057C21B7");

    public static IEnumerable<CouponType> CouponTypes => new[]
    {
        new CouponType(ExtraPercentOffId, "Extra Percent Off", "Get a percentage discount on your purchase", true),
        new CouponType(ExtraPercentOffYOrMoreItemsId, "Extra Percent Off Y+ Items", "Get a percentage discount when you buy Y or more items", true),
        new CouponType(ExtraAmountOffAmountOrMoreId, "Extra Amount Off $X+", "Get a fixed discount when your order is $X or more", true),
        new CouponType(BuyXGetYAtPercentOffId, "Buy X Get Y at % Off", "Buy X items and get Y items at a percentage discount", true),
        new CouponType(BuyXGetYFreeId, "Buy X Get Y Free", "Buy X items and get Y items for free", true),
        new CouponType(ExtraPercentOffAmountOrMoreId, "Extra Percent Off $X+", "Get a percentage discount when your order is $X or more", true),
        new CouponType(ExtraAmountOffItemsThresholdId, "Extra Amount Off Y+ Items", "Get a fixed discount when you buy Y or more items", true),
        new CouponType(ExtraAmountOffId, "Extra Amount Off", "Get a fixed discount on your purchase", true),
        new CouponType(ExtraAmountOffEachItemId, "Extra Amount Off Each Item", "Get a fixed discount on each item", true),
        new CouponType(SaveAmountForEveryItemsId, "Save $ For Every X Items", "Save money for every X items purchased", true),
        new CouponType(SaveAmountForEveryAmountId, "Save $ For Every $X", "Save money for every $X spent", true)
    };
}
