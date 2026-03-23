
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Entities;

public sealed class FixedPriceListing(Guid id) : Listing(id)
{
    public ListingType Type { get; private set; }
    public FixedPricePricing Pricing { get; private set; } = null!;

    private readonly HashSet<Variation> _variations = [];
    public IReadOnlyCollection<Variation> Variations => _variations;
    public OfferSettings OfferSettings { get; private set; } = null!;

    public static Result<FixedPriceListing> CreateSingle(
        string title,
        string sku,
        string listingDescription,
        Guid categoryId,
        Guid? conditionId,
        string conditionDescription,
        IEnumerable<ItemSpecific> itemSpecifics,
        decimal price,
        bool allowOffers,
        decimal? minimumOffer,
        decimal? autoAcceptOffer,
        IEnumerable<ListingImage> listingImages,
        int quantity = 1,
        ListingStatus status = ListingStatus.Draft)
    {
        var offerSettingsResult = OfferSettings.Create(allowOffers, minimumOffer, autoAcceptOffer);
        if (offerSettingsResult.IsFailure) return offerSettingsResult.Error;

        var listing = new FixedPriceListing(Guid.NewGuid())
        {
            Format = ListingFormat.FixedPrice,
            Type = ListingType.Single,
            Title = title,
            Sku = sku,
            ListingDescription = listingDescription,
            CategoryId = categoryId,
            ConditionId = conditionId,
            ConditionDescription = conditionDescription,
            Pricing = new FixedPricePricing(price, quantity),
            Status = status,
            Duration = Duration.Gtc,
            OfferSettings = offerSettingsResult.Value
        };
        listing._itemSpecifics.UnionWith(itemSpecifics);

        foreach (var img in listingImages)
        {
            var addImageResult = listing.AddImage(img.Url, img.IsPrimary);
            if (addImageResult.IsFailure) return addImageResult.Error;
        }

        return listing;
    }

    public static Result<FixedPriceListing> CreateWithMultiVariation(
        string title,
        string sku,
        string listingDescription,
        Guid categoryId,
        Guid? conditionId,
        string conditionDescription,
        bool allowOffers,
        decimal? minimumOffer,
        decimal? autoAcceptOffer,
        IEnumerable<Variation> variations,
        ListingStatus status = ListingStatus.Draft)
    {
        if (variations.Count() < 2)
        {
            return Error.Failure("", "Multi variation listings must have at least 2 variations.");
        }

        var offerSettingsResult = OfferSettings.Create(allowOffers, minimumOffer, autoAcceptOffer);
        if (offerSettingsResult.IsFailure) return offerSettingsResult.Error;

        var listing = new FixedPriceListing(Guid.NewGuid())
        {
            Format = ListingFormat.FixedPrice,
            Type = ListingType.MultiVariation,
            Title = title,
            Sku = sku,
            ListingDescription = listingDescription,
            CategoryId = categoryId,
            ConditionId = conditionId,
            ConditionDescription = conditionDescription,
            Status = status,
            Duration = Duration.Gtc,
            OfferSettings = offerSettingsResult.Value
        };
        listing._variations.UnionWith(variations);
        return listing;
    }

    public Result UpdatePricing(decimal price, int quantity)
    {
        if (Type != ListingType.Single)
        {
            return Error.Failure("Listing.InvalidOperation", "Cannot update pricing for multi-variation listings.");
        }
        
        // Track price changes
        if (Pricing != null && Pricing.Price != price)
        {
            LastPriceChangeDate = DateTime.UtcNow;
        }
        
        Pricing = new FixedPricePricing(price, quantity);
        return Result.Success();
    }

    public Result UpdateVariations(IEnumerable<Variation> variations)
    {
        if (Type != ListingType.MultiVariation)
        {
            return Error.Failure("Listing.InvalidOperation", "Cannot update variations for single listings.");
        }
        if (variations.Count() < 2)
        {
            return Error.Failure("Listing.Variations", "Multi variation listings must have at least 2 variations.");
        }

        _variations.Clear();
        _variations.UnionWith(variations);


        return Result.Success();

    }

    public Result UpdateOfferSettings(bool allowOffers, decimal? minimumOffer, decimal? autoAcceptOffer)
    {
        var offerSettingsResult = OfferSettings.Create(allowOffers, minimumOffer, autoAcceptOffer);
        if (offerSettingsResult.IsFailure)
        {
            return offerSettingsResult.Error;
        }

        OfferSettings = offerSettingsResult.Value;
        return Result.Success();
    }
    protected override void ClearAllForFormatChange()
    {
        Pricing = null!;
        _variations.Clear();
    }

    public Result SetType(ListingType type)
    {
        Type = type;
        return Result.Success();
    }

    public override decimal GetEstimatedValue()
    {
        if (Type == ListingType.Single)
        {
            return Pricing.Price * Pricing.Quantity;
        }
        else
        {
            return _variations.Sum(v => v.Price * v.Quantity);
        }
    }
}