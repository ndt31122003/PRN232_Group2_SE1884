using PRN232_EbayClone.Application.SaleEvents.Dtos;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSaleEventByIdQuery(Guid SaleEventId) : IQuery<SaleEventDto>;

public sealed class GetSaleEventByIdQueryValidator : AbstractValidator<GetSaleEventByIdQuery>
{
    public GetSaleEventByIdQueryValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");
    }
}

public sealed class GetSaleEventByIdQueryHandler : IQueryHandler<GetSaleEventByIdQuery, SaleEventDto>
{
    private readonly ISaleEventRepository _saleEventRepository;

    public GetSaleEventByIdQueryHandler(ISaleEventRepository saleEventRepository)
    {
        _saleEventRepository = saleEventRepository;
    }

    public async Task<Result<SaleEventDto>> Handle(GetSaleEventByIdQuery request, CancellationToken cancellationToken)
    {
        var saleEvent = await _saleEventRepository.GetByIdAsync(request.SaleEventId, cancellationToken);
        
        if (saleEvent == null)
        {
            return new Error("SaleEvent.NotFound", "Sale event not found");
        }

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

        var dto = new SaleEventDto(
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

        return Result.Success(dto);
    }
}
