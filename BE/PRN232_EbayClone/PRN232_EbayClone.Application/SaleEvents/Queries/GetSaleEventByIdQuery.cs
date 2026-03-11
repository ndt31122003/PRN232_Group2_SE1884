using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSaleEventByIdQuery(Guid Id) : IQuery<SaleEventDetailDto?>;

public sealed class GetSaleEventByIdQueryHandler : IQueryHandler<GetSaleEventByIdQuery, SaleEventDetailDto?>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;

    public GetSaleEventByIdQueryHandler(ISaleEventRepository saleEventRepository, IUserContext userContext)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
    }

    public async Task<Result<SaleEventDetailDto?>> Handle(GetSaleEventByIdQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return Result.Failure<SaleEventDetailDto?>(SaleEventErrors.Unauthorized);
        }

        var sellerId = sellerGuid.ToString();
        var saleEvent = await _saleEventRepository.GetByIdForSellerAsync(request.Id, sellerId, cancellationToken);
        if (saleEvent is null)
        {
            return Result.Success<SaleEventDetailDto?>(null);
        }

        var tiers = saleEvent.DiscountTiers.Select(t => new SaleEventTierDto(
            t.Id,
            t.Priority,
            t.DiscountType,
            t.DiscountValue,
            t.Label,
            t.Listings.Select(l => l.ListingId).ToList().AsReadOnly())).ToList().AsReadOnly();

        var dto = new SaleEventDetailDto(
            saleEvent.Id,
            saleEvent.Name,
            saleEvent.Description,
            saleEvent.Mode,
            saleEvent.Status,
            saleEvent.StartDate,
            saleEvent.EndDate,
            saleEvent.OfferFreeShipping,
            saleEvent.IncludeSkippedItems,
            saleEvent.BlockPriceIncreaseRevisions,
            saleEvent.HighlightPercentage,
            tiers);

    return Result.Success<SaleEventDetailDto?>(dto);
    }
}
