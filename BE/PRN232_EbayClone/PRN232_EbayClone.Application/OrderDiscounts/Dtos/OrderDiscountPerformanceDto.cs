namespace PRN232_EbayClone.Application.OrderDiscounts.Dtos;

public sealed record OrderDiscountPerformanceDto(
    Guid DiscountId,
    string DiscountName,
    int OrderCount,
    decimal TotalDiscountAmount,
    decimal TotalSalesRevenue,
    int TotalItemsSold,
    decimal AverageOrderValue,
    DateTime LastUpdated);
