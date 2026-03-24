using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.BuyerFeedback.Entities;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

internal sealed class SellerBlacklistRepository : Repository<SellerBlacklist, Guid>, ISellerBlacklistRepository
{
    public SellerBlacklistRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<SellerBlacklist?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<SellerBlacklist>()
            .FirstOrDefaultAsync(sb => sb.Id == id, cancellationToken);
    }

    public async Task<SellerBlacklist?> GetBySellerAndBuyerAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SellerBlacklist>()
            .FirstOrDefaultAsync(sb => sb.SellerId == sellerId && sb.BuyerId == buyerId, cancellationToken);
    }

    public async Task<List<SellerBlacklist>> GetBySellerAsync(string sellerId, bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<SellerBlacklist>()
            .Where(sb => sb.SellerId == sellerId);

        if (activeOnly)
        {
            query = query.Where(sb => sb.IsActive);
        }

        return await query
            .OrderByDescending(sb => sb.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsBlacklistedAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SellerBlacklist>()
            .AnyAsync(sb => sb.SellerId == sellerId && sb.BuyerId == buyerId && sb.IsActive, cancellationToken);
    }
}