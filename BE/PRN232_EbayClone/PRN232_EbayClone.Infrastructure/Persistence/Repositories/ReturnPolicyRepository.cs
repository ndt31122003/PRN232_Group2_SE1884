using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ReturnPolicyRepository :
    Repository<ReturnPolicy, Guid>,
    IReturnPolicyRepository
{
    public ReturnPolicyRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<ReturnPolicy?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return DbContext.ReturnPolicies
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<ReturnPolicy?> GetByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken)
    {
        return DbContext.ReturnPolicies
            .SingleOrDefaultAsync(p => p.StoreId == storeId, cancellationToken);
    }
}

