
using PRN232_EbayClone.Domain.Categories.Errors;

namespace PRN232_EbayClone.Application.Categories.Queries;

public sealed record GetCategoryDetailQuery(
    Guid Id
) : IQuery<CategoryDetailDto>;

public sealed record CategoryDetailDto(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentId,
    bool IsLeaf,
    List<CategorySpecificDto> Specifics,
    List<ConditionDto> Conditions);

public sealed record CategorySpecificDto(
    Guid Id,
    string Name,
    bool IsRequired,
    bool AllowMultiple,
    List<string> Values);

public sealed record ConditionDto(
    Guid Id,
    string Name,
    string Description);

public sealed class GetCategoryDetailQueryHandler : IQueryHandler<GetCategoryDetailQuery, CategoryDetailDto>
{
    private readonly ICategoryRepository _categoryRepository;
    public GetCategoryDetailQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDetailDto>> Handle(GetCategoryDetailQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return CategoryErrors.NotFound;

        var dto = new CategoryDetailDto(
            Id: category.Id,
            Name: category.Name,
            Description: category.Description,
            ParentId: category.ParentId,
            IsLeaf: category.IsLeaf,
            Specifics: category.CategorySpecifics.Select(s => new CategorySpecificDto
            (
                Id: s.Id,
                Name: s.Name,
                IsRequired: s.IsRequired,
                AllowMultiple: s.AllowMultiple,
                Values: [.. s.Values]
            )).ToList(),
            Conditions: category.CategoryConditions.Select(c => new ConditionDto
            (
                Id: c.Condition.Id,
                Name: c.Condition.Name,
                Description: c.Condition.Description
            )).ToList()
        );
        return dto;
    }
}