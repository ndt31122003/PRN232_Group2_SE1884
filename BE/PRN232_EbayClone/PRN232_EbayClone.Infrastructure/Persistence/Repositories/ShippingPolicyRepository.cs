using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ShippingPolicyRepository :
    Repository<ShippingPolicy, Guid>,
    IShippingPolicyRepository
{
    public ShippingPolicyRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<ShippingPolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return DbContext.ShippingPolicies
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<List<ShippingPolicy>> GetByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        return DbContext.ShippingPolicies
            .Where(p => p.StoreId == storeId)
            .ToListAsync(cancellationToken);
    }

    public Task<ShippingPolicy?> GetDefaultByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        return DbContext.ShippingPolicies
            .Where(p => p.StoreId == storeId && p.IsDefault)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

