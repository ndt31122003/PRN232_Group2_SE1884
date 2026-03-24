using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Listings.Common;

internal static class ListingCloneFactory
{
    public static Result<Listing> Clone(Listing listing, ListingStatus status)
    {
        var images = listing.Images
            .Select(img => new ListingImage(img.Url, img.IsPrimary))
            .ToList();

        return listing switch
        {
            FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.Single
                => CloneFixedPriceSingle(fixedPrice, images, status),
            FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.MultiVariation
                => CloneFixedPriceMultiVariation(fixedPrice, images, status),
            AuctionListing auction
                => CloneAuction(auction, images, status),
            _ => Error.Failure("Listing.UnsupportedFormat", "Unsupported listing format for duplication.")
        };
    }

    public static Result<Listing> CloneAsFixedPrice(Listing listing, ListingStatus status, decimal? priceOverride = null)
    {
        var images = listing.Images
            .Select(img => new ListingImage(img.Url, img.IsPrimary))
            .ToList();

        return listing switch
        {
            FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.Single
                => CloneFixedPriceSingle(fixedPrice, images, status, priceOverride),
            FixedPriceListing fixedPrice when fixedPrice.Type == ListingType.MultiVariation
                => CloneFixedPriceMultiVariation(fixedPrice, images, status),
            AuctionListing auction
                => CloneAuctionAsFixedPrice(auction, images, status, priceOverride),
            _ => Error.Failure("Listing.UnsupportedFormat", "Unsupported listing format for duplication.")
        };
    }

    private static Result<Listing> CloneFixedPriceSingle(
        FixedPriceListing listing,
        List<ListingImage> images,
        ListingStatus status,
        decimal? priceOverride = null)
    {
        var itemSpecifics = listing.ItemSpecifics
            .Select(spec => new ItemSpecific(spec.Name, spec.Values.ToArray()))
            .ToList();

        var price = priceOverride ?? listing.Pricing.Price;

        var createResult = FixedPriceListing.CreateSingle(
            listing.Title,
            listing.Sku,
            listing.ListingDescription,
            listing.CategoryId,
            listing.ConditionId,
            listing.ConditionDescription,
            itemSpecifics,
            price,
            listing.OfferSettings.AllowOffers,
            listing.OfferSettings.MinimumOffer,
            listing.OfferSettings.AutoAcceptOffer,
            images,
            listing.Pricing.Quantity,
            listing.ShippingPolicyId,
            listing.ReturnPolicyId,
            status);
        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        return createResult.Value;
    }

    private static Result<Listing> CloneFixedPriceMultiVariation(
        FixedPriceListing listing,
        List<ListingImage> images,
        ListingStatus status)
    {
        var variations = new List<Variation>();

        foreach (var variation in listing.Variations)
        {
            var variationSpecifics = variation.VariationSpecifics
                .Select(spec => new VariationSpecific(spec.Name, spec.Values.ToArray()))
                .ToList();

            var variationImages = variation.Images
                .Select(img => new VariationImage(img.Url, img.IsPrimary))
                .ToList();

            var variationResult = Variation.Create(
                variation.Sku,
                variation.Price,
                variationSpecifics,
                variationImages,
                variation.Quantity);

            if (variationResult.IsFailure)
                return variationResult.Error;

            variations.Add(variationResult.Value);
        }

        var createResult = FixedPriceListing.CreateWithMultiVariation(
            listing.Title,
            listing.Sku,
            listing.ListingDescription,
            listing.CategoryId,
            listing.ConditionId,
            listing.ConditionDescription,
            listing.OfferSettings.AllowOffers,
            listing.OfferSettings.MinimumOffer,
            listing.OfferSettings.AutoAcceptOffer,
            variations,
            images,
            listing.ShippingPolicyId,
            listing.ReturnPolicyId,
            status);

        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        return createResult.Value;
    }

    private static Result<Listing> CloneAuction(
        AuctionListing listing,
        List<ListingImage> images,
        ListingStatus status)
    {
        var itemSpecifics = listing.ItemSpecifics
            .Select(spec => new ItemSpecific(spec.Name, spec.Values.ToArray()))
            .ToList();

        var createResult = AuctionListing.Create(
            listing.Title,
            listing.Sku,
            listing.ListingDescription,
            listing.CategoryId,
            listing.ConditionId,
            listing.ConditionDescription,
            itemSpecifics,
            listing.Pricing.StartPrice,
            listing.Pricing.ReservePrice,
            listing.Pricing.BuyItNowPrice,
            listing.Duration,
            images,
            listing.ShippingPolicyId,
            listing.ReturnPolicyId,
            status);
        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        return createResult.Value;
    }

    private static Result<Listing> CloneAuctionAsFixedPrice(
        AuctionListing listing,
        List<ListingImage> images,
        ListingStatus status,
        decimal? priceOverride)
    {
        var itemSpecifics = listing.ItemSpecifics
            .Select(spec => new ItemSpecific(spec.Name, spec.Values.ToArray()))
            .ToList();

        var price = priceOverride ?? listing.Pricing.BuyItNowPrice ?? listing.Pricing.StartPrice;
        if (price <= 0)
        {
            return Error.Failure("Listing.InvalidPrice", "Cannot convert auction to fixed price without a valid price.");
        }

        var createResult = FixedPriceListing.CreateSingle(
            listing.Title,
            listing.Sku,
            listing.ListingDescription,
            listing.CategoryId,
            listing.ConditionId,
            listing.ConditionDescription,
            itemSpecifics,
            price,
            false,
            null,
            null,
            images,
            1,
            listing.ShippingPolicyId,
            listing.ReturnPolicyId,
            status);

        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        var duplicate = createResult.Value;

        return duplicate;
    }
}
