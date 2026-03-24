using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record ActivateSaleEventCommand(Guid SaleEventId) : ICommand;

public sealed class ActivateSaleEventCommandValidator : AbstractValidator<ActivateSaleEventCommand>
{
    public ActivateSaleEventCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");
    }
}

public sealed class ActivateSaleEventCommandHandler : ICommandHandler<ActivateSaleEventCommand>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateSaleEventCommandHandler(
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

    public async Task<Result> Handle(ActivateSaleEventCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return new Error("SaleEvent.Unauthorized", "User is not authorized");
        }

        var saleEvent = await _saleEventRepository.GetByIdAsync(request.SaleEventId, cancellationToken);
        if (saleEvent == null)
        {
            return new Error("SaleEvent.NotFound", "Sale event not found");
        }

        // Verify ownership
        if (saleEvent.SellerId != sellerGuid)
        {
            return new Error("SaleEvent.Unauthorized", "User does not own this sale event");
        }

        try
        {
            // Activate the sale event
            saleEvent.Activate();

            // Create price snapshots if price increase blocking is enabled
            if (saleEvent.BlockPriceIncreaseRevisions)
            {
                foreach (var saleEventListing in saleEvent.Listings)
                {
                    var listing = await _listingRepository.GetByIdAsync(saleEventListing.ListingId, cancellationToken);
                    if (listing != null && listing is FixedPriceListing fixedPriceListing)
                    {
                        var snapshot = SaleEventPriceSnapshot.Create(
                            saleEvent.Id,
                            listing.Id,
                            fixedPriceListing.Pricing.Price);

                        await _saleEventRepository.AddPriceSnapshotAsync(snapshot, cancellationToken);
                    }
                }
            }

            await _saleEventRepository.UpdateAsync(saleEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (InvalidOperationException ex)
        {
            return new Error("SaleEvent.ActivationFailed", ex.Message);
        }
    }
}
