using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Dtos;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSaleEventsBySellerQuery(
    int Page = 1,
    int PageSize = 10
) : IQuery<PagedResult<SaleEventDto>>;

public sealed class GetSaleEventsBySellerQueryValidator : AbstractValidator<GetSaleEventsBySellerQuery>
{
    public GetSaleEventsBySellerQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
}

public sealed class GetSaleEventsBySellerQueryHandler : IQueryHandler<GetSaleEventsBySellerQuery, PagedResult<SaleEventDto>>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;

    public GetSaleEventsBySellerQueryHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagedResult<SaleEventDto>>> Handle(
        GetSaleEventsBySellerQuery request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return new Error("SaleEvent.Unauthorized", "User is not authorized");
        }

        var saleEvents = await _saleEventRepository.GetBySellerIdAsync(
            sellerGuid,
            request.Page,
            request.PageSize,
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

        var pagedResult = new PagedResult<SaleEventDto>(
            dtos,
            request.Page,
            request.PageSize,
            dtos.Count);

        return Result.Success(pagedResult);
    }
}

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount
);
