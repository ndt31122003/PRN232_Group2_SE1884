using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IRepository<TEntity, TId> 
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
