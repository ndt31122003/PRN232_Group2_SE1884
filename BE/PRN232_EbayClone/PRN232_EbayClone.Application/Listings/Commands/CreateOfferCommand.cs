using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Notifications.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record CreateOfferCommand(Guid ListingId, decimal Amount) : ICommand<Guid>;

public sealed class CreateOfferCommandHandler : ICommandHandler<CreateOfferCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateOfferCommandHandler(
        IListingRepository listingRepository,
        IOfferRepository offerRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        IRealtimeNotifier realtimeNotifier,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _offerRepository = offerRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _realtimeNotifier = realtimeNotifier;
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

        // Lookup buyer name
        string buyerName = userId;
        if (Guid.TryParse(userId, out var buyerGuid))
        {
            var buyer = await _userRepository.GetByIdAsync(new UserId(buyerGuid), cancellationToken);
            if (buyer is not null) buyerName = buyer.FullName;
        }

        var offer = Offer.Create(request.ListingId, userId, request.Amount);

        // Auto-accept logic if configured
        if (fixedPriceListing.OfferSettings.AutoAcceptOffer.HasValue &&
            request.Amount >= fixedPriceListing.OfferSettings.AutoAcceptOffer.Value)
        {
            offer.Accept();
        }

        _offerRepository.Add(offer);

        // Persist notification for the seller
        if (Guid.TryParse(listing.CreatedBy, out var sellerGuid))
        {
            var notification = Notification.Create(
                sellerGuid,
                "NewOffer",
                "New offer received!",
                $"{buyerName} made an offer of ${request.Amount:F2} on \"{listing.Title}\".",
                request.ListingId);

            _notificationRepository.Add(notification);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Push real-time toast to seller
        if (!string.IsNullOrEmpty(listing.CreatedBy))
        {
            var toastPayload = new
            {
                Type = "NewOffer",
                Title = "New offer received!",
                Message = $"{buyerName} offered ${request.Amount:F2} on \"{listing.Title}\".",
                ReferenceId = request.ListingId
            };

            await _realtimeNotifier.BroadcastToUserAsync(
                listing.CreatedBy, "Notification", toastPayload, cancellationToken);
        }

        return offer.Id;
    }
}
