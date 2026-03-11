using PRN232_EbayClone.Domain.Stores.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IStoreRepository : IRepository<Store, StoreId>
{
    Task<Store?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Store>> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
}

