using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetScheduledListingsQuery(
    string? SearchTerm = null,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<PagingResult<ScheduledListingDto>>;

public sealed record ScheduledListingDto(
    Guid Id,
    string Title,
    string Thumbnail,
    string Sku,
    ListingFormat Format,
    int AvailableQuantity,
    Duration Duration,
    decimal CurrentPrice,
    decimal StartPrice,
    decimal ReservePrice,
    decimal ShippingCost,
    DateTime? StartDate
);

public sealed class GetScheduledListingsQueryHandler : IQueryHandler<GetScheduledListingsQuery, PagingResult<ScheduledListingDto>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetScheduledListingsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagingResult<ScheduledListingDto>>> Handle(GetScheduledListingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (items, totalCount) = await _listingRepository.GetScheduledListingsAsync(
            userId,
            request.SearchTerm,
            pageNumber,
            pageSize,
            cancellationToken);

        var pagingResult = new PagingResult<ScheduledListingDto>(items, totalCount, pageNumber, pageSize);

        return Result.Success(pagingResult);
    }
}
