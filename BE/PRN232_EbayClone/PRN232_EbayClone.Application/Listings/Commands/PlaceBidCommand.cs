using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Notifications.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record PlaceBidCommand(Guid ListingId, decimal Amount) : ICommand<Guid>;

public sealed class PlaceBidCommandHandler : ICommandHandler<PlaceBidCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public PlaceBidCommandHandler(
        IListingRepository listingRepository,
        IBidRepository bidRepository,
        IUserRepository userRepository,
        INotificationRepository notificationRepository,
        IRealtimeNotifier realtimeNotifier,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _bidRepository = bidRepository;
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _realtimeNotifier = realtimeNotifier;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<Guid>> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Auth.Unauthorized", "You must be logged in to place a bid.");
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null || listing.Status != ListingStatus.Active)
        {
            return Error.Failure("Listing.NotFound", "The listing was not found or is no longer active.");
        }

        if (listing is not AuctionListing auctionListing)
        {
            return Error.Failure("Listing.InvalidFormat", "Bids can only be placed on auction listings.");
        }

        var highestBid = await _bidRepository.GetHighestBidByListingIdAsync(request.ListingId, cancellationToken);
        var currentPrice = highestBid?.Amount ?? auctionListing.Pricing.StartPrice;

        if (request.Amount <= currentPrice)
        {
            return Error.Failure("Bid.TooLow", $"Your bid must be higher than the current price of {currentPrice}.");
        }

        // Lookup bidder name for notifications
        string bidderName = userId;
        if (Guid.TryParse(userId, out var bidderGuid))
        {
            var bidder = await _userRepository.GetByIdAsync(new UserId(bidderGuid), cancellationToken);
            if (bidder is not null) bidderName = bidder.FullName;
        }

        var bid = Bid.Create(request.ListingId, userId, request.Amount);
        _bidRepository.Add(bid);

        // Create a notification for the seller (listing owner)
        if (Guid.TryParse(listing.CreatedBy, out var sellerGuid))
        {
            var notification = Notification.Create(
                sellerGuid,
                "NewBid",
                "New bid placed!",
                $"{bidderName} placed a bid of ${request.Amount:F2} on \"{listing.Title}\".",
                request.ListingId);

            _notificationRepository.Add(notification);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Broadcast to listing viewers (real-time update for bid panel)
        var bidUpdate = new
        {
            ListingId = request.ListingId,
            BidId = bid.Id,
            BidderName = bidderName,
            Amount = request.Amount,
            PlacedAt = bid.CreatedAt
        };

        await _realtimeNotifier.BroadcastToListingGroupAsync(
            request.ListingId, "NewBid", bidUpdate, cancellationToken);

        // Notify the seller with a toast
        if (!string.IsNullOrEmpty(listing.CreatedBy))
        {
            var toastPayload = new
            {
                Type = "NewBid",
                Title = "New bid placed!",
                Message = $"{bidderName} placed ${request.Amount:F2} on \"{listing.Title}\".",
                ReferenceId = request.ListingId
            };

            await _realtimeNotifier.BroadcastToUserAsync(
                listing.CreatedBy, "Notification", toastPayload, cancellationToken);
        }

        return bid.Id;
    }
}
