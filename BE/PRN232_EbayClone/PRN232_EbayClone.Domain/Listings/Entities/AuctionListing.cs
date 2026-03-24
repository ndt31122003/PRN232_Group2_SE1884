using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public sealed class AuctionListing(Guid id) : Listing(id)
{
    public AuctionPricing Pricing { get; private set; } = null!;
    public int BidsCount { get; private set; }

    public static Result<AuctionListing> Create(
        string title,
        string sku,
        string listingDescription,
        Guid categoryId,
        Guid? conditionId,
        string conditionDescription,
        IEnumerable<ItemSpecific> itemSpecifics,
        decimal startPrice,
        decimal? reservePrice,
        decimal? buyItNowPrice,
        Duration duration,
        IEnumerable<ListingImage> listingImages,
        Guid? shippingPolicyId = null,
        Guid? returnPolicyId = null,
        ListingStatus status = ListingStatus.Draft)
    {
        if (duration == Duration.Gtc)
        {
            return Error.Failure(
                "AuctionListing.InvalidDuration",
                "GTC duration is not allowed for auction listings.");
        }

        var buyItNowPriceValidation = IsValidBuyItNowPrice(startPrice, buyItNowPrice);
        if (buyItNowPriceValidation.IsFailure)
            return buyItNowPriceValidation.Error;

        var auctionListing = new AuctionListing(Guid.NewGuid())
        {
            Format = ListingFormat.Auction,
            Title = title,
            Sku = sku,
            ListingDescription = listingDescription,
            CategoryId = categoryId,
            ConditionId = conditionId,
            ConditionDescription = conditionDescription,
            Pricing = new AuctionPricing(startPrice, reservePrice, buyItNowPrice),
            Duration = duration,
            ShippingPolicyId = shippingPolicyId,
            ReturnPolicyId = returnPolicyId,
            Status = status
        };
        auctionListing._itemSpecifics.UnionWith(itemSpecifics);

        foreach (var img in listingImages)
        {
            var addImageResult = auctionListing.AddImage(img.Url, img.IsPrimary);
            if (addImageResult.IsFailure) return addImageResult.Error;
        }

        return auctionListing;
    }
    private static Result IsValidBuyItNowPrice(decimal startPrice, decimal? buyItNowPrice)
    {
        if (!buyItNowPrice.HasValue)
            return Result.Success();

        if (buyItNowPrice.Value >= startPrice * 1.3m)
            return Result.Success();

        return Error.Failure(
                "AuctionListing.InvalidBuyItNowPrice",
                "Buy It Now price must be at least 30% higher than the start price.");
    }

    public override decimal GetEstimatedValue()
    {
        return Pricing.BuyItNowPrice ?? Pricing.StartPrice;
    }

    public Result UpdatePricing(
        decimal startPrice,
        decimal? reservePrice,
        decimal? buyItNowPrice,
        Duration duration)
    {
        if (duration == Duration.Gtc)
        {
            return Error.Failure(
                "AuctionListing.InvalidDuration",
                "GTC duration is not allowed for auction listings.");
        }

        var buyItNowPriceValidation = IsValidBuyItNowPrice(startPrice, buyItNowPrice);
        if (buyItNowPriceValidation.IsFailure)
            return buyItNowPriceValidation.Error;

        Pricing = new AuctionPricing(startPrice, reservePrice, buyItNowPrice);
        Duration = duration;
        return Result.Success();
    }

    protected override void ClearAllForFormatChange()
    {
        Pricing = null!;
    }
}
