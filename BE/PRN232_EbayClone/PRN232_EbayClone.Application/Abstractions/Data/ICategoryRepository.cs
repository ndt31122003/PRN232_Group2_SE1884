using PRN232_EbayClone.Domain.Categories.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<List<Category>> GetCategoriesAsync(
        Guid? parentId = null,
        CancellationToken cancellationToken = default);
}
