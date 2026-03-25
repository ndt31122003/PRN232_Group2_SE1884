using System;
using System.Collections.Generic;
using System.Text.Json;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class DemoSeedData
{
    private const int ListingsPerSeller = 100;
    private const string DefaultPasswordHash = "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq"; // bcrypt for 123abc@A

    private static readonly DemoSeller[] Sellers = new[]
    {
        new DemoSeller(0, Guid.Parse("70000000-0000-0000-0000-000000000001"), "Alice Johnson", "demo.seller1@example.com"),
        new DemoSeller(1, Guid.Parse("70000000-0000-0000-0000-000000000002"), "Brian Carter", "demo.seller2@example.com"),
        new DemoSeller(2, Guid.Parse("70000000-0000-0000-0000-000000000003"), "Cecilia Gomez", "demo.seller3@example.com")
    };

    internal static readonly IReadOnlyList<object> Users;
    internal static readonly IReadOnlyList<object> ActiveListings;
    internal static readonly IReadOnlyList<object> FixedPriceListings;
    internal static readonly IReadOnlyList<object> FixedPriceListingPricing;
    internal static readonly IReadOnlyList<object> FixedPriceOfferSettings;
    internal static readonly IReadOnlyList<object> ListingImages;
    internal static readonly IReadOnlyList<object> ListingTemplates;

    static DemoSeedData()
    {
        var users = new List<object>(Sellers.Length);
        var activeListings = new List<object>(Sellers.Length * ListingsPerSeller);
        var listings = new List<object>(Sellers.Length * ListingsPerSeller);
        var pricing = new List<object>(Sellers.Length * ListingsPerSeller);
        var offers = new List<object>(Sellers.Length * ListingsPerSeller);
        var images = new List<object>(Sellers.Length * ListingsPerSeller);
        var templates = new List<object>(Sellers.Length);

        var categories = new[]
        {
            CategorySeedData.CellPhonesCategoryId,
            CategorySeedData.LaptopsCategoryId,
            CategorySeedData.CamerasCategoryId,
            CategorySeedData.MensAthleticShoesCategoryId,
            CategorySeedData.KitchenAppliancesCategoryId
        };

        var conditions = new[]
        {
            ConditionSeedData.NewConditionId,
            ConditionSeedData.NewWithTagsConditionId,
            ConditionSeedData.OpenBoxConditionId,
            ConditionSeedData.PreOwnedConditionId,
            ConditionSeedData.UsedConditionId
        };

        var conditionDescriptions = new[]
        {
            "Brand new condition with original packaging.",
            "New with retail tags attached.",
            "Open box item inspected for quality.",
            "Gently used and fully functional.",
            "Used item in working condition."
        };

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var listingGlobalIndex = 0;
        var productProfiles = BuildProductProfiles();

        for (var sellerIndex = 0; sellerIndex < Sellers.Length; sellerIndex++)
        {
            var seller = Sellers[sellerIndex];
            decimal totalValue = 0m;

            for (var i = 0; i < ListingsPerSeller; i++)
            {
                var listingId = CreateListingId(sellerIndex, i);
                var createdAt = SeedDefaults.Timestamp.AddDays(-(listingGlobalIndex % 45));
                var categoryIdx = (sellerIndex + i) % categories.Length;
                var conditionIdx = (sellerIndex * 3 + i) % conditions.Length;
                var price = Math.Round(29.99m + sellerIndex * 8m + (i % 15), 2);
                var quantity = (i % 5) + 1;
                var allowOffers = i % 3 == 0;
                decimal? minimumOffer = allowOffers ? Math.Round(price * 0.90m, 2) : null;
                decimal? autoAccept = allowOffers ? Math.Round(price, 2) : null;
                var profile = BuildProductProfile(productProfiles, sellerIndex, i);

                listings.Add(new
                {
                    Id = listingId,
                    Format = ListingFormat.FixedPrice,
                    Type = ListingType.Single,
                    Status = ListingStatus.Active,
                    Title = $"{profile.Brand} {profile.Name} {profile.Variant}",
                    Sku = $"{profile.CategoryCode}-{sellerIndex + 1:D1}-{i + 1:D4}",
                    ListingDescription = $"{profile.Brand} {profile.Name} {profile.Variant} curated for {seller.FullName}'s demo storefront.",
                    CategoryId = categories[categoryIdx],
                    ConditionId = conditions[conditionIdx],
                    ConditionDescription = conditionDescriptions[conditionIdx],
                    SellerId = seller.Id,
                    CreatedAt = createdAt,
                    CreatedBy = seller.Id.ToString(),
                    UpdatedAt = (DateTime?)null,
                    UpdatedBy = (string?)null,
                    IsDeleted = false,
                    Duration = Duration.Gtc,
                    ScheduledStartTime = (DateTime?)null,
                    DraftExpiredAt = (DateTime?)null,
                    StartDate = createdAt,
                    EndDate = (DateTime?)null,
                    WatchersCount = 0
                });

                pricing.Add(new
                {
                    FixedPriceListingId = listingId,
                    Price = price,
                    Quantity = quantity
                });

                offers.Add(new
                {
                    FixedPriceListingId = listingId,
                    AllowOffers = allowOffers,
                    MinimumOffer = minimumOffer,
                    AutoAcceptOffer = autoAccept
                });

                images.Add(new
                {
                    listing_id = listingId,
                    Id = 1,
                    Url = $"https://picsum.photos/seed/{sellerIndex + 1}-{i + 1}/640/640",
                    IsPrimary = true
                });

                activeListings.Add(new
                {
                    Value = listingId,
                    seller_id = new UserId(seller.Id)
                });

                totalValue += price * quantity;
                listingGlobalIndex++;
            }

            users.Add(new
            {
                Id = new UserId(seller.Id),
                Username = seller.Email,
                FullName = seller.FullName,
                Email = new Email(seller.Email),
                PasswordHash = DefaultPasswordHash,
                CreatedAt = SeedDefaults.Timestamp,
                CreatedBy = "System",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null,
                IsDeleted = false,
                IsEmailVerified = true,
                IsPaymentVerified = true,
                IsPhoneVerified = false,
                IsBusinessVerified = false,
                PhoneNumber = (string?)null,
                BusinessName = (string?)null,
                PerformanceLevel = SellerPerformanceLevel.TopRated,
                _activeTotalValue = Math.Round(totalValue, 2)
            });

            var templatePayload = JsonSerializer.Serialize(new
            {
                title = "Sample Listing Template",
                price = 49.99m,
                quantity = 5,
                categoryId = categories[sellerIndex % categories.Length],
                conditionId = ConditionSeedData.NewConditionId
            }, serializerOptions);

            templates.Add(new
            {
                Id = CreateTemplateId(sellerIndex),
                Name = $"{seller.FullName.Split(' ')[0]}'s Starter Template",
                Description = "Reusable template seeded for demo purposes.",
                PayloadJson = templatePayload,
                FormatLabel = "Fixed Price",
                ThumbnailUrl = $"https://picsum.photos/seed/template-{sellerIndex + 1}/320/180",
                CreatedAt = SeedDefaults.Timestamp,
                CreatedBy = seller.Id.ToString(),
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null,
                IsDeleted = false
            });
        }

        Users = users;
        ActiveListings = activeListings;
        FixedPriceListings = listings;
        FixedPriceListingPricing = pricing;
        FixedPriceOfferSettings = offers;
        ListingImages = images;
        ListingTemplates = templates;
    }

    private static Guid CreateListingId(int sellerIndex, int listingIndex)
    {
        var prefix = sellerIndex switch
        {
            0 => "71000000",
            1 => "72000000",
            2 => "73000000",
            _ => throw new ArgumentOutOfRangeException(nameof(sellerIndex), sellerIndex, "Unsupported seller index")
        };

        var suffix = (listingIndex + 1).ToString("x12");
        return Guid.Parse($"{prefix}-0000-0000-0000-{suffix}");
    }

    private static Guid CreateTemplateId(int sellerIndex)
    {
        var prefix = sellerIndex switch
        {
            0 => "81000000",
            1 => "82000000",
            2 => "83000000",
            _ => throw new ArgumentOutOfRangeException(nameof(sellerIndex), sellerIndex, "Unsupported seller index")
        };

        return Guid.Parse($"{prefix}-0000-0000-0000-000000000001");
    }

    private static ProductProfile[] BuildProductProfiles()
    {
        return
        [
            new ProductProfile("MBL", ["Astra", "Nova", "Pulse", "Orbit", "Zenith"], ["Smartphone", "5G Phone", "Pocket Camera Phone"], ["128GB", "256GB", "Pro Edition", "Travel Bundle"]),
            new ProductProfile("LTP", ["Northpeak", "Vertex", "Skyline", "Atlas", "Summit"], ["Laptop", "Ultrabook", "Creator Laptop"], ["13-inch", "15-inch", "RTX Ready", "Workstation"]),
            new ProductProfile("CAM", ["Lumina", "ShutterLab", "FramePro", "Optix", "Aerial"], ["Mirrorless Camera", "Action Camera", "Vlog Camera"], ["Starter Kit", "Dual Lens", "4K Edition", "Travel Pack"]),
            new ProductProfile("SHO", ["TrailCore", "Aerofit", "PulseRun", "StreetFlex", "MotionLab"], ["Running Shoes", "Training Shoes", "Athletic Sneakers"], ["Men's", "Women's", "Lite", "All-Terrain"]),
            new ProductProfile("HOM", ["KitchenForge", "HomeNest", "DailyCraft", "Warmtable", "SteamWorks"], ["Air Fryer", "Espresso Machine", "Blender", "Rice Cooker"], ["Compact", "Family Size", "Premium", "Stainless"])
        ];
    }

    private static ProductProfile BuildProductProfile(ProductProfile[] profiles, int sellerIndex, int listingIndex)
    {
        var category = profiles[(sellerIndex + listingIndex) % profiles.Length];
        return category with
        {
            Brand = category.Brands[(sellerIndex + listingIndex) % category.Brands.Length],
            Name = category.Names[(listingIndex + sellerIndex * 2) % category.Names.Length],
            Variant = category.Variants[(listingIndex * 2 + sellerIndex) % category.Variants.Length]
        };
    }

    private sealed record DemoSeller(int Index, Guid Id, string FullName, string Email);

    private sealed record ProductProfile(
        string CategoryCode,
        string[] Brands,
        string[] Names,
        string[] Variants)
    {
        public string Brand { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Variant { get; init; } = string.Empty;
    }
}
