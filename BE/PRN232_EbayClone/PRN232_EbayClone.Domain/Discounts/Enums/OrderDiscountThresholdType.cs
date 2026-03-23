namespace PRN232_EbayClone.Domain.Discounts.Enums;

/// <summary>
/// Defines the type of threshold for order discounts
/// </summary>
public enum OrderDiscountThresholdType
{
    /// <summary>
    /// Discount triggered when order subtotal meets threshold amount
    /// </summary>
    SpendBased = 1,

    /// <summary>
    /// Discount triggered when order contains threshold quantity of items
    /// </summary>
    QuantityBased = 2
}
