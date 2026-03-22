using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record PlaceBidCommand(Guid ListingId, decimal Amount) : ICommand<Guid>;

public sealed class PlaceBidCommandHandler : ICommandHandler<PlaceBidCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IBidRepository _bidRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public PlaceBidCommandHandler(
        IListingRepository listingRepository,
        IBidRepository bidRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _bidRepository = bidRepository;
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

        var bid = Bid.Create(request.ListingId, userId, request.Amount);

        _bidRepository.Add(bid);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return bid.Id;
    }
}
