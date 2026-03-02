using PRN232_EbayClone.Application.Research.Abstractions;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Research.Queries;

public sealed record GetSourcingInsightsQuery(
    string? Keyword,
    string? Sort,
    bool SavedOnly,
    int Page,
    int PageSize,
    Guid? UserId) : IQuery<SourcingInsightsResponseDto>;

public sealed class GetSourcingInsightsQueryHandler(
    ISourcingInsightsRepository Repository)
    : IQueryHandler<GetSourcingInsightsQuery, SourcingInsightsResponseDto>
{
    private const int DefaultPageSize = 12;
    private const int MaxPageSize = 150;

    public async Task<Result<SourcingInsightsResponseDto>> Handle(
        GetSourcingInsightsQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.Page > 0 ? request.Page : 1;
        var pageSize = request.PageSize > 0 ? Math.Min(request.PageSize, MaxPageSize) : DefaultPageSize;

        var categories = await Repository.GetAllCategoriesAsync(cancellationToken);

        IReadOnlyCollection<string> savedIds = Array.Empty<string>();
        if (request.UserId.HasValue)
        {
            savedIds = await Repository.GetSavedCategoryIdsAsync(request.UserId.Value, cancellationToken);
        }

        IEnumerable<SourcingCategoryDto> filtered = categories;

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var term = request.Keyword.Trim();
            filtered = filtered.Where(category =>
                category.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                || category.Group.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (request.SavedOnly && savedIds.Count > 0)
        {
            filtered = filtered.Where(category => savedIds.Contains(category.Id));
        }
        else if (request.SavedOnly)
        {
            filtered = Enumerable.Empty<SourcingCategoryDto>();
        }

        filtered = ApplySorting(filtered, request.Sort);

        var total = filtered.Count();
        var items = filtered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var response = new SourcingInsightsResponseDto(items, savedIds, total);
        return Result.Success(response);
    }

    private static IEnumerable<SourcingCategoryDto> ApplySorting(
        IEnumerable<SourcingCategoryDto> source,
        string? sort)
    {
        return sort?.ToLowerInvariant() switch
        {
            "searchvolume" => source.OrderByDescending(category => category.SearchVolume),
            "activelistings" => source.OrderByDescending(category => category.ActiveListings),
            "sellthrough" => source.OrderByDescending(category => category.SellThroughRate),
            "alpha" => source.OrderBy(category => category.Name),
            _ => source.OrderByDescending(category => category.OpportunityScore)
        };
    }
}
