using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class OrderDiscountRepository : Repository<OrderDiscount, Guid>, IOrderDiscountRepository
{
    public OrderDiscountRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<OrderDiscount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<OrderDiscount>()
            .Include(d => d.Tiers)
            .Include(d => d.ItemRules)
            .Include(d => d.CategoryRules)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<OrderDiscount>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<OrderDiscount>()
            .Include(d => d.Tiers)
            .Include(d => d.ItemRules)
            .Include(d => d.CategoryRules)
            .Where(d => d.SellerId == sellerId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderDiscount>> GetActiveDiscountsAsync(DateTime currentDate, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<OrderDiscount>()
            .Include(d => d.Tiers)
            .Include(d => d.ItemRules)
            .Include(d => d.CategoryRules)
            .Where(d => d.IsActive && d.StartDate <= currentDate && d.EndDate >= currentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderDiscount>> GetActiveDiscountsForListingAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;
        
        return await DbContext.Set<OrderDiscount>()
            .Include(d => d.Tiers)
            .Include(d => d.ItemRules)
            .Include(d => d.CategoryRules)
            .Where(d => d.IsActive && 
                       d.StartDate <= currentDate && 
                       d.EndDate >= currentDate &&
                       (d.ApplyToAllItems || 
                        d.ItemRules.Any(r => r.ListingId == listingId && !r.IsExclusion)))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasBeenAppliedToOrdersAsync(Guid discountId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<AppliedOrderDiscount>()
            .AnyAsync(a => a.OrderDiscountId == discountId, cancellationToken);
    }

    public async Task AddAsync(OrderDiscount discount, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<OrderDiscount>().AddAsync(discount, cancellationToken);
    }

    public Task UpdateAsync(OrderDiscount discount, CancellationToken cancellationToken = default)
    {
        DbContext.Set<OrderDiscount>().Update(discount);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var discount = await GetByIdAsync(id, cancellationToken);
        if (discount != null)
        {
            DbContext.Set<OrderDiscount>().Remove(discount);
        }
    }
}

// Helper entity for tracking applied discounts
public class AppliedOrderDiscount
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid OrderDiscountId { get; set; }
    public decimal DiscountAmount { get; set; }
    public Guid? AppliedTierId { get; set; }
    public DateTime AppliedAt { get; set; }
}
