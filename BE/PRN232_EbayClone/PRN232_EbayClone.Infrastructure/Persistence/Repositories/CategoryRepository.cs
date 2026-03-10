using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Categories.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository :
    Repository<Category, Guid>,
    ICategoryRepository
{
    public CategoryRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public override Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return DbContext.Categories
            .Include(c => c.CategorySpecifics)      
            .Include(c => c.CategoryConditions)
                .ThenInclude(cc => cc.Condition)
            .Include(c => c.Children)
            .AsSplitQuery()
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<List<Category>> GetCategoriesAsync(Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return DbContext.Categories
            .Where(c => c.ParentId == parentId)
            .Include(c => c.Children)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
