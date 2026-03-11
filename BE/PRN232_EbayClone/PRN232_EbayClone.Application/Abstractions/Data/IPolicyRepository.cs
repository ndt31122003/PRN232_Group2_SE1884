using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IShippingPolicyRepository : IRepository<ShippingPolicy, Guid>
{
    Task<List<ShippingPolicy>> GetByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken = default);
    Task<ShippingPolicy?> GetDefaultByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken = default);
}

public interface IReturnPolicyRepository : IRepository<ReturnPolicy, Guid>
{
    Task<ReturnPolicy?> GetByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken = default);
}

