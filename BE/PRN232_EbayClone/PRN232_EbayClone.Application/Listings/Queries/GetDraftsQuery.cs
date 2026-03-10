using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetDraftsQuery(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<PagingResult<DraftListingDto>>;

public sealed record DraftListingDto(
    Guid Id,
    string Title,
    string Thumbnail,
    string Sku,
    ListingFormat Format,
    int AvailableQuantity,
    decimal StartPrice,
    decimal BuyItNowPrice,
    decimal ShippingCost,
    Duration Duration,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    DateTime? ExpiredAt
);

public sealed class GetDraftsQueryHandler : IQueryHandler<GetDraftsQuery, PagingResult<DraftListingDto>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetDraftsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagingResult<DraftListingDto>>> Handle(GetDraftsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (items, totalCount) = await _listingRepository.GetDraftListingsAsync(
            userId,
            request.SearchTerm,
            pageNumber,
            pageSize,
            cancellationToken);

        var pagingResult = new PagingResult<DraftListingDto>(items, totalCount, pageNumber, pageSize);

        return Result.Success(pagingResult);
    }
}