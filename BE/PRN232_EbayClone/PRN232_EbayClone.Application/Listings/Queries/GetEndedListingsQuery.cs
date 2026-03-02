using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetEndedListingsQuery(
    string? SearchTerm = null,
    SoldStatus? SoldStatus = null,
    RelistStatus? RelistStatus = null,
    DateTime? FromDate = null,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<PagingResult<EndedListingDto>> ;

public sealed record EndedListingDto(
    Guid Id,
    string Title,
    string Thumbnail,
    string Sku,
    ListingFormat Format,
    int AvailableQuantity,
    Duration Duration,
    SoldStatus SoldStatus,
    RelistStatus RelistStatus,
    decimal CurrentPrice,
    decimal StartPrice,
    decimal ReservePrice,
    DateTime? StartDate,
    DateTime? EndDate
);

public sealed class GetEndedListingsQueryHandler : IQueryHandler<GetEndedListingsQuery, PagingResult<EndedListingDto>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetEndedListingsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagingResult<EndedListingDto>>> Handle(GetEndedListingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (items, totalCount) = await _listingRepository.GetEndedListingsAsync(
            userId,
            request.SearchTerm,
            request.SoldStatus,
            request.RelistStatus,
            request.FromDate,
            pageNumber,
            pageSize,
            cancellationToken);

        var pagingResult = new PagingResult<EndedListingDto>(items, totalCount, pageNumber, pageSize);

        return Result.Success(pagingResult);
    }
}