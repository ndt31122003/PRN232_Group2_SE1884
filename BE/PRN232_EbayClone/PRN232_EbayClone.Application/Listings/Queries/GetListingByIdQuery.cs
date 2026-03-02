using System.Linq;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetListingByIdQuery(Guid ListingId) : IQuery<ListingDetailsDto>;

public sealed record ListingDetailsDto(
    Guid ListingId,
    ListingFormat Format,
    ListingType? Type,
    string Title,
    string Sku,
    string ListingDescription,
    Guid CategoryId,
    Guid? ConditionId,
    string ConditionDescription,
    IReadOnlyCollection<ItemSpecific> ItemSpecifics,
    IReadOnlyCollection<ListingImage> ListingImages,
    decimal? Price,
    int? Quantity,
    IReadOnlyCollection<VariationDto>? Variations,
    decimal? StartPrice,
    decimal? ReservePrice,
    decimal? BuyItNowPrice,
    Duration Duration,
    DateTime? ScheduledStartTime,
    bool AllowOffers,
    decimal? MinimumOffer,
    decimal? AutoAcceptOffer,
    ListingStatus Status,
    bool IsDraft
);

public sealed class GetListingByIdQueryHandler : IQueryHandler<GetListingByIdQuery, ListingDetailsDto>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUserContext _userContext;

    public GetListingByIdQueryHandler(IListingRepository listingRepository, IUserContext userContext)
    {
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result<ListingDetailsDto>> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
        {
            return ListingErrors.NotFound;
        }

        if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
        {
            return ListingErrors.Unauthorized;
        }

        var itemSpecifics = listing.ItemSpecifics.ToList();
        var listingImages = listing.Images.ToList();

        ListingType? type = null;
        decimal? price = null;
        int? quantity = null;
        List<VariationDto>? variations = null;
        decimal? startPrice = null;
        decimal? reservePrice = null;
        decimal? buyItNowPrice = null;
        var duration = listing.Duration;
        var allowOffers = false;
        decimal? minimumOffer = null;
        decimal? autoAcceptOffer = null;

        switch (listing)
        {
            case FixedPriceListing fixedPrice:
            {
                type = fixedPrice.Type;
                allowOffers = fixedPrice.OfferSettings.AllowOffers;
                minimumOffer = fixedPrice.OfferSettings.MinimumOffer;
                autoAcceptOffer = fixedPrice.OfferSettings.AutoAcceptOffer;

                if (fixedPrice.Type == ListingType.Single)
                {
                    price = fixedPrice.Pricing.Price;
                    quantity = fixedPrice.Pricing.Quantity;
                }
                else if (fixedPrice.Type == ListingType.MultiVariation)
                {
                    variations = fixedPrice.Variations
                        .Select(variation => new VariationDto(
                            variation.Sku,
                            variation.Price,
                            variation.Quantity,
                            variation.VariationSpecifics,
                            variation.Images))
                        .ToList();
                }

                break;
            }

            case AuctionListing auction:
            {
                startPrice = auction.Pricing.StartPrice;
                reservePrice = auction.Pricing.ReservePrice;
                buyItNowPrice = auction.Pricing.BuyItNowPrice;
                duration = auction.Duration;
                break;
            }
        }

        var dto = new ListingDetailsDto(
            listing.Id,
            listing.Format,
            type,
            listing.Title,
            listing.Sku,
            listing.ListingDescription,
            listing.CategoryId,
            listing.ConditionId,
            listing.ConditionDescription,
            itemSpecifics,
            listingImages,
            price,
            quantity,
            variations,
            startPrice,
            reservePrice,
            buyItNowPrice,
            duration,
            listing.ScheduledStartTime,
            allowOffers,
            minimumOffer,
            autoAcceptOffer,
            listing.Status,
            listing.Status == ListingStatus.Draft);

        return dto;
    }
}
