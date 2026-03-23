using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ShippingDiscountRepository : Repository<ShippingDiscount, Guid>, IShippingDiscountRepository
{
    public ShippingDiscountRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<ShippingDiscount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<ShippingDiscount>()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ShippingDiscount>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ShippingDiscount>()
            .Where(d => d.SellerId == sellerId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ShippingDiscount>> GetActiveDiscountsAsync(DateTime currentDate, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<ShippingDiscount>()
            .Where(d => d.IsActive && d.StartDate <= currentDate && d.EndDate >= currentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ShippingDiscount discount, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<ShippingDiscount>().AddAsync(discount, cancellationToken);
    }

    public Task UpdateAsync(ShippingDiscount discount, CancellationToken cancellationToken = default)
    {
        DbContext.Set<ShippingDiscount>().Update(discount);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var discount = await GetByIdAsync(id, cancellationToken);
        if (discount != null)
        {
            DbContext.Set<ShippingDiscount>().Remove(discount);
        }
    }
}
