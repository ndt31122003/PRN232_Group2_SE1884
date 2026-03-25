using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Discounts.Entities;

/// <summary>
/// Represents a category inclusion or exclusion rule for order discounts
/// </summary>
public sealed class OrderDiscountCategoryRule : Entity<Guid>
{
    public Guid OrderDiscountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public bool IsExclusion { get; private set; }

    private OrderDiscountCategoryRule(Guid id) : base(id) { }

    public static Result<OrderDiscountCategoryRule> Create(
        Guid orderDiscountId,
        Guid categoryId,
        bool isExclusion)
    {
        var rule = new OrderDiscountCategoryRule(Guid.NewGuid())
        {
            OrderDiscountId = orderDiscountId,
            CategoryId = categoryId,
            IsExclusion = isExclusion
        };

        return Result.Success(rule);
    }
}
