using PRN232_EbayClone.Application.Research.Dtos;

namespace PRN232_EbayClone.Application.Research.Abstractions;

public interface ISourcingInsightsRepository
{
    Task<IReadOnlyList<SourcingCategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<string>> GetSavedCategoryIdsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task SaveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default);

    Task RemoveCategoryAsync(Guid userId, string categoryId, CancellationToken cancellationToken = default);
}
