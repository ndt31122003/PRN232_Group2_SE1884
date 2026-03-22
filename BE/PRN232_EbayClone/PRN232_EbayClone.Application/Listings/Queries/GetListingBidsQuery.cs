using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Queries;

public sealed record GetListingBidsQuery(Guid? ListingId = null) : IQuery<List<BidDto>>;

public record BidDto(
    Guid Id,
    Guid ListingId,
    string ListingTitle,
    string? ListingThumbnail,
    string BidderId,
    string BidderName,
    decimal Amount,
    DateTime CreatedAt);

public sealed class GetListingBidsQueryHandler(
    IBidRepository bidRepository,
    IUserRepository userRepository,
    IUserContext userContext)
    : IQueryHandler<GetListingBidsQuery, List<BidDto>>
{
    public async Task<Result<List<BidDto>>> Handle(GetListingBidsQuery request, CancellationToken cancellationToken)
    {
        var sellerId = userContext.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Result.Success(new List<BidDto>());
        }

        // We need a method in IBidRepository to get bids by seller Id
        // For now, let's assume we can filter after fetching or add the method
        var bids = await bidRepository.GetBySellerIdAsync(sellerId, request.ListingId, cancellationToken);
        
        if (!bids.Any())
        {
            return Result.Success(new List<BidDto>());
        }

        // Fetch user names for bidders
        var bidderIds = bids.Select(b => b.BidderId).Distinct().ToList();
        var bidderLookup = new Dictionary<string, string>();

        foreach (var bidderIdStr in bidderIds)
        {
            if (Guid.TryParse(bidderIdStr, out var guid))
            {
                var user = await userRepository.GetByIdAsync(new UserId(guid), cancellationToken);
                if (user != null)
                {
                    bidderLookup[bidderIdStr] = user.FullName;
                }
            }
        }

        var result = bids.Select(b => new BidDto(
            b.Id,
            b.ListingId,
            b.Listing.Title,
            b.Listing.Images.FirstOrDefault(i => i.IsPrimary)?.Url ?? b.Listing.Images.FirstOrDefault()?.Url,
            b.BidderId,
            bidderLookup.TryGetValue(b.BidderId, out var name) ? name : "Unknown Bidder",
            b.Amount,
            b.CreatedAt
        )).ToList();

        return Result.Success(result);
    }
}
