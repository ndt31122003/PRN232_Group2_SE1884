using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Dtos;
using PRN232_EbayClone.Domain.SaleEvents.Errors;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSaleEventEligibleListingsQuery(
    string? SearchTerm,
    Guid? CategoryId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int? MinDaysOnSite,
    bool ExcludeAlreadyAssigned,
    int PageNumber,
    int PageSize) : IQuery<SaleEventEligibleListingsPageDto>;

public sealed class GetSaleEventEligibleListingsQueryValidator : AbstractValidator<GetSaleEventEligibleListingsQuery>
{
    public GetSaleEventEligibleListingsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
        RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue);
        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue);
        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice!.Value)
            .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);
        RuleFor(x => x.MinDaysOnSite).GreaterThanOrEqualTo(0).When(x => x.MinDaysOnSite.HasValue);
    }
}

public sealed class GetSaleEventEligibleListingsQueryHandler : IQueryHandler<GetSaleEventEligibleListingsQuery, SaleEventEligibleListingsPageDto>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetSaleEventEligibleListingsQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<SaleEventEligibleListingsPageDto>> Handle(GetSaleEventEligibleListingsQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return SaleEventErrors.Unauthorized;
        }

        var (items, totalCount) = await _listingRepository.GetEligibleListingsForSaleEventAsync(
            sellerGuid.ToString(),
            request.SearchTerm,
            request.CategoryId,
            request.MinPrice,
            request.MaxPrice,
            request.MinDaysOnSite,
            request.ExcludeAlreadyAssigned,
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        var page = new SaleEventEligibleListingsPageDto(items, totalCount, request.PageNumber, request.PageSize);
        return Result.Success(page);
    }
}
