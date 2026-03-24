namespace PRN232_EbayClone.Domain.Discounts.Enums;

/// <summary>
/// Defines how a sale event applies discounts to listings
/// </summary>
public enum SaleEventMode
{
    /// <summary>
    /// Apply markdown discounts to listings (eBay's default mode)
    /// </summary>
    DiscountAndSaleEvent = 1,
    
    /// <summary>
    /// Show strike-through pricing without applying discounts (highlight only)
    /// </summary>
    SaleEventOnly = 2
}
