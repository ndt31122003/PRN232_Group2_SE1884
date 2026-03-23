namespace PRN232_EbayClone.Domain.Discounts.Enums;

/// <summary>
/// Defines the type of discount applied in a sale event tier
/// </summary>
public enum SaleEventDiscountType
{
    /// <summary>
    /// Percentage-based discount (e.g., 20% off)
    /// </summary>
    Percent = 1,
    
    /// <summary>
    /// Fixed amount discount (e.g., $10 off)
    /// </summary>
    Amount = 2
}
