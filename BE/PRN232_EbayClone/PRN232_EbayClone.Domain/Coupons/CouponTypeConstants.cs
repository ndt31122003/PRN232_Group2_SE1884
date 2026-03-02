using System;

namespace PRN232_EbayClone.Domain.Coupons;

public static class CouponTypeConstants
{
    public static readonly Guid ExtraPercentOff = Guid.Parse("9E1D4EA5-5B09-48BE-BE90-E2790F6BA537");
    public static readonly Guid ExtraPercentOffYOrMoreItems = Guid.Parse("0D0C32FE-349C-4857-B20A-2D3F8DB91ED4");
    public static readonly Guid ExtraAmountOffAmountOrMore = Guid.Parse("CFA2E0F1-B720-4590-A7D4-4CE0844F9671");
    public static readonly Guid BuyXGetYAtPercentOff = Guid.Parse("7EAA19CF-6B36-4A1C-B7B5-A9ABCB7EEFF2");
    public static readonly Guid BuyXGetYFree = Guid.Parse("ED9D5151-6F8C-4628-A5A9-4C24867E5673");
    public static readonly Guid ExtraPercentOffAmountOrMore = Guid.Parse("2C5A6A6A-FE7E-4813-A134-70572B5AB90A");
    public static readonly Guid ExtraAmountOffItemsThreshold = Guid.Parse("773F8D9B-EB8E-4FF4-A21E-4BB2FA5407F4");
    public static readonly Guid ExtraAmountOff = Guid.Parse("990C28B3-753E-41B1-A798-965CF46B7DCD");
    public static readonly Guid ExtraAmountOffEachItem = Guid.Parse("7A5A0B7A-ED8F-4B91-A7C3-59E5363B76F3");
    public static readonly Guid SaveAmountForEveryItems = Guid.Parse("3B980145-62B6-4AE6-9CF8-7838BC7B84E0");
    public static readonly Guid SaveAmountForEveryAmount = Guid.Parse("51F2ED38-06BB-496E-B5CB-7AA3057C21B7");
}
