using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record CreateOfferCommand(Guid ListingId, decimal Amount) : ICommand<Guid>;

public sealed class CreateOfferCommandHandler : ICommandHandler<CreateOfferCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateOfferCommandHandler(
        IListingRepository listingRepository,
        IOfferRepository offerRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _offerRepository = offerRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<Guid>> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Auth.Unauthorized", "You must be logged in to make an offer.");
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null || listing.Status != ListingStatus.Active)
        {
            return Error.Failure("Listing.NotFound", "The listing was not found or is no longer active.");
        }

        if (listing is not FixedPriceListing fixedPriceListing)
        {
            return Error.Failure("Listing.InvalidFormat", "Only fixed-price listings accept offers.");
        }

        if (!fixedPriceListing.OfferSettings.AllowOffers)
        {
            return Error.Failure("Listing.OffersNotAllowed", "This listing does not accept offers.");
        }

        if (fixedPriceListing.OfferSettings.MinimumOffer.HasValue && request.Amount < fixedPriceListing.OfferSettings.MinimumOffer.Value)
        {
            return Error.Failure("Offer.TooLow", $"The offer amount must be at least {fixedPriceListing.OfferSettings.MinimumOffer.Value}.");
        }

        var offer = Offer.Create(request.ListingId, userId, request.Amount);
        
        // Auto-accept logic if configured
        if (fixedPriceListing.OfferSettings.AutoAcceptOffer.HasValue && request.Amount >= fixedPriceListing.OfferSettings.AutoAcceptOffer.Value)
        {
            offer.Accept();
            // In a full implementation, this might trigger Order creation immediately.
            // For now, we just mark it as accepted.
        }

        _offerRepository.Add(offer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return offer.Id;
    }
}
