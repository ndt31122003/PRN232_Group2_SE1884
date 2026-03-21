using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetActiveListingsQuery(
    string? SearchTerm = null,
    ListingFormat? Format = null,
    bool? OutOfStock = null,
    int PageNumber = 1,
    int PageSize = 20
) : IQuery<PagingResult<ActiveListingDto>>;

public sealed record ActiveListingDto(
    Guid Id,
    string Title,
    string Thumbnail,
    string Sku,
    ListingFormat Format,
    int AvailableQuantity,
    int SoldQuantity,
    Duration Duration,
    decimal CurrentPrice,
    string? Discounts,
    decimal? StartPrice,
    decimal? ReservePrice,
    decimal? ShippingCost,
    DateTime? StartDate,
    DateTime? EndDate,
    int WatchersCount,
    int BidsCount,
    int OffersCount,
    decimal? BestOfferAmount
);

public sealed class GetActiveListingsQueryHandler : IQueryHandler<GetActiveListingsQuery, PagingResult<ActiveListingDto>>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetActiveListingsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagingResult<ActiveListingDto>>> Handle(GetActiveListingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 200);

        var (items, totalCount) = await _listingRepository.GetActiveListingsAsync(
            userId,
            request.SearchTerm,
            request.Format,
            request.OutOfStock,
            pageNumber,
            pageSize,
            cancellationToken);

        var pagingResult = new PagingResult<ActiveListingDto>(items, totalCount, pageNumber, pageSize);

        return Result.Success(pagingResult);
    }
}
