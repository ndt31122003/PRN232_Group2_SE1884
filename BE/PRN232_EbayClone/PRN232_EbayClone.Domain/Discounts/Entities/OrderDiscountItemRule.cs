using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Represents an item inclusion or exclusion rule for order discounts
/// </summary>
public sealed class OrderDiscountItemRule : Entity<Guid>
{
    public Guid OrderDiscountId { get; private set; }
    public Guid ListingId { get; private set; }
    public bool IsExclusion { get; private set; }

    private OrderDiscountItemRule(Guid id) : base(id) { }

    public static Result<OrderDiscountItemRule> Create(
        Guid orderDiscountId,
        Guid listingId,
        bool isExclusion)
    {
        var rule = new OrderDiscountItemRule(Guid.NewGuid())
        {
            OrderDiscountId = orderDiscountId,
            ListingId = listingId,
            IsExclusion = isExclusion
        };

        return Result.Success(rule);
    }
}
