using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.SaleEvents.Entities;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SaleEvents.Errors;
using PRN232_EbayClone.Domain.SaleEvents.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed class CreateSaleEventCommandHandler : ICommandHandler<CreateSaleEventCommand, Guid>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleEventCommandHandler(
        ISaleEventRepository saleEventRepository,
        IListingRepository listingRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _listingRepository = listingRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSaleEventCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return SaleEventErrors.Unauthorized;
        }

    var sellerId = new UserId(sellerGuid);
    var sellerIdString = sellerGuid.ToString();

        var normalizedName = request.Name.Trim().ToLowerInvariant();
    var nameExists = await _saleEventRepository.NameExistsAsync(sellerIdString, normalizedName, null, cancellationToken);
        if (nameExists)
        {
            return SaleEventErrors.NameAlreadyExists;
        }

        var tierDefs = request.Tiers?.Select(t => new SaleEventDiscountTierDefinition(
            t.DiscountType,
            t.DiscountValue,
            t.Priority,
            t.Label,
            t.ListingIds)).ToList();

        // validate all listing ownership
        var listingIds = tierDefs?.SelectMany(t => t.ListingIds).Distinct().ToList() ?? new List<Guid>();
        if (listingIds.Count > 0)
        {
            var listings = await _listingRepository.GetListingsForExportAsync(
                sellerIdString,
                listingIds,
                null,
                null,
                listingIds.Count,
                cancellationToken);

            if (listings.Count != listingIds.Count)
            {
                return SaleEventErrors.ListingsNotOwnedBySeller;
            }
        }

        var saleEventOrError = SaleEvent.Create(
            sellerId,
            request.Name,
            request.Description,
            request.Mode,
            request.StartDate,
            request.EndDate,
            request.OfferFreeShipping,
            request.IncludeSkippedItems,
            request.BlockPriceIncreaseRevisions,
            request.HighlightPercentage,
            tierDefs);

        if (saleEventOrError.IsFailure)
        {
            return saleEventOrError.Error;
        }

        var saleEvent = saleEventOrError.Value;

        _saleEventRepository.Add(saleEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(saleEvent.Id);
    }
}
