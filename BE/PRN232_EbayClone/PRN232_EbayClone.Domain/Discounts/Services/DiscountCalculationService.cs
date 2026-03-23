using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.Services;

/// <summary>
/// Service để tính toán và áp dụng nhiều discount cùng lúc
/// </summary>
public sealed class DiscountCalculationService
{
    public Result<DiscountCalculationResult> CalculateTotalDiscount(
        Money orderSubtotal,
        Money shippingCost,
        int totalItemCount,
        IEnumerable<IDiscount> applicableDiscounts)
    {
        var currency = orderSubtotal.Currency;
        var zeroMoney = Money.Zero(currency);
        if (zeroMoney.IsFailure)
            return zeroMoney.Error;

        var result = new DiscountCalculationResult
        {
            OrderDiscount = zeroMoney.Value,
            ShippingDiscount = zeroMoney.Value,
            TotalDiscount = zeroMoney.Value
        };

        var currentDate = DateTime.UtcNow;
        var activeDiscounts = applicableDiscounts
            .Where(d => d.IsApplicable(currentDate))
            .ToList();

        foreach (var discount in activeDiscounts)
        {
            var discountResult = discount.CalculateDiscount(orderSubtotal, totalItemCount);
            if (discountResult.IsFailure)
                continue;

            var discountAmount = discountResult.Value;

            switch (discount.Type)
            {
                case Enums.DiscountType.ShippingDiscount:
                    var addShipping = result.ShippingDiscount + discountAmount;
                    if (addShipping.IsSuccess)
                        result.ShippingDiscount = addShipping.Value;
                    break;

                default:
                    var addOrder = result.OrderDiscount + discountAmount;
                    if (addOrder.IsSuccess)
                        result.OrderDiscount = addOrder.Value;
                    break;
            }
        }

        var totalResult = result.OrderDiscount + result.ShippingDiscount;
        if (totalResult.IsFailure)
            return totalResult.Error;

        result.TotalDiscount = totalResult.Value;
        result.AppliedDiscounts = activeDiscounts;

        return Result.Success(result);
    }
}

public sealed class DiscountCalculationResult
{
    public Money OrderDiscount { get; set; } = null!;
    public Money ShippingDiscount { get; set; } = null!;
    public Money TotalDiscount { get; set; } = null!;
    public IEnumerable<IDiscount> AppliedDiscounts { get; set; } = [];
}
