using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Stores.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class StoreRepository :
    Repository<Store, StoreId>,
    IStoreRepository
{
    public StoreRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override async Task<Store?> GetByIdAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        return await DbContext.Stores
            .Include(s => s.Subscriptions)
            .SingleOrDefaultAsync(s => s.Id == storeId, cancellationToken);
    }

    public Task<Store?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return DbContext.Stores
            .Include(s => s.Subscriptions)
            .SingleOrDefaultAsync(s => s.Slug == slug, cancellationToken);
    }

    public Task<List<Store>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.Stores
            .Include(s => s.Subscriptions)
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken)
    {
        return DbContext.Stores
            .AnyAsync(s => s.Slug == slug, cancellationToken);
    }
}

