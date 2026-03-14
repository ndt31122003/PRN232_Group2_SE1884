using System;
using System.Linq;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Tests.Listings;

internal static class ListingTestData
{
    public static Category CreateLeafCategory(params CategorySpecific[] specifics)
    {
        var categoryResult = Category.Create("Cat", "Desc", specifics.ToList());
        return categoryResult.Value;
    }

    public static CategorySpecific CreateSpecific(string name, bool required = false, bool allowMultiple = true)
    {
        return CategorySpecific.Create(name, required, allowMultiple, new[] { "Default" }).Value;
    }

    public static FixedPriceListing CreateDraftSingleListing()
    {
        var listing = FixedPriceListing.CreateSingle(
            "Title",
            "SKU",
            "Description",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Condition",
            new[] { new ItemSpecific("Color", new[] { "Red" }) },
            10m,
            true,
            5m,
            8m,
            new[] { new ListingImage("http://img", true) },
            3).Value;

        return listing;
    }

    public static FixedPriceListing CreateDraftMultiVariationListing()
    {
        var variations = new[]
        {
            Variation.Create("SKU-1", 15m, new[]{ new VariationSpecific("Size", new[]{ "M" }) }, Array.Empty<VariationImage>(), 2).Value,
            Variation.Create("SKU-2", 20m, new[]{ new VariationSpecific("Size", new[]{ "L" }) }, Array.Empty<VariationImage>(), 4).Value
        };

        var listing = FixedPriceListing.CreateWithMultiVariation(
            "Title",
            "SKU",
            "Description",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Condition",
            true,
            5m,
            8m,
            variations,
            Array.Empty<ListingImage>()).Value;

        return listing;
    }

    public static AuctionListing CreateDraftAuctionListing()
    {
        var listing = AuctionListing.Create(
            "Title",
            "SKU",
            "Description",
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Condition",
            new[] { new ItemSpecific("Color", new[] { "Blue" }) },
            10m,
            20m,
            25m,
            Duration.SevenDays,
            new[] { new ListingImage("http://img", true) }).Value;

        return listing;
    }
}
