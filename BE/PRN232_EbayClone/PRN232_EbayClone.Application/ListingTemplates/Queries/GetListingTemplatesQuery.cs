using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.ListingTemplates;
using PRN232_EbayClone.Application.ListingTemplates.Dtos;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.ListingTemplates.Queries;

public sealed record GetListingTemplatesQuery(
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<ListingTemplateListResult>;

public sealed record ListingTemplateListResult(
    IReadOnlyList<ListingTemplateDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);

public sealed class GetListingTemplatesQueryHandler : IQueryHandler<GetListingTemplatesQuery, ListingTemplateListResult>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUserContext _userContext;

    public GetListingTemplatesQueryHandler(IListingTemplateRepository templateRepository, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _userContext = userContext;
    }

    public async Task<Result<ListingTemplateListResult>> Handle(GetListingTemplatesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingTemplateErrors.Unauthorized;
        }

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (items, totalCount) = await _templateRepository.GetPagedAsync(userId, request.SearchTerm, pageNumber, pageSize, cancellationToken);

        var dtos = items
            .Select(template => new ListingTemplateDto(
                template.Id,
                template.Name,
                template.Description,
                string.IsNullOrWhiteSpace(template.FormatLabel)
                    ? ListingTemplateFormatResolver.Resolve(template.PayloadJson)
                    : template.FormatLabel,
                template.ThumbnailUrl,
                template.CreatedAt,
                template.UpdatedAt))
            .ToList();

        return Result.Success(new ListingTemplateListResult(dtos, totalCount, pageNumber, pageSize));
    }
}
