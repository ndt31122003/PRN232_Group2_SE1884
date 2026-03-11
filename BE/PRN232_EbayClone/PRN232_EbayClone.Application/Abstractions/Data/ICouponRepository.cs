using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ICouponRepository : IRepository<Coupon, Guid>
{
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default);
    Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Coupon>> GetSellerCouponsAsync(Guid sellerId, CancellationToken cancellationToken = default);
}
