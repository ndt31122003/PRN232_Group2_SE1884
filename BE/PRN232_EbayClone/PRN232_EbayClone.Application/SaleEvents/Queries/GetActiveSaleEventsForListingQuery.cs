using PRN232_EbayClone.Application.SaleEvents.Dtos;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetActiveSaleEventsForListingQuery(Guid ListingId) : IQuery<IReadOnlyList<SaleEventDto>>;

public sealed class GetActiveSaleEventsForListingQueryValidator : AbstractValidator<GetActiveSaleEventsForListingQuery>
{
    public GetActiveSaleEventsForListingQueryValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required");
    }
}

public sealed class GetActiveSaleEventsForListingQueryHandler 
    : IQueryHandler<GetActiveSaleEventsForListingQuery, IReadOnlyList<SaleEventDto>>
{
    private readonly ISaleEventRepository _saleEventRepository;

    public GetActiveSaleEventsForListingQueryHandler(ISaleEventRepository saleEventRepository)
    {
        _saleEventRepository = saleEventRepository;
    }

    public async Task<Result<IReadOnlyList<SaleEventDto>>> Handle(
        GetActiveSaleEventsForListingQuery request,
        CancellationToken cancellationToken)
    {
        var saleEvents = await _saleEventRepository.GetActiveSaleEventsForListingAsync(
            request.ListingId,
            cancellationToken);

        var dtos = saleEvents.Select(saleEvent =>
        {
            var tierDtos = saleEvent.DiscountTiers
                .Select(t => new SaleEventDiscountTierDto(
                    t.Id,
                    t.DiscountType,
                    t.DiscountValue,
                    t.Priority,
                    t.Label,
                    t.Listings.Count))
                .OrderBy(t => t.Priority)
                .ToList();

            return new SaleEventDto(
                saleEvent.Id,
                saleEvent.Name,
                saleEvent.Description,
                saleEvent.BuyerMessageLabel,
                saleEvent.Mode,
                saleEvent.Status,
                saleEvent.StartDate,
                saleEvent.EndDate,
                saleEvent.OfferFreeShipping,
                saleEvent.BlockPriceIncreaseRevisions,
                saleEvent.IncludeSkippedItems,
                saleEvent.HighlightPercentage,
                tierDtos,
                saleEvent.Listings.Count,
                saleEvent.CreatedAt,
                saleEvent.UpdatedAt);
        }).ToList();

        return Result.Success<IReadOnlyList<SaleEventDto>>(dtos);
    }
}
