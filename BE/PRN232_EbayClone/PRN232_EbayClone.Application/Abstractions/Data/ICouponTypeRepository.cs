using System;
using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ICouponTypeRepository : IRepository<CouponType, Guid>
{
    Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default);
}
