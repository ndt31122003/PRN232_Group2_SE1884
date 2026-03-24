using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : notnull
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly IDbConnectionFactory ConnectionFactory;
    protected Repository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
    {
        DbContext = context;
        ConnectionFactory = connectionFactory;
    }

    public abstract Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    public IQueryable<TEntity> GetQueryable()
    {
        return DbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }
    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }
    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }
}
