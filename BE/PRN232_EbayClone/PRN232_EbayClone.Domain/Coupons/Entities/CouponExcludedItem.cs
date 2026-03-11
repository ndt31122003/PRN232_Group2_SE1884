using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class CouponExcludedItem : AggregateRoot<Guid>
{
    public Guid CouponId { get; private set; }
    public Coupon? Coupon { get; private set; }
    public Guid ItemId { get; private set; }

    private CouponExcludedItem(Guid id) : base(id)
    {
    }

    public static CouponExcludedItem Create(Guid couponId, Guid itemId)
    {
        return new CouponExcludedItem(Guid.NewGuid())
        {
            CouponId = couponId,
            ItemId = itemId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
