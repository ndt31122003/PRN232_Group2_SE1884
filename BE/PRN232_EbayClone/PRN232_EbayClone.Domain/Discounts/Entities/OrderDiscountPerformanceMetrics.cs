using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Tracks performance metrics for order discounts
/// </summary>
public sealed class OrderDiscountPerformanceMetrics : Entity<Guid>
{
    public Guid OrderDiscountId { get; private set; }
    public int OrderCount { get; private set; }
    public decimal TotalDiscountAmount { get; private set; }
    public decimal TotalSalesRevenue { get; private set; }
    public int TotalItemsSold { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public decimal AverageOrderValue => OrderCount > 0 
        ? TotalSalesRevenue / OrderCount 
        : 0;

    private OrderDiscountPerformanceMetrics(Guid id) : base(id) { }

    public static OrderDiscountPerformanceMetrics Create(Guid orderDiscountId)
    {
        return new OrderDiscountPerformanceMetrics(Guid.NewGuid())
        {
            OrderDiscountId = orderDiscountId,
            OrderCount = 0,
            TotalDiscountAmount = 0,
            TotalSalesRevenue = 0,
            TotalItemsSold = 0,
            LastUpdated = DateTime.UtcNow
        };
    }

    public void RecordOrder(decimal discountAmount, decimal orderTotal, int itemCount)
    {
        OrderCount++;
        TotalDiscountAmount += discountAmount;
        TotalSalesRevenue += orderTotal;
        TotalItemsSold += itemCount;
        LastUpdated = DateTime.UtcNow;
    }

    public void AdjustForReturn(decimal discountDifference, decimal orderTotalDifference, int itemCountDifference)
    {
        TotalDiscountAmount += discountDifference;
        TotalSalesRevenue += orderTotalDifference;
        TotalItemsSold += itemCountDifference;
        LastUpdated = DateTime.UtcNow;
    }
}
