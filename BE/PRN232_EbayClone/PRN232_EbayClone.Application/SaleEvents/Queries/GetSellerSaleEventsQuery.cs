using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSellerSaleEventsQuery() : IQuery<IReadOnlyList<SaleEventSummaryDto>>;

public sealed class GetSellerSaleEventsQueryHandler : IQueryHandler<GetSellerSaleEventsQuery, IReadOnlyList<SaleEventSummaryDto>>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;

    public GetSellerSaleEventsQueryHandler(ISaleEventRepository saleEventRepository, IUserContext userContext)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
    }

    public async Task<Result<IReadOnlyList<SaleEventSummaryDto>>> Handle(GetSellerSaleEventsQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return Result.Failure<IReadOnlyList<SaleEventSummaryDto>>(SaleEventErrors.Unauthorized);
        }

        var sellerId = sellerGuid.ToString();
        var events = await _saleEventRepository.GetSellerSaleEventsAsync(sellerId, cancellationToken);

        var dtos = events.Select(se => new SaleEventSummaryDto(
            se.Id,
            se.Name,
            se.Status,
            se.StartDate,
            se.EndDate,
            se.DiscountTiers.Count,
            se.Listings.Count,
            se.OfferFreeShipping,
            se.IncludeSkippedItems,
            se.BlockPriceIncreaseRevisions,
            se.HighlightPercentage)).ToList();

        return Result.Success<IReadOnlyList<SaleEventSummaryDto>>(dtos);
    }
}
