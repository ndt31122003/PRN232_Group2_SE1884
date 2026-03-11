using System;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class CouponTypeRepository : Repository<CouponType, Guid>, ICouponTypeRepository
{
    public CouponTypeRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<CouponType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.CouponTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);
    }

    public Task<bool> IsActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.CouponTypes
            .AsNoTracking()
            .AnyAsync(ct => ct.Id == id && ct.IsActive, cancellationToken);
    }
}
