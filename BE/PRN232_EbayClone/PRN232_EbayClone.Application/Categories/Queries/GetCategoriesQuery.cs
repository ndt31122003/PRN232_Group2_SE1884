namespace PRN232_EbayClone.Application.Categories.Queries;

public sealed record GetCategoriesQuery(
    Guid? ParentId = null
) : IQuery<List<CategoryDto>>;

public sealed record CategoryDto(
    Guid Id,
    string Name,
    Guid? ParentId,
    bool IsLeaf
);

public sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetCategoriesAsync(request.ParentId, cancellationToken);

        var categoryDtos = categories.Select(c => new CategoryDto(
            c.Id,
            c.Name,
            c.ParentId,
            c.IsLeaf
        )).ToList();
        return categoryDtos;
    }
}
