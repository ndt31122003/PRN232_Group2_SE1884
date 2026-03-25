using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

public sealed class SaleEventPerformanceMetrics : Entity<Guid>
{
    public Guid SaleEventId { get; private set; }
    public int OrderCount { get; private set; }
    public decimal TotalDiscountAmount { get; private set; }
    public decimal TotalSalesRevenue { get; private set; }
    public int TotalItemsSold { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public decimal AverageDiscountPerOrder => OrderCount > 0 ? TotalDiscountAmount / OrderCount : 0;
    public decimal AverageOrderValue => OrderCount > 0 ? TotalSalesRevenue / OrderCount : 0;

    private SaleEventPerformanceMetrics() : base(Guid.Empty) { }
    private SaleEventPerformanceMetrics(Guid id) : base(id) { }

    public static SaleEventPerformanceMetrics Create(Guid saleEventId)
    {
        return new SaleEventPerformanceMetrics
        {
            Id = Guid.NewGuid(),
            SaleEventId = saleEventId,
            OrderCount = 0,
            TotalDiscountAmount = 0,
            TotalSalesRevenue = 0,
            TotalItemsSold = 0,
            LastUpdated = DateTime.UtcNow
        };
    }

    public void RecordOrder(decimal discountAmount, decimal orderRevenue, int itemCount)
    {
        if (discountAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative", nameof(discountAmount));

        if (orderRevenue < 0)
            throw new ArgumentException("Order revenue cannot be negative", nameof(orderRevenue));

        if (itemCount <= 0)
            throw new ArgumentException("Item count must be greater than 0", nameof(itemCount));

        OrderCount++;
        TotalDiscountAmount += discountAmount;
        TotalSalesRevenue += orderRevenue;
        TotalItemsSold += itemCount;
        LastUpdated = DateTime.UtcNow;
    }

    public void AdjustForReturn(decimal discountAmount, decimal orderRevenue, int itemCount)
    {
        if (OrderCount <= 0)
            throw new InvalidOperationException("Cannot adjust metrics with no orders");

        OrderCount--;
        TotalDiscountAmount -= discountAmount;
        TotalSalesRevenue -= orderRevenue;
        TotalItemsSold -= itemCount;
        LastUpdated = DateTime.UtcNow;
    }
}
