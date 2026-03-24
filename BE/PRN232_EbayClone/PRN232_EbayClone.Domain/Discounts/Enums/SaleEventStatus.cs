namespace PRN232_EbayClone.Domain.Discounts.Enums;

/// <summary>
/// Represents the lifecycle status of a sale event
/// </summary>
public enum SaleEventStatus
{
    /// <summary>
    /// Sale event is being created/edited, not yet scheduled
    /// </summary>
    Draft = 1,
    
    /// <summary>
    /// Sale event is scheduled to start in the future
    /// </summary>
    Scheduled = 2,
    
    /// <summary>
    /// Sale event is currently active and applying discounts
    /// </summary>
    Active = 3,
    
    /// <summary>
    /// Sale event has ended (past end date)
    /// </summary>
    Ended = 4,
    
    /// <summary>
    /// Sale event was manually cancelled by seller
    /// </summary>
    Cancelled = 5
}
