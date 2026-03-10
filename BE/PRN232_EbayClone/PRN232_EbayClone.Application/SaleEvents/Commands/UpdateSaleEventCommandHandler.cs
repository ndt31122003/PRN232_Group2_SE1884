using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.SaleEvents.Entities;
using PRN232_EbayClone.Domain.SaleEvents.Errors;
using PRN232_EbayClone.Domain.SaleEvents.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed class UpdateSaleEventCommandHandler : ICommandHandler<UpdateSaleEventCommand, Guid>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSaleEventCommandHandler(
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

    public async Task<Result<Guid>> Handle(UpdateSaleEventCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return SaleEventErrors.Unauthorized;
        }

        var sellerId = new UserId(sellerGuid);
        var sellerIdString = sellerGuid.ToString();

        var saleEvent = await _saleEventRepository.GetByIdForSellerTrackingAsync(request.SaleEventId, sellerIdString, cancellationToken);
        if (saleEvent is null)
        {
            return SaleEventErrors.NotFound;
        }

        var normalizedName = request.Name.Trim().ToLowerInvariant();
        var nameExists = await _saleEventRepository.NameExistsAsync(sellerIdString, normalizedName, request.SaleEventId, cancellationToken);
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

        var updateResult = saleEvent.Update(
            request.Name,
            request.Description,
            request.Mode,
            request.StartDate,
            request.EndDate,
            request.OfferFreeShipping,
            request.IncludeSkippedItems,
            request.BlockPriceIncreaseRevisions,
            request.HighlightPercentage,
            tierDefs,
            DateTime.UtcNow);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        _saleEventRepository.Update(saleEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(saleEvent.Id);
    }
}
