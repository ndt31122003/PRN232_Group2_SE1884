using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.ListingTemplates.Dtos;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.ListingTemplates.Queries;

public sealed record GetListingTemplateByIdQuery(Guid Id) : IQuery<ListingTemplateDetailsDto>;

public sealed class GetListingTemplateByIdQueryHandler : IQueryHandler<GetListingTemplateByIdQuery, ListingTemplateDetailsDto>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUserContext _userContext;

    public GetListingTemplateByIdQueryHandler(IListingTemplateRepository templateRepository, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _userContext = userContext;
    }

    public async Task<Result<ListingTemplateDetailsDto>> Handle(GetListingTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingTemplateErrors.Unauthorized;
        }

        var template = await _templateRepository.GetByIdForOwnerAsync(request.Id, userId, cancellationToken);
        if (template is null)
        {
            var existing = await _templateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null)
            {
                return ListingTemplateErrors.NotFound;
            }

            if (!string.Equals(existing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
            {
                return ListingTemplateErrors.Unauthorized;
            }

            template = existing;
        }

        var dto = new ListingTemplateDetailsDto(
            template.Id,
            template.Name,
            template.Description,
            template.FormatLabel,
            template.ThumbnailUrl,
            template.CreatedAt,
            template.UpdatedAt,
            template.PayloadJson);

        return Result.Success(dto);
    }
}
