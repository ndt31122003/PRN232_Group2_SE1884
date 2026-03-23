using PRN232_EbayClone.Domain.Discounts.Abstractions;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.OrderDiscounts.Services;

public interface IDiscountPriorityService
{
    /// <summary>
    /// Resolves which discount should be applied when multiple discounts are available
    /// </summary>
    /// <param name="availableDiscounts">List of available discounts</param>
    /// <param name="orderTotal">Order total amount</param>
    /// <param name="itemCount">Number of items in the order</param>
    /// <returns>The discount(s) that should be applied</returns>
    Result<IReadOnlyList<IDiscount>> ResolveDiscountPriority(
        IReadOnlyList<IDiscount> availableDiscounts,
        Money orderTotal,
        int itemCount);

    /// <summary>
    /// Calculates the total discount amount from multiple discounts
    /// </summary>
    Result<Money> CalculateTotalDiscount(
        IReadOnlyList<IDiscount> discounts,
        Money orderTotal,
        int itemCount);
}

public sealed class DiscountPriorityService : IDiscountPriorityService
{
    public Result<IReadOnlyList<IDiscount>> ResolveDiscountPriority(
        IReadOnlyList<IDiscount> availableDiscounts,
        Money orderTotal,
        int itemCount)
    {
        if (!availableDiscounts.Any())
        {
            return Result.Success<IReadOnlyList<IDiscount>>(Array.Empty<IDiscount>());
        }

        var applicableDiscounts = new List<IDiscount>();

        // Separate discounts by type
        var saleEvents = availableDiscounts.Where(d => d.Type == DiscountType.SaleEvent).ToList();
        var orderDiscounts = availableDiscounts.Where(d => d.Type == DiscountType.OrderDiscount).ToList();
        var coupons = availableDiscounts.Where(d => d.Type == DiscountType.Coupon).ToList();

        // Priority Rule 1: Sale Event vs Order Discount - best wins (mutually exclusive)
        IDiscount? bestPrimaryDiscount = null;
        Money bestPrimaryDiscountAmount = Money.Zero(orderTotal.Currency).Value;

        // Find best sale event (lowest price wins)
        if (saleEvents.Any())
        {
            IDiscount? bestSaleEvent = null;
            Money bestSaleEventAmount = Money.Zero(orderTotal.Currency).Value;

            foreach (var saleEvent in saleEvents)
            {
                var discountResult = saleEvent.CalculateDiscount(orderTotal, itemCount);
                if (discountResult.IsSuccess && discountResult.Value.Amount > bestSaleEventAmount.Amount)
                {
                    bestSaleEvent = saleEvent;
                    bestSaleEventAmount = discountResult.Value;
                }
            }

            if (bestSaleEvent != null)
            {
                bestPrimaryDiscount = bestSaleEvent;
                bestPrimaryDiscountAmount = bestSaleEventAmount;
            }
        }

        // Find best order discount
        if (orderDiscounts.Any())
        {
            foreach (var orderDiscount in orderDiscounts)
            {
                var discountResult = orderDiscount.CalculateDiscount(orderTotal, itemCount);
                if (discountResult.IsSuccess && discountResult.Value.Amount > bestPrimaryDiscountAmount.Amount)
                {
                    bestPrimaryDiscount = orderDiscount;
                    bestPrimaryDiscountAmount = discountResult.Value;
                }
            }
        }

        // Add the best primary discount (sale event or order discount)
        if (bestPrimaryDiscount != null)
        {
            applicableDiscounts.Add(bestPrimaryDiscount);
        }

        // Priority Rule 2: Coupons can stack with sale events
        // Add all applicable coupons
        foreach (var coupon in coupons)
        {
            var discountResult = coupon.CalculateDiscount(orderTotal, itemCount);
            if (discountResult.IsSuccess && discountResult.Value.Amount > 0)
            {
                applicableDiscounts.Add(coupon);
            }
        }

        return Result.Success<IReadOnlyList<IDiscount>>(applicableDiscounts);
    }

    public Result<Money> CalculateTotalDiscount(
        IReadOnlyList<IDiscount> discounts,
        Money orderTotal,
        int itemCount)
    {
        if (!discounts.Any())
        {
            var zeroMoney = Money.Zero(orderTotal.Currency);
            return zeroMoney.IsFailure ? zeroMoney.Error : zeroMoney.Value;
        }

        decimal totalDiscountAmount = 0;

        foreach (var discount in discounts)
        {
            var discountResult = discount.CalculateDiscount(orderTotal, itemCount);
            if (discountResult.IsSuccess)
            {
                totalDiscountAmount += discountResult.Value.Amount;
            }
        }

        // Ensure total discount doesn't exceed order total
        totalDiscountAmount = Math.Min(totalDiscountAmount, orderTotal.Amount);

        var totalDiscount = Money.Create(totalDiscountAmount, orderTotal.Currency);
        return totalDiscount.IsFailure ? totalDiscount.Error : totalDiscount.Value;
    }
}
