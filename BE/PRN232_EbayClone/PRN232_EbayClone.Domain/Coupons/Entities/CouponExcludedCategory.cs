using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class CouponExcludedCategory : AggregateRoot<Guid>
{
    public Guid CouponId { get; private set; }
    public Coupon? Coupon { get; private set; }
    public Guid CategoryId { get; private set; }

    private CouponExcludedCategory(Guid id) : base(id)
    {
    }

    public static CouponExcludedCategory Create(Guid couponId, Guid categoryId)
    {
        return new CouponExcludedCategory(Guid.NewGuid())
        {
            CouponId = couponId,
            CategoryId = categoryId,
            CreatedAt = DateTime.UtcNow
        };
    }
}
