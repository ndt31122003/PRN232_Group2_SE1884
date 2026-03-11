using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class CouponRepository : Repository<Coupon, Guid>, ICouponRepository
{
    public CouponRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Coupons
            .Include(c => c.Conditions)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return DbContext.Coupons
            .Include(c => c.Conditions)
            .FirstOrDefaultAsync(c => c.Code == normalizedCode, cancellationToken);
    }

    public Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return DbContext.Coupons
            .AsNoTracking()
            .AnyAsync(c => c.Code == normalizedCode, cancellationToken);
    }

    public async Task<IReadOnlyList<Coupon>> GetSellerCouponsAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Coupons
            .AsNoTracking()
            .Where(c => c.SellerId != null && c.SellerId == new UserId(sellerId))
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
