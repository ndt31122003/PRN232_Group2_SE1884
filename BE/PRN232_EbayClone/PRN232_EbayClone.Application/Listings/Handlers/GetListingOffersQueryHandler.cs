using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Listings.Dtos;
using PRN232_EbayClone.Application.Listings.Queries;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Handlers;

public sealed class GetListingOffersQueryHandler(
    IOfferRepository offerRepository,
    IUserRepository userRepository,
    IUserContext userContext)
    : IQueryHandler<GetListingOffersQuery, List<OfferDto>>
{
    public async Task<Result<List<OfferDto>>> Handle(GetListingOffersQuery request, CancellationToken cancellationToken)
    {
        var sellerId = userContext.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Result.Success(new List<OfferDto>());
        }

        var offers = await offerRepository.GetBySellerIdAsync(sellerId!, request.ListingId, cancellationToken);
        if (!offers.Any())
        {
            return Result.Success(new List<OfferDto>());
        }

        // Fetch user names for buyers
        var buyerIds = offers.Select(o => o.BuyerId).Distinct().ToList();
        var buyerLookup = new Dictionary<string, string>();

        foreach (var buyerIdStr in buyerIds)
        {
            if (Guid.TryParse(buyerIdStr, out var guid))
            {
                var user = await userRepository.GetByIdAsync(new UserId(guid), cancellationToken);
                if (user != null)
                {
                    buyerLookup[buyerIdStr] = user.FullName;
                }
            }
        }

        var result = offers.Select((PRN232_EbayClone.Domain.Listings.Entities.Offer o) => new OfferDto(
            o.Id,
            o.ListingId,
            o.Listing.Title,
            o.Listing.Images.FirstOrDefault(i => i.IsPrimary)?.Url ?? o.Listing.Images.FirstOrDefault()?.Url,
            o.BuyerId,
            buyerLookup.TryGetValue(o.BuyerId, out var name) ? name : "Unknown Buyer",
            o.Amount,
            GetListingCurrentPrice(o.Listing),
            o.Status,
            o.CreatedAt
        )).ToList();

        return Result.Success(result);
    }

    private decimal GetListingCurrentPrice(PRN232_EbayClone.Domain.Listings.Entities.Listing listing)
    {
        if (listing is PRN232_EbayClone.Domain.Listings.Entities.FixedPriceListing fp) return fp.Pricing?.Price ?? 0;
        if (listing is PRN232_EbayClone.Domain.Listings.Entities.AuctionListing auc) return auc.Pricing?.StartPrice ?? 0;
        return 0;
    }
}
