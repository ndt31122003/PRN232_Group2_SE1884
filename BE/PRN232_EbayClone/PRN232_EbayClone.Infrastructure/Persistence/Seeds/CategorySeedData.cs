using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class CategorySeedData
{
    internal static readonly Guid? RootCategoryId = null;
    internal static readonly Guid ElectronicsCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000001");
    internal static readonly Guid CellPhonesCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000002");
    internal static readonly Guid LaptopsCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000003");
    internal static readonly Guid CamerasCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000004");
    internal static readonly Guid TvHomeAudioCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000005");
    internal static readonly Guid VideoGameConsolesCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000006");
    internal static readonly Guid WearableTechCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000007");
    internal static readonly Guid SmartHomeCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000008");
    internal static readonly Guid VehicleElectronicsCategoryId = Guid.Parse("10000000-0000-0000-0000-000000000009");
    internal static readonly Guid FashionCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000001");
    internal static readonly Guid MensAthleticShoesCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000002");
    internal static readonly Guid WomensDressesCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000003");
    internal static readonly Guid MensClothingCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000004");
    internal static readonly Guid WomensHandbagsCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000005");
    internal static readonly Guid WomensShoesCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000006");
    internal static readonly Guid WatchesCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000007");
    internal static readonly Guid JewelryCategoryId = Guid.Parse("20000000-0000-0000-0000-000000000008");
    internal static readonly Guid HomeGardenCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000001");
    internal static readonly Guid KitchenAppliancesCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000002");
    internal static readonly Guid FurnitureCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000003");
    internal static readonly Guid HomeDecorCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000004");
    internal static readonly Guid ToolsWorkshopCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000005");
    internal static readonly Guid YardGardenCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000006");
    internal static readonly Guid HomeImprovementCategoryId = Guid.Parse("30000000-0000-0000-0000-000000000007");
    internal static readonly Guid MotorsCategoryId = Guid.Parse("50000000-0000-0000-0000-000000000001");
    internal static readonly Guid CarPartsAccessoriesCategoryId = Guid.Parse("50000000-0000-0000-0000-000000000002");
    internal static readonly Guid MotorcyclePartsCategoryId = Guid.Parse("50000000-0000-0000-0000-000000000003");
    internal static readonly Guid AutomotiveToolsCategoryId = Guid.Parse("50000000-0000-0000-0000-000000000004");
    internal static readonly Guid WheelsTiresCategoryId = Guid.Parse("50000000-0000-0000-0000-000000000005");
    internal static readonly Guid CollectiblesArtCategoryId = Guid.Parse("60000000-0000-0000-0000-000000000001");
    internal static readonly Guid TradingCardsCategoryId = Guid.Parse("60000000-0000-0000-0000-000000000002");
    internal static readonly Guid ComicsCategoryId = Guid.Parse("60000000-0000-0000-0000-000000000003");
    internal static readonly Guid ArtPrintsCategoryId = Guid.Parse("60000000-0000-0000-0000-000000000004");
    internal static readonly Guid CoinsPaperMoneyCategoryId = Guid.Parse("60000000-0000-0000-0000-000000000005");
    internal static readonly Guid ToysHobbiesCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000001");
    internal static readonly Guid ActionFiguresCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000002");
    internal static readonly Guid ModelRailroadsCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000003");
    internal static readonly Guid RcVehiclesCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000004");
    internal static readonly Guid DollsBearsCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000005");
    internal static readonly Guid LegoBuildingToysCategoryId = Guid.Parse("70000000-0000-0000-0000-000000000006");
    internal static readonly Guid SportingGoodsCategoryId = Guid.Parse("80000000-0000-0000-0000-000000000001");
    internal static readonly Guid OutdoorSportsCategoryId = Guid.Parse("80000000-0000-0000-0000-000000000002");
    internal static readonly Guid FitnessRunningYogaCategoryId = Guid.Parse("80000000-0000-0000-0000-000000000003");
    internal static readonly Guid CyclingCategoryId = Guid.Parse("80000000-0000-0000-0000-000000000004");
    internal static readonly Guid GolfCategoryId = Guid.Parse("80000000-0000-0000-0000-000000000005");
    internal static readonly Guid HealthBeautyCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000001");
    internal static readonly Guid MakeupCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000002");
    internal static readonly Guid SkinCareCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000003");
    internal static readonly Guid VitaminsSupplementsCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000004");
    internal static readonly Guid HairCareCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000005");
    internal static readonly Guid FragrancesCategoryId = Guid.Parse("90000000-0000-0000-0000-000000000006");
    internal static readonly Guid BusinessIndustrialCategoryId = Guid.Parse("A0000000-0000-0000-0000-000000000001");
    internal static readonly Guid HeavyEquipmentCategoryId = Guid.Parse("A0000000-0000-0000-0000-000000000002");
    internal static readonly Guid MroIndustrialSuppliesCategoryId = Guid.Parse("A0000000-0000-0000-0000-000000000003");
    internal static readonly Guid RetailServicesCategoryId = Guid.Parse("A0000000-0000-0000-0000-000000000004");
    internal static readonly Guid OfficeEquipmentCategoryId = Guid.Parse("A0000000-0000-0000-0000-000000000005");
    internal static readonly Guid MusicalInstrumentsCategoryId = Guid.Parse("B0000000-0000-0000-0000-000000000001");
    internal static readonly Guid GuitarsBassesCategoryId = Guid.Parse("B0000000-0000-0000-0000-000000000002");
    internal static readonly Guid ProAudioEquipmentCategoryId = Guid.Parse("B0000000-0000-0000-0000-000000000003");
    internal static readonly Guid DjEquipmentCategoryId = Guid.Parse("B0000000-0000-0000-0000-000000000004");
    internal static readonly Guid BrassWoodwindCategoryId = Guid.Parse("B0000000-0000-0000-0000-000000000005");
    internal static readonly Guid PetSuppliesCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000001");
    internal static readonly Guid DogSuppliesCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000002");
    internal static readonly Guid CatSuppliesCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000003");
    internal static readonly Guid FishAquariumCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000004");
    internal static readonly Guid SmallAnimalSuppliesCategoryId = Guid.Parse("C0000000-0000-0000-0000-000000000005");
    internal static readonly Guid BabyCategoryId = Guid.Parse("D0000000-0000-0000-0000-000000000001");
    internal static readonly Guid StrollersCategoryId = Guid.Parse("D0000000-0000-0000-0000-000000000002");
    internal static readonly Guid NurseryFurnitureCategoryId = Guid.Parse("D0000000-0000-0000-0000-000000000003");
    internal static readonly Guid BabySafetyCategoryId = Guid.Parse("D0000000-0000-0000-0000-000000000004");
    internal static readonly Guid BabyFeedingCategoryId = Guid.Parse("D0000000-0000-0000-0000-000000000005");
    internal static readonly Guid CraftsCategoryId = Guid.Parse("E0000000-0000-0000-0000-000000000001");
    internal static readonly Guid ScrapbookingSuppliesCategoryId = Guid.Parse("E0000000-0000-0000-0000-000000000002");
    internal static readonly Guid ArtSuppliesCategoryId = Guid.Parse("E0000000-0000-0000-0000-000000000003");
    internal static readonly Guid FabricCategoryId = Guid.Parse("E0000000-0000-0000-0000-000000000004");
    internal static readonly Guid BeadsJewelryMakingCategoryId = Guid.Parse("E0000000-0000-0000-0000-000000000005");

    internal static readonly IReadOnlyList<object> Categories = new object[]
    {
        new
        {
            Id = ElectronicsCategoryId,
            Name = "Electronics",
            Description = "Consumer electronics, components, and accessories.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CellPhonesCategoryId,
            Name = "Cell Phones & Smartphones",
            Description = "Smartphones and cell phone devices.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = LaptopsCategoryId,
            Name = "Laptops & Netbooks",
            Description = "Portable computers and accessories.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CamerasCategoryId,
            Name = "Cameras & Photo",
            Description = "Digital cameras and photography equipment.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FashionCategoryId,
            Name = "Fashion",
            Description = "Apparel, shoes, and accessories.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = MensAthleticShoesCategoryId,
            Name = "Men's Athletic Shoes",
            Description = "Performance and casual athletic footwear for men.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WomensDressesCategoryId,
            Name = "Women's Dresses",
            Description = "Dresses for every style and occasion.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = HomeGardenCategoryId,
            Name = "Home & Garden",
            Description = "Home improvement, décor, and outdoor living.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = KitchenAppliancesCategoryId,
            Name = "Small Kitchen Appliances",
            Description = "Countertop appliances and kitchen helpers.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FurnitureCategoryId,
            Name = "Furniture",
            Description = "Indoor and outdoor furniture collections.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Electronics extensions
        new
        {
            Id = TvHomeAudioCategoryId,
            Name = "TV, Video & Home Audio",
            Description = "Televisions, speakers, and streaming devices.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = VideoGameConsolesCategoryId,
            Name = "Video Game Consoles",
            Description = "Home and handheld gaming systems.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WearableTechCategoryId,
            Name = "Wearable Technology",
            Description = "Smartwatches, fitness trackers, and smart eyewear.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = SmartHomeCategoryId,
            Name = "Smart Home",
            Description = "Connected home devices and automation hubs.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = VehicleElectronicsCategoryId,
            Name = "Vehicle Electronics & GPS",
            Description = "Navigation, dash cams, and in-car entertainment.",
            ParentId = ElectronicsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Fashion extensions
        new
        {
            Id = MensClothingCategoryId,
            Name = "Men's Clothing",
            Description = "Casual, business, and formal apparel for men.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WomensHandbagsCategoryId,
            Name = "Women's Handbags & Bags",
            Description = "Designer totes, crossbody bags, and backpacks.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WomensShoesCategoryId,
            Name = "Women's Shoes",
            Description = "Heels, flats, and casual footwear.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WatchesCategoryId,
            Name = "Watches",
            Description = "Timepieces ranging from vintage to luxury.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = JewelryCategoryId,
            Name = "Fine Jewelry",
            Description = "Rings, necklaces, and bracelets crafted in precious metals.",
            ParentId = FashionCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Home & Garden extensions
        new
        {
            Id = HomeDecorCategoryId,
            Name = "Home Décor",
            Description = "Interior accents, wall art, and lighting.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ToolsWorkshopCategoryId,
            Name = "Tools & Workshop Equipment",
            Description = "Power tools and shop essentials.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = YardGardenCategoryId,
            Name = "Yard, Garden & Outdoor Living",
            Description = "Outdoor décor, landscaping, and patio gear.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = HomeImprovementCategoryId,
            Name = "Home Improvement",
            Description = "Building supplies, fixtures, and hardware.",
            ParentId = HomeGardenCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Motors
        new
        {
            Id = MotorsCategoryId,
            Name = "eBay Motors",
            Description = "Complete automotive marketplace for vehicles and parts.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CarPartsAccessoriesCategoryId,
            Name = "Car Parts & Accessories",
            Description = "OEM and aftermarket components for every ride.",
            ParentId = MotorsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = MotorcyclePartsCategoryId,
            Name = "Motorcycle Parts",
            Description = "Upgrades and replacement parts for bikes.",
            ParentId = MotorsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = AutomotiveToolsCategoryId,
            Name = "Automotive Tools & Supplies",
            Description = "Garage lifts, diagnostics, and specialty tools.",
            ParentId = MotorsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = WheelsTiresCategoryId,
            Name = "Wheels, Tires & Parts",
            Description = "Rims, tire sets, TPMS sensors, and more.",
            ParentId = MotorsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Collectibles & Art
        new
        {
            Id = CollectiblesArtCategoryId,
            Name = "Collectibles & Art",
            Description = "Treasures from pop culture, history, and fine art.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = TradingCardsCategoryId,
            Name = "Collectible Card Games",
            Description = "TCG singles, sealed product, and memorabilia.",
            ParentId = CollectiblesArtCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ComicsCategoryId,
            Name = "Comics & Graphic Novels",
            Description = "Golden Age through modern runs and collectibles.",
            ParentId = CollectiblesArtCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ArtPrintsCategoryId,
            Name = "Art Prints",
            Description = "Limited editions, lithographs, and posters.",
            ParentId = CollectiblesArtCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CoinsPaperMoneyCategoryId,
            Name = "Coins & Paper Money",
            Description = "Graded coins, bullion, and currency.",
            ParentId = CollectiblesArtCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Toys & Hobbies
        new
        {
            Id = ToysHobbiesCategoryId,
            Name = "Toys & Hobbies",
            Description = "Playsets, model kits, and collector favorites.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ActionFiguresCategoryId,
            Name = "Action Figures",
            Description = "Superheroes, anime, and pop-culture icons.",
            ParentId = ToysHobbiesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ModelRailroadsCategoryId,
            Name = "Model Railroads & Trains",
            Description = "Locomotives, rolling stock, and scenery kits.",
            ParentId = ToysHobbiesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = RcVehiclesCategoryId,
            Name = "RC Model Vehicles & Kits",
            Description = "Radio-controlled cars, drones, and planes.",
            ParentId = ToysHobbiesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = DollsBearsCategoryId,
            Name = "Dolls & Bears",
            Description = "Barbie, Blythe, Build-A-Bear, and more.",
            ParentId = ToysHobbiesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = LegoBuildingToysCategoryId,
            Name = "LEGO & Building Toys",
            Description = "Modular builds and sealed collectible sets.",
            ParentId = ToysHobbiesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Sporting Goods
        new
        {
            Id = SportingGoodsCategoryId,
            Name = "Sporting Goods",
            Description = "Gear for every sport, indoors and out.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = OutdoorSportsCategoryId,
            Name = "Outdoor Sports",
            Description = "Camping, hiking, hunting, and fishing gear.",
            ParentId = SportingGoodsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FitnessRunningYogaCategoryId,
            Name = "Fitness, Running & Yoga",
            Description = "Exercise machines, apparel, and accessories.",
            ParentId = SportingGoodsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CyclingCategoryId,
            Name = "Cycling",
            Description = "Bikes, parts, helmets, and apparel.",
            ParentId = SportingGoodsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = GolfCategoryId,
            Name = "Golf",
            Description = "Clubs, balls, carts, and training aids.",
            ParentId = SportingGoodsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Health & Beauty
        new
        {
            Id = HealthBeautyCategoryId,
            Name = "Health & Beauty",
            Description = "Wellness essentials and personal care favorites.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = MakeupCategoryId,
            Name = "Makeup",
            Description = "Cosmetics, palettes, and tools.",
            ParentId = HealthBeautyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = SkinCareCategoryId,
            Name = "Skin Care",
            Description = "Serums, moisturizers, and devices.",
            ParentId = HealthBeautyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = VitaminsSupplementsCategoryId,
            Name = "Vitamins & Dietary Supplements",
            Description = "Wellness, immunity, and performance blends.",
            ParentId = HealthBeautyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = HairCareCategoryId,
            Name = "Hair Care",
            Description = "Styling tools, treatments, and color.",
            ParentId = HealthBeautyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FragrancesCategoryId,
            Name = "Fragrances",
            Description = "Perfumes, colognes, and body mists.",
            ParentId = HealthBeautyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Business & Industrial
        new
        {
            Id = BusinessIndustrialCategoryId,
            Name = "Business & Industrial",
            Description = "Equipment, supplies, and services for every trade.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = HeavyEquipmentCategoryId,
            Name = "Heavy Equipment",
            Description = "Excavators, loaders, and industrial vehicles.",
            ParentId = BusinessIndustrialCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = MroIndustrialSuppliesCategoryId,
            Name = "MRO & Industrial Supplies",
            Description = "Maintenance, repair, and operations essentials.",
            ParentId = BusinessIndustrialCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = RetailServicesCategoryId,
            Name = "Retail & Services",
            Description = "Point-of-sale, signage, and consulting packages.",
            ParentId = BusinessIndustrialCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = OfficeEquipmentCategoryId,
            Name = "Office Equipment",
            Description = "Printers, copiers, and office machines.",
            ParentId = BusinessIndustrialCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Musical Instruments & Gear
        new
        {
            Id = MusicalInstrumentsCategoryId,
            Name = "Musical Instruments & Gear",
            Description = "Instruments, pro audio, and stage equipment.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = GuitarsBassesCategoryId,
            Name = "Guitars & Basses",
            Description = "Electric, acoustic, and bass guitars.",
            ParentId = MusicalInstrumentsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ProAudioEquipmentCategoryId,
            Name = "Pro Audio Equipment",
            Description = "Mixers, microphones, and studio gear.",
            ParentId = MusicalInstrumentsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = DjEquipmentCategoryId,
            Name = "DJ Equipment",
            Description = "Controllers, turntables, and lighting.",
            ParentId = MusicalInstrumentsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = BrassWoodwindCategoryId,
            Name = "Brass & Woodwind",
            Description = "Saxes, trumpets, clarinets, and accessories.",
            ParentId = MusicalInstrumentsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Pet Supplies
        new
        {
            Id = PetSuppliesCategoryId,
            Name = "Pet Supplies",
            Description = "Care essentials for pets large and small.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = DogSuppliesCategoryId,
            Name = "Dog Supplies",
            Description = "Beds, crates, and training essentials.",
            ParentId = PetSuppliesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = CatSuppliesCategoryId,
            Name = "Cat Supplies",
            Description = "Litter, scratchers, and cat furniture.",
            ParentId = PetSuppliesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FishAquariumCategoryId,
            Name = "Fish & Aquarium",
            Description = "Aquariums, filtration, and décor.",
            ParentId = PetSuppliesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = SmallAnimalSuppliesCategoryId,
            Name = "Small Animal Supplies",
            Description = "Habitat accessories for hamsters, rabbits, and more.",
            ParentId = PetSuppliesCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Baby Essentials
        new
        {
            Id = BabyCategoryId,
            Name = "Baby Essentials",
            Description = "Nursery gear, travel systems, and feeding must-haves.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = StrollersCategoryId,
            Name = "Strollers & Travel Systems",
            Description = "Lightweight, jogging, and convertible options.",
            ParentId = BabyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = NurseryFurnitureCategoryId,
            Name = "Nursery Furniture",
            Description = "Cribs, dressers, and gliders.",
            ParentId = BabyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = BabySafetyCategoryId,
            Name = "Baby Safety",
            Description = "Monitors, gates, and proofing essentials.",
            ParentId = BabyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = BabyFeedingCategoryId,
            Name = "Baby Feeding",
            Description = "Bottles, warmers, and nursing support.",
            ParentId = BabyCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        // Crafts
        new
        {
            Id = CraftsCategoryId,
            Name = "Crafts",
            Description = "DIY staples spanning every creative discipline.",
            ParentId = RootCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ScrapbookingSuppliesCategoryId,
            Name = "Scrapbooking & Paper Crafting",
            Description = "Stamps, dies, and embellishments.",
            ParentId = CraftsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ArtSuppliesCategoryId,
            Name = "Art Supplies",
            Description = "Paints, canvases, and studio tools.",
            ParentId = CraftsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = FabricCategoryId,
            Name = "Fabric",
            Description = "Yardage, quilting, and upholstery textiles.",
            ParentId = CraftsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = BeadsJewelryMakingCategoryId,
            Name = "Beads & Jewelry Making",
            Description = "Findings, gemstones, and tools.",
            ParentId = CraftsCategoryId,
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        }
    };

    internal static readonly IReadOnlyList<object> CategorySpecifics = new object[]
    {
        new
        {
            Id = Guid.Parse("10100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CellPhonesCategoryId,
            _values = new HashSet<string>
            {
                "Apple",
                "Samsung",
                "Google",
                "OnePlus",
                "Motorola"
            }
        },
        new
        {
            Id = Guid.Parse("10100000-0000-0000-0000-000000000002"),
            Name = "Model",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CellPhonesCategoryId,
            _values = new HashSet<string>
            {
                "iPhone 15",
                "Galaxy S24",
                "Pixel 8",
                "OnePlus 12",
                "Moto G Power"
            }
        },
        new
        {
            Id = Guid.Parse("10100000-0000-0000-0000-000000000003"),
            Name = "Storage Capacity",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CellPhonesCategoryId,
            _values = new HashSet<string>
            {
                "64 GB",
                "128 GB",
                "256 GB",
                "512 GB",
                "1 TB"
            }
        },
        new
        {
            Id = Guid.Parse("10100000-0000-0000-0000-000000000004"),
            Name = "Color",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CellPhonesCategoryId,
            _values = new HashSet<string>
            {
                "Black",
                "White",
                "Blue",
                "Red",
                "Purple"
            }
        },
        new
        {
            Id = Guid.Parse("10100000-0000-0000-0000-000000000005"),
            Name = "Network",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CellPhonesCategoryId,
            _values = new HashSet<string>
            {
                "Unlocked",
                "AT&T",
                "Verizon",
                "T-Mobile",
                "US Cellular"
            }
        },
        new
        {
            Id = Guid.Parse("10200000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = LaptopsCategoryId,
            _values = new HashSet<string>
            {
                "Apple",
                "Dell",
                "HP",
                "Lenovo",
                "ASUS"
            }
        },
        new
        {
            Id = Guid.Parse("10200000-0000-0000-0000-000000000002"),
            Name = "Processor",
            IsRequired = true,
            AllowMultiple = false,
            category_id = LaptopsCategoryId,
            _values = new HashSet<string>
            {
                "Intel Core i5",
                "Intel Core i7",
                "AMD Ryzen 5",
                "AMD Ryzen 7",
                "Apple M2"
            }
        },
        new
        {
            Id = Guid.Parse("10200000-0000-0000-0000-000000000003"),
            Name = "RAM Size",
            IsRequired = true,
            AllowMultiple = false,
            category_id = LaptopsCategoryId,
            _values = new HashSet<string>
            {
                "8 GB",
                "16 GB",
                "32 GB",
                "64 GB"
            }
        },
        new
        {
            Id = Guid.Parse("10200000-0000-0000-0000-000000000004"),
            Name = "Storage Type",
            IsRequired = false,
            AllowMultiple = false,
            category_id = LaptopsCategoryId,
            _values = new HashSet<string>
            {
                "SSD",
                "HDD",
                "Hybrid"
            }
        },
        new
        {
            Id = Guid.Parse("10200000-0000-0000-0000-000000000005"),
            Name = "Screen Size",
            IsRequired = false,
            AllowMultiple = false,
            category_id = LaptopsCategoryId,
            _values = new HashSet<string>
            {
                "13 in",
                "14 in",
                "15.6 in",
                "17 in"
            }
        },
        new
        {
            Id = Guid.Parse("10300000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CamerasCategoryId,
            _values = new HashSet<string>
            {
                "Canon",
                "Nikon",
                "Sony",
                "Fujifilm",
                "Panasonic"
            }
        },
        new
        {
            Id = Guid.Parse("10300000-0000-0000-0000-000000000002"),
            Name = "Camera Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CamerasCategoryId,
            _values = new HashSet<string>
            {
                "DSLR",
                "Mirrorless",
                "Point & Shoot",
                "Action"
            }
        },
        new
        {
            Id = Guid.Parse("10300000-0000-0000-0000-000000000003"),
            Name = "Maximum Resolution",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CamerasCategoryId,
            _values = new HashSet<string>
            {
                "12 MP",
                "16 MP",
                "24 MP",
                "32 MP"
            }
        },
        new
        {
            Id = Guid.Parse("10300000-0000-0000-0000-000000000004"),
            Name = "Optical Zoom",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CamerasCategoryId,
            _values = new HashSet<string>
            {
                "None",
                "5x",
                "10x",
                "20x"
            }
        },
        new
        {
            Id = Guid.Parse("10300000-0000-0000-0000-000000000005"),
            Name = "Series",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CamerasCategoryId,
            _values = new HashSet<string>
            {
                "EOS",
                "Alpha",
                "X-T",
                "Lumix"
            }
        },
        new
        {
            Id = Guid.Parse("20100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MensAthleticShoesCategoryId,
            _values = new HashSet<string>
            {
                "Nike",
                "Adidas",
                "New Balance",
                "Puma",
                "Under Armour"
            }
        },
        new
        {
            Id = Guid.Parse("20100000-0000-0000-0000-000000000002"),
            Name = "US Shoe Size",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MensAthleticShoesCategoryId,
            _values = new HashSet<string>
            {
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13"
            }
        },
        new
        {
            Id = Guid.Parse("20100000-0000-0000-0000-000000000003"),
            Name = "Color",
            IsRequired = false,
            AllowMultiple = false,
            category_id = MensAthleticShoesCategoryId,
            _values = new HashSet<string>
            {
                "Black",
                "White",
                "Red",
                "Blue",
                "Gray"
            }
        },
        new
        {
            Id = Guid.Parse("20100000-0000-0000-0000-000000000004"),
            Name = "Style",
            IsRequired = false,
            AllowMultiple = false,
            category_id = MensAthleticShoesCategoryId,
            _values = new HashSet<string>
            {
                "Low Top",
                "Mid Top",
                "High Top"
            }
        },
        new
        {
            Id = Guid.Parse("20100000-0000-0000-0000-000000000005"),
            Name = "Width",
            IsRequired = false,
            AllowMultiple = false,
            category_id = MensAthleticShoesCategoryId,
            _values = new HashSet<string>
            {
                "Standard",
                "Wide",
                "Extra Wide"
            }
        },
        new
        {
            Id = Guid.Parse("20200000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WomensDressesCategoryId,
            _values = new HashSet<string>
            {
                "ASOS",
                "Free People",
                "H&M",
                "Reformation",
                "Zara"
            }
        },
        new
        {
            Id = Guid.Parse("20200000-0000-0000-0000-000000000002"),
            Name = "Size Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WomensDressesCategoryId,
            _values = new HashSet<string>
            {
                "Regular",
                "Petite",
                "Plus",
                "Tall"
            }
        },
        new
        {
            Id = Guid.Parse("20200000-0000-0000-0000-000000000003"),
            Name = "Dress Length",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WomensDressesCategoryId,
            _values = new HashSet<string>
            {
                "Mini",
                "Knee Length",
                "Midi",
                "Maxi"
            }
        },
        new
        {
            Id = Guid.Parse("20200000-0000-0000-0000-000000000004"),
            Name = "Material",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WomensDressesCategoryId,
            _values = new HashSet<string>
            {
                "Cotton",
                "Linen",
                "Polyester",
                "Silk"
            }
        },
        new
        {
            Id = Guid.Parse("20200000-0000-0000-0000-000000000005"),
            Name = "Pattern",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WomensDressesCategoryId,
            _values = new HashSet<string>
            {
                "Solid",
                "Floral",
                "Striped",
                "Polka Dot",
                "Animal Print"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = KitchenAppliancesCategoryId,
            _values = new HashSet<string>
            {
                "Breville",
                "Cuisinart",
                "Instant Pot",
                "KitchenAid",
                "Ninja"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000002"),
            Name = "Appliance Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = KitchenAppliancesCategoryId,
            _values = new HashSet<string>
            {
                "Air Fryer",
                "Blender",
                "Coffee Maker",
                "Mixer",
                "Pressure Cooker"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000003"),
            Name = "Color",
            IsRequired = false,
            AllowMultiple = false,
            category_id = KitchenAppliancesCategoryId,
            _values = new HashSet<string>
            {
                "Black",
                "Red",
                "Silver",
                "Stainless Steel",
                "White"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000004"),
            Name = "Power Source",
            IsRequired = false,
            AllowMultiple = false,
            category_id = KitchenAppliancesCategoryId,
            _values = new HashSet<string>
            {
                "Electric",
                "Battery",
                "Manual"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000005"),
            Name = "Capacity",
            IsRequired = false,
            AllowMultiple = false,
            category_id = KitchenAppliancesCategoryId,
            _values = new HashSet<string>
            {
                "2 qt",
                "4 qt",
                "6 qt",
                "8 qt"
            }
        },
        new
        {
            Id = Guid.Parse("30200000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FurnitureCategoryId,
            _values = new HashSet<string>
            {
                "Ashley",
                "Crate & Barrel",
                "IKEA",
                "West Elm",
                "Wayfair"
            }
        },
        new
        {
            Id = Guid.Parse("30200000-0000-0000-0000-000000000002"),
            Name = "Room",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FurnitureCategoryId,
            _values = new HashSet<string>
            {
                "Bedroom",
                "Dining Room",
                "Home Office",
                "Living Room",
                "Patio"
            }
        },
        new
        {
            Id = Guid.Parse("30200000-0000-0000-0000-000000000003"),
            Name = "Material",
            IsRequired = false,
            AllowMultiple = false,
            category_id = FurnitureCategoryId,
            _values = new HashSet<string>
            {
                "Fabric",
                "Glass",
                "Leather",
                "Metal",
                "Wood"
            }
        },
        new
        {
            Id = Guid.Parse("30200000-0000-0000-0000-000000000004"),
            Name = "Color",
            IsRequired = false,
            AllowMultiple = false,
            category_id = FurnitureCategoryId,
            _values = new HashSet<string>
            {
                "Black",
                "Gray",
                "Natural",
                "White",
                "Walnut"
            }
        },
        new
        {
            Id = Guid.Parse("30200000-0000-0000-0000-000000000005"),
            Name = "Assembly Required",
            IsRequired = false,
            AllowMultiple = false,
            category_id = FurnitureCategoryId,
            _values = new HashSet<string>
            {
                "No",
                "Yes"
            }
        },
        new
        {
            Id = Guid.Parse("10400000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = TvHomeAudioCategoryId,
            _values = new HashSet<string>
            {
                "LG",
                "Samsung",
                "Sony",
                "TCL",
                "Vizio"
            }
        },
        new
        {
            Id = Guid.Parse("10400000-0000-0000-0000-000000000002"),
            Name = "Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = TvHomeAudioCategoryId,
            _values = new HashSet<string>
            {
                "4K UHD TV",
                "Soundbar",
                "AV Receiver",
                "Streaming Device"
            }
        },
        new
        {
            Id = Guid.Parse("10400000-0000-0000-0000-000000000003"),
            Name = "Smart Platform",
            IsRequired = false,
            AllowMultiple = false,
            category_id = TvHomeAudioCategoryId,
            _values = new HashSet<string>
            {
                "Google TV",
                "Roku",
                "Fire TV",
                "webOS",
                "Tizen"
            }
        },
        new
        {
            Id = Guid.Parse("10500000-0000-0000-0000-000000000001"),
            Name = "Platform",
            IsRequired = true,
            AllowMultiple = false,
            category_id = VideoGameConsolesCategoryId,
            _values = new HashSet<string>
            {
                "Nintendo",
                "PlayStation",
                "Xbox",
                "Steam Deck"
            }
        },
        new
        {
            Id = Guid.Parse("10500000-0000-0000-0000-000000000002"),
            Name = "Model",
            IsRequired = true,
            AllowMultiple = false,
            category_id = VideoGameConsolesCategoryId,
            _values = new HashSet<string>
            {
                "PlayStation 5",
                "Xbox Series X",
                "Nintendo Switch OLED",
                "Steam Deck OLED"
            }
        },
        new
        {
            Id = Guid.Parse("10500000-0000-0000-0000-000000000003"),
            Name = "Storage Capacity",
            IsRequired = false,
            AllowMultiple = false,
            category_id = VideoGameConsolesCategoryId,
            _values = new HashSet<string>
            {
                "64 GB",
                "512 GB",
                "1 TB",
                "2 TB"
            }
        },
        new
        {
            Id = Guid.Parse("10600000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WearableTechCategoryId,
            _values = new HashSet<string>
            {
                "Apple",
                "Fitbit",
                "Garmin",
                "Samsung",
                "Withings"
            }
        },
        new
        {
            Id = Guid.Parse("10600000-0000-0000-0000-000000000002"),
            Name = "Features",
            IsRequired = false,
            AllowMultiple = true,
            category_id = WearableTechCategoryId,
            _values = new HashSet<string>
            {
                "GPS",
                "Heart Rate Monitor",
                "NFC",
                "SpO2",
                "Sleep Tracking"
            }
        },
        new
        {
            Id = Guid.Parse("10700000-0000-0000-0000-000000000001"),
            Name = "Device Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = SmartHomeCategoryId,
            _values = new HashSet<string>
            {
                "Smart Speaker",
                "Smart Display",
                "Smart Lighting",
                "Smart Thermostat",
                "Security Camera"
            }
        },
        new
        {
            Id = Guid.Parse("10700000-0000-0000-0000-000000000002"),
            Name = "Ecosystem",
            IsRequired = false,
            AllowMultiple = false,
            category_id = SmartHomeCategoryId,
            _values = new HashSet<string>
            {
                "Alexa",
                "Apple Home",
                "Google Home",
                "Matter",
                "SmartThings"
            }
        },
        new
        {
            Id = Guid.Parse("10800000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = VehicleElectronicsCategoryId,
            _values = new HashSet<string>
            {
                "Dash Cam",
                "GPS",
                "Car Stereo",
                "Backup Camera",
                "Radar Detector"
            }
        },
        new
        {
            Id = Guid.Parse("10800000-0000-0000-0000-000000000002"),
            Name = "Compatible Vehicle",
            IsRequired = false,
            AllowMultiple = true,
            category_id = VehicleElectronicsCategoryId,
            _values = new HashSet<string>
            {
                "Universal",
                "Ford",
                "GM",
                "Toyota",
                "Volkswagen"
            }
        },
        new
        {
            Id = Guid.Parse("20300000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MensClothingCategoryId,
            _values = new HashSet<string>
            {
                "Banana Republic",
                "Hugo Boss",
                "Levi's",
                "Nike",
                "Ralph Lauren"
            }
        },
        new
        {
            Id = Guid.Parse("20300000-0000-0000-0000-000000000002"),
            Name = "Size",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MensClothingCategoryId,
            _values = new HashSet<string>
            {
                "S",
                "M",
                "L",
                "XL",
                "XXL"
            }
        },
        new
        {
            Id = Guid.Parse("20400000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WomensHandbagsCategoryId,
            _values = new HashSet<string>
            {
                "Coach",
                "Gucci",
                "Kate Spade",
                "Louis Vuitton",
                "Tory Burch"
            }
        },
        new
        {
            Id = Guid.Parse("20400000-0000-0000-0000-000000000002"),
            Name = "Materials",
            IsRequired = false,
            AllowMultiple = true,
            category_id = WomensHandbagsCategoryId,
            _values = new HashSet<string>
            {
                "Canvas",
                "Leather",
                "Nylon",
                "Patent Leather",
                "Vegan Leather"
            }
        },
        new
        {
            Id = Guid.Parse("20400000-0000-0000-0000-000000000003"),
            Name = "Style",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WomensHandbagsCategoryId,
            _values = new HashSet<string>
            {
                "Backpack",
                "Crossbody",
                "Satchel",
                "Shoulder Bag",
                "Tote"
            }
        },
        new
        {
            Id = Guid.Parse("20500000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WomensShoesCategoryId,
            _values = new HashSet<string>
            {
                "Birkenstock",
                "Clarks",
                "Dr. Martens",
                "Sam Edelman",
                "Steve Madden"
            }
        },
        new
        {
            Id = Guid.Parse("20500000-0000-0000-0000-000000000002"),
            Name = "US Shoe Size",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WomensShoesCategoryId,
            _values = new HashSet<string>
            {
                "5",
                "6",
                "7",
                "8",
                "9",
                "10"
            }
        },
        new
        {
            Id = Guid.Parse("20500000-0000-0000-0000-000000000003"),
            Name = "Style",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WomensShoesCategoryId,
            _values = new HashSet<string>
            {
                "Boots",
                "Flats",
                "Heels",
                "Sandals",
                "Sneakers"
            }
        },
        new
        {
            Id = Guid.Parse("20600000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WatchesCategoryId,
            _values = new HashSet<string>
            {
                "Casio",
                "Citizen",
                "Omega",
                "Rolex",
                "Seiko"
            }
        },
        new
        {
            Id = Guid.Parse("20600000-0000-0000-0000-000000000002"),
            Name = "Movement",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WatchesCategoryId,
            _values = new HashSet<string>
            {
                "Automatic",
                "Quartz",
                "Mechanical",
                "Solar"
            }
        },
        new
        {
            Id = Guid.Parse("20700000-0000-0000-0000-000000000001"),
            Name = "Metal",
            IsRequired = true,
            AllowMultiple = false,
            category_id = JewelryCategoryId,
            _values = new HashSet<string>
            {
                "Gold",
                "Platinum",
                "Rose Gold",
                "Sterling Silver",
                "White Gold"
            }
        },
        new
        {
            Id = Guid.Parse("20700000-0000-0000-0000-000000000002"),
            Name = "Jewelry Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = JewelryCategoryId,
            _values = new HashSet<string>
            {
                "Bracelet",
                "Earrings",
                "Necklace",
                "Ring"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000006"),
            Name = "Style",
            IsRequired = false,
            AllowMultiple = false,
            category_id = HomeDecorCategoryId,
            _values = new HashSet<string>
            {
                "Bohemian",
                "Farmhouse",
                "Mid-Century",
                "Modern",
                "Traditional"
            }
        },
        new
        {
            Id = Guid.Parse("30100000-0000-0000-0000-000000000007"),
            Name = "Room",
            IsRequired = true,
            AllowMultiple = false,
            category_id = HomeDecorCategoryId,
            _values = new HashSet<string>
            {
                "Bedroom",
                "Dining Room",
                "Kitchen",
                "Living Room",
                "Office"
            }
        },
        new
        {
            Id = Guid.Parse("30300000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = false,
            AllowMultiple = false,
            category_id = ToolsWorkshopCategoryId,
            _values = new HashSet<string>
            {
                "Bosch",
                "DeWalt",
                "Hilti",
                "Makita",
                "Milwaukee"
            }
        },
        new
        {
            Id = Guid.Parse("30300000-0000-0000-0000-000000000002"),
            Name = "Power Source",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ToolsWorkshopCategoryId,
            _values = new HashSet<string>
            {
                "Battery",
                "Corded Electric",
                "Compressed Air",
                "Manual"
            }
        },
        new
        {
            Id = Guid.Parse("30400000-0000-0000-0000-000000000001"),
            Name = "Category",
            IsRequired = true,
            AllowMultiple = false,
            category_id = HomeImprovementCategoryId,
            _values = new HashSet<string>
            {
                "Flooring",
                "Hardware",
                "Lighting",
                "Plumbing",
                "Storage"
            }
        },
        new
        {
            Id = Guid.Parse("30400000-0000-0000-0000-000000000002"),
            Name = "Finish",
            IsRequired = false,
            AllowMultiple = false,
            category_id = HomeImprovementCategoryId,
            _values = new HashSet<string>
            {
                "Brushed Nickel",
                "Chrome",
                "Matte Black",
                "Oil-Rubbed Bronze"
            }
        },
        new
        {
            Id = Guid.Parse("40100000-0000-0000-0000-000000000001"),
            Name = "Part Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CarPartsAccessoriesCategoryId,
            _values = new HashSet<string>
            {
                "Brakes",
                "Engine",
                "Exterior",
                "Interior",
                "Suspension"
            }
        },
        new
        {
            Id = Guid.Parse("40100000-0000-0000-0000-000000000002"),
            Name = "Brand",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CarPartsAccessoriesCategoryId,
            _values = new HashSet<string>
            {
                "ACDelco",
                "Bosch",
                "Denso",
                "Mopar",
                "Motorcraft"
            }
        },
        new
        {
            Id = Guid.Parse("40100000-0000-0000-0000-000000000003"),
            Name = "Compatible Make",
            IsRequired = false,
            AllowMultiple = true,
            category_id = CarPartsAccessoriesCategoryId,
            _values = new HashSet<string>
            {
                "Chevrolet",
                "Ford",
                "Honda",
                "Toyota",
                "Universal"
            }
        },
        new
        {
            Id = Guid.Parse("40200000-0000-0000-0000-000000000001"),
            Name = "Part Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MotorcyclePartsCategoryId,
            _values = new HashSet<string>
            {
                "Body & Frame",
                "Drivetrain",
                "Electrical",
                "Engine",
                "Suspension"
            }
        },
        new
        {
            Id = Guid.Parse("40300000-0000-0000-0000-000000000001"),
            Name = "Tool Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = AutomotiveToolsCategoryId,
            _values = new HashSet<string>
            {
                "Diagnostic",
                "Hand Tool",
                "Lifts & Jacks",
                "Power Tool",
                "Specialty"
            }
        },
        new
        {
            Id = Guid.Parse("40400000-0000-0000-0000-000000000001"),
            Name = "Tire Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = WheelsTiresCategoryId,
            _values = new HashSet<string>
            {
                "All-Season",
                "Performance",
                "Snow/Winter",
                "Off-Road"
            }
        },
        new
        {
            Id = Guid.Parse("40400000-0000-0000-0000-000000000002"),
            Name = "Rim Diameter",
            IsRequired = false,
            AllowMultiple = false,
            category_id = WheelsTiresCategoryId,
            _values = new HashSet<string>
            {
                "16 in",
                "17 in",
                "18 in",
                "19 in",
                "20 in"
            }
        },
        new
        {
            Id = Guid.Parse("60100000-0000-0000-0000-000000000001"),
            Name = "Franchise",
            IsRequired = true,
            AllowMultiple = false,
            category_id = TradingCardsCategoryId,
            _values = new HashSet<string>
            {
                "Magic: The Gathering",
                "Pokémon",
                "Yu-Gi-Oh!",
                "Marvel Snap",
                "Disney Lorcana"
            }
        },
        new
        {
            Id = Guid.Parse("60100000-0000-0000-0000-000000000002"),
            Name = "Card Condition",
            IsRequired = true,
            AllowMultiple = false,
            category_id = TradingCardsCategoryId,
            _values = new HashSet<string>
            {
                "Gem Mint",
                "Near Mint",
                "Lightly Played",
                "Moderately Played",
                "Heavily Played"
            }
        },
        new
        {
            Id = Guid.Parse("60100000-0000-0000-0000-000000000003"),
            Name = "Graded",
            IsRequired = false,
            AllowMultiple = false,
            category_id = TradingCardsCategoryId,
            _values = new HashSet<string>
            {
                "BGS",
                "CGC",
                "PSA",
                "SGC",
                "Ungraded"
            }
        },
        new
        {
            Id = Guid.Parse("60200000-0000-0000-0000-000000000001"),
            Name = "Publisher",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ComicsCategoryId,
            _values = new HashSet<string>
            {
                "DC",
                "Dark Horse",
                "IDW",
                "Image",
                "Marvel"
            }
        },
        new
        {
            Id = Guid.Parse("60200000-0000-0000-0000-000000000002"),
            Name = "Era",
            IsRequired = false,
            AllowMultiple = false,
            category_id = ComicsCategoryId,
            _values = new HashSet<string>
            {
                "Golden Age",
                "Silver Age",
                "Bronze Age",
                "Modern"
            }
        },
        new
        {
            Id = Guid.Parse("60300000-0000-0000-0000-000000000001"),
            Name = "Artist",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ArtPrintsCategoryId,
            _values = new HashSet<string>
            {
                "Andy Warhol",
                "Banksy",
                "Jean-Michel Basquiat",
                "Salvador Dalí",
                "Yoshitomo Nara"
            }
        },
        new
        {
            Id = Guid.Parse("60300000-0000-0000-0000-000000000002"),
            Name = "Medium",
            IsRequired = false,
            AllowMultiple = false,
            category_id = ArtPrintsCategoryId,
            _values = new HashSet<string>
            {
                "Giclée",
                "Lithograph",
                "Screenprint",
                "Serigraph"
            }
        },
        new
        {
            Id = Guid.Parse("60400000-0000-0000-0000-000000000001"),
            Name = "Certification",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CoinsPaperMoneyCategoryId,
            _values = new HashSet<string>
            {
                "ANACS",
                "NGC",
                "PCGS",
                "PMG",
                "Uncertified"
            }
        },
        new
        {
            Id = Guid.Parse("60400000-0000-0000-0000-000000000002"),
            Name = "Grade",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CoinsPaperMoneyCategoryId,
            _values = new HashSet<string>
            {
                "MS 70",
                "MS 69",
                "MS 65",
                "AU 55",
                "XF 45"
            }
        },
        new
        {
            Id = Guid.Parse("70100000-0000-0000-0000-000000000001"),
            Name = "Franchise",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ActionFiguresCategoryId,
            _values = new HashSet<string>
            {
                "DC",
                "Dragon Ball",
                "Marvel",
                "Star Wars",
                "Transformers"
            }
        },
        new
        {
            Id = Guid.Parse("70100000-0000-0000-0000-000000000002"),
            Name = "Scale",
            IsRequired = false,
            AllowMultiple = false,
            category_id = ActionFiguresCategoryId,
            _values = new HashSet<string>
            {
                "1:6",
                "1:12",
                "1:18",
                "6 in",
                "12 in"
            }
        },
        new
        {
            Id = Guid.Parse("70200000-0000-0000-0000-000000000001"),
            Name = "Scale",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ModelRailroadsCategoryId,
            _values = new HashSet<string>
            {
                "HO",
                "N",
                "O",
                "G",
                "Z"
            }
        },
        new
        {
            Id = Guid.Parse("70200000-0000-0000-0000-000000000002"),
            Name = "Power Type",
            IsRequired = false,
            AllowMultiple = false,
            category_id = ModelRailroadsCategoryId,
            _values = new HashSet<string>
            {
                "DC",
                "DCC",
                "Battery"
            }
        },
        new
        {
            Id = Guid.Parse("70300000-0000-0000-0000-000000000001"),
            Name = "Vehicle Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = RcVehiclesCategoryId,
            _values = new HashSet<string>
            {
                "Car",
                "Truck",
                "Boat",
                "Plane",
                "Drone"
            }
        },
        new
        {
            Id = Guid.Parse("70300000-0000-0000-0000-000000000002"),
            Name = "Power",
            IsRequired = false,
            AllowMultiple = false,
            category_id = RcVehiclesCategoryId,
            _values = new HashSet<string>
            {
                "Electric",
                "Gas",
                "Nitro"
            }
        },
        new
        {
            Id = Guid.Parse("70400000-0000-0000-0000-000000000001"),
            Name = "Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = DollsBearsCategoryId,
            _values = new HashSet<string>
            {
                "Barbie",
                "Fashion Doll",
                "Teddy Bear",
                "Vintage Doll"
            }
        },
        new
        {
            Id = Guid.Parse("70500000-0000-0000-0000-000000000001"),
            Name = "Theme",
            IsRequired = false,
            AllowMultiple = false,
            category_id = LegoBuildingToysCategoryId,
            _values = new HashSet<string>
            {
                "Architecture",
                "City",
                "Ideas",
                "Star Wars",
                "Technic"
            }
        },
        new
        {
            Id = Guid.Parse("80100000-0000-0000-0000-000000000001"),
            Name = "Sport",
            IsRequired = true,
            AllowMultiple = false,
            category_id = OutdoorSportsCategoryId,
            _values = new HashSet<string>
            {
                "Camping",
                "Climbing",
                "Fishing",
                "Hunting",
                "Water Sports"
            }
        },
        new
        {
            Id = Guid.Parse("80200000-0000-0000-0000-000000000001"),
            Name = "Equipment Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FitnessRunningYogaCategoryId,
            _values = new HashSet<string>
            {
                "Cardio Machine",
                "Free Weights",
                "Resistance Bands",
                "Yoga Mat"
            }
        },
        new
        {
            Id = Guid.Parse("80300000-0000-0000-0000-000000000001"),
            Name = "Bicycle Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CyclingCategoryId,
            _values = new HashSet<string>
            {
                "Mountain",
                "Road",
                "Hybrid",
                "Gravel",
                "Electric"
            }
        },
        new
        {
            Id = Guid.Parse("80300000-0000-0000-0000-000000000002"),
            Name = "Frame Size",
            IsRequired = false,
            AllowMultiple = false,
            category_id = CyclingCategoryId,
            _values = new HashSet<string>
            {
                "Small",
                "Medium",
                "Large",
                "X-Large"
            }
        },
        new
        {
            Id = Guid.Parse("80400000-0000-0000-0000-000000000001"),
            Name = "Club Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = GolfCategoryId,
            _values = new HashSet<string>
            {
                "Driver",
                "Fairway Wood",
                "Hybrid",
                "Iron Set",
                "Putter"
            }
        },
        new
        {
            Id = Guid.Parse("80400000-0000-0000-0000-000000000002"),
            Name = "Flex",
            IsRequired = false,
            AllowMultiple = false,
            category_id = GolfCategoryId,
            _values = new HashSet<string>
            {
                "Ladies",
                "Regular",
                "Stiff",
                "Extra Stiff"
            }
        },
        new
        {
            Id = Guid.Parse("90100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MakeupCategoryId,
            _values = new HashSet<string>
            {
                "Charlotte Tilbury",
                "Dior",
                "Fenty Beauty",
                "MAC",
                "Rare Beauty"
            }
        },
        new
        {
            Id = Guid.Parse("90100000-0000-0000-0000-000000000002"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MakeupCategoryId,
            _values = new HashSet<string>
            {
                "Foundation",
                "Eyeshadow",
                "Lipstick",
                "Mascara",
                "Primer"
            }
        },
        new
        {
            Id = Guid.Parse("90100000-0000-0000-0000-000000000003"),
            Name = "Shade",
            IsRequired = false,
            AllowMultiple = false,
            category_id = MakeupCategoryId,
            _values = new HashSet<string>
            {
                "Fair",
                "Light",
                "Medium",
                "Tan",
                "Deep"
            }
        },
        new
        {
            Id = Guid.Parse("90200000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = SkinCareCategoryId,
            _values = new HashSet<string>
            {
                "CeraVe",
                "Dermalogica",
                "Drunk Elephant",
                "La Roche-Posay",
                "Tatcha"
            }
        },
        new
        {
            Id = Guid.Parse("90200000-0000-0000-0000-000000000002"),
            Name = "Skin Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = SkinCareCategoryId,
            _values = new HashSet<string>
            {
                "Dry",
                "Normal",
                "Oily",
                "Combination",
                "Sensitive"
            }
        },
        new
        {
            Id = Guid.Parse("90200000-0000-0000-0000-000000000003"),
            Name = "Concern",
            IsRequired = false,
            AllowMultiple = true,
            category_id = SkinCareCategoryId,
            _values = new HashSet<string>
            {
                "Acne",
                "Anti-Aging",
                "Brightening",
                "Hydration",
                "Sun Protection"
            }
        },
        new
        {
            Id = Guid.Parse("90300000-0000-0000-0000-000000000001"),
            Name = "Formulation",
            IsRequired = true,
            AllowMultiple = false,
            category_id = VitaminsSupplementsCategoryId,
            _values = new HashSet<string>
            {
                "Capsule",
                "Gummy",
                "Powder",
                "Softgel",
                "Tablet"
            }
        },
        new
        {
            Id = Guid.Parse("90300000-0000-0000-0000-000000000002"),
            Name = "Main Purpose",
            IsRequired = false,
            AllowMultiple = true,
            category_id = VitaminsSupplementsCategoryId,
            _values = new HashSet<string>
            {
                "Energy",
                "General Wellness",
                "Immune Support",
                "Joint Health",
                "Sleep"
            }
        },
        new
        {
            Id = Guid.Parse("90400000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = HairCareCategoryId,
            _values = new HashSet<string>
            {
                "Amika",
                "Dyson",
                "GHD",
                "Olaplex",
                "Redken"
            }
        },
        new
        {
            Id = Guid.Parse("90400000-0000-0000-0000-000000000002"),
            Name = "Hair Type",
            IsRequired = false,
            AllowMultiple = true,
            category_id = HairCareCategoryId,
            _values = new HashSet<string>
            {
                "Coily",
                "Curly",
                "Straight",
                "Wavy",
                "Fine"
            }
        },
        new
        {
            Id = Guid.Parse("90500000-0000-0000-0000-000000000001"),
            Name = "Fragrance Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FragrancesCategoryId,
            _values = new HashSet<string>
            {
                "Eau de Parfum",
                "Eau de Toilette",
                "Eau de Cologne",
                "Perfume Oil"
            }
        },
        new
        {
            Id = Guid.Parse("90500000-0000-0000-0000-000000000002"),
            Name = "Volume",
            IsRequired = false,
            AllowMultiple = false,
            category_id = FragrancesCategoryId,
            _values = new HashSet<string>
            {
                "30 ml",
                "50 ml",
                "75 ml",
                "100 ml"
            }
        },
        new
        {
            Id = Guid.Parse("A0100000-0000-0000-0000-000000000001"),
            Name = "Equipment Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = HeavyEquipmentCategoryId,
            _values = new HashSet<string>
            {
                "Backhoe",
                "Bulldozer",
                "Excavator",
                "Forklift",
                "Skid Steer"
            }
        },
        new
        {
            Id = Guid.Parse("A0100000-0000-0000-0000-000000000002"),
            Name = "Hours",
            IsRequired = false,
            AllowMultiple = false,
            category_id = HeavyEquipmentCategoryId,
            _values = new HashSet<string>
            {
                "0-500",
                "501-1500",
                "1501-3000",
                "3001+"
            }
        },
        new
        {
            Id = Guid.Parse("A0200000-0000-0000-0000-000000000001"),
            Name = "Supply Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = MroIndustrialSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Fasteners",
                "Hydraulics",
                "HVAC",
                "Pneumatics",
                "Safety"
            }
        },
        new
        {
            Id = Guid.Parse("A0300000-0000-0000-0000-000000000001"),
            Name = "Service Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = RetailServicesCategoryId,
            _values = new HashSet<string>
            {
                "Consulting",
                "Installation",
                "Maintenance",
                "Marketing",
                "Training"
            }
        },
        new
        {
            Id = Guid.Parse("A0400000-0000-0000-0000-000000000001"),
            Name = "Equipment Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = OfficeEquipmentCategoryId,
            _values = new HashSet<string>
            {
                "3D Printer",
                "Laser Printer",
                "Multifunction",
                "Scanner",
                "Shredder"
            }
        },
        new
        {
            Id = Guid.Parse("B0100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = GuitarsBassesCategoryId,
            _values = new HashSet<string>
            {
                "Fender",
                "Gibson",
                "Ibanez",
                "PRS",
                "Taylor"
            }
        },
        new
        {
            Id = Guid.Parse("B0100000-0000-0000-0000-000000000002"),
            Name = "Body Type",
            IsRequired = false,
            AllowMultiple = false,
            category_id = GuitarsBassesCategoryId,
            _values = new HashSet<string>
            {
                "Solid",
                "Semi-Hollow",
                "Hollow"
            }
        },
        new
        {
            Id = Guid.Parse("B0200000-0000-0000-0000-000000000001"),
            Name = "Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ProAudioEquipmentCategoryId,
            _values = new HashSet<string>
            {
                "Mixer",
                "Microphone",
                "Monitor",
                "Interface",
                "Outboard Gear"
            }
        },
        new
        {
            Id = Guid.Parse("B0300000-0000-0000-0000-000000000001"),
            Name = "Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = DjEquipmentCategoryId,
            _values = new HashSet<string>
            {
                "Controller",
                "Mixer",
                "Turntable",
                "Lighting"
            }
        },
        new
        {
            Id = Guid.Parse("B0400000-0000-0000-0000-000000000001"),
            Name = "Instrument",
            IsRequired = true,
            AllowMultiple = false,
            category_id = BrassWoodwindCategoryId,
            _values = new HashSet<string>
            {
                "Clarinet",
                "Flute",
                "Saxophone",
                "Trombone",
                "Trumpet"
            }
        },
        new
        {
            Id = Guid.Parse("C0100000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = DogSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Apparel",
                "Crates",
                "Food",
                "Grooming",
                "Toys"
            }
        },
        new
        {
            Id = Guid.Parse("C0100000-0000-0000-0000-000000000002"),
            Name = "Size",
            IsRequired = false,
            AllowMultiple = false,
            category_id = DogSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "XS",
                "S",
                "M",
                "L",
                "XL"
            }
        },
        new
        {
            Id = Guid.Parse("C0200000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = CatSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Food",
                "Litter",
                "Scratchers",
                "Toys"
            }
        },
        new
        {
            Id = Guid.Parse("C0300000-0000-0000-0000-000000000001"),
            Name = "Aquarium Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FishAquariumCategoryId,
            _values = new HashSet<string>
            {
                "Freshwater",
                "Marine",
                "Reef",
                "Brackish"
            }
        },
        new
        {
            Id = Guid.Parse("C0300000-0000-0000-0000-000000000002"),
            Name = "Tank Capacity",
            IsRequired = false,
            AllowMultiple = false,
            category_id = FishAquariumCategoryId,
            _values = new HashSet<string>
            {
                "1-10 gal",
                "11-30 gal",
                "31-55 gal",
                "56+ gal"
            }
        },
        new
        {
            Id = Guid.Parse("C0400000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = SmallAnimalSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Bedding",
                "Cages",
                "Food",
                "Toys"
            }
        },
        new
        {
            Id = Guid.Parse("D0100000-0000-0000-0000-000000000001"),
            Name = "Brand",
            IsRequired = true,
            AllowMultiple = false,
            category_id = StrollersCategoryId,
            _values = new HashSet<string>
            {
                "Baby Jogger",
                "Bugaboo",
                "Chicco",
                "Graco",
                "UPPAbaby"
            }
        },
        new
        {
            Id = Guid.Parse("D0100000-0000-0000-0000-000000000002"),
            Name = "Stroller Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = StrollersCategoryId,
            _values = new HashSet<string>
            {
                "Full-Size",
                "Jogging",
                "Travel System",
                "Umbrella"
            }
        },
        new
        {
            Id = Guid.Parse("D0200000-0000-0000-0000-000000000001"),
            Name = "Furniture Piece",
            IsRequired = true,
            AllowMultiple = false,
            category_id = NurseryFurnitureCategoryId,
            _values = new HashSet<string>
            {
                "Crib",
                "Changing Table",
                "Glider",
                "Dresser"
            }
        },
        new
        {
            Id = Guid.Parse("D0300000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = BabySafetyCategoryId,
            _values = new HashSet<string>
            {
                "Baby Monitor",
                "Safety Gate",
                "Outlet Cover",
                "Cabinet Lock"
            }
        },
        new
        {
            Id = Guid.Parse("D0400000-0000-0000-0000-000000000001"),
            Name = "Feeding Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = BabyFeedingCategoryId,
            _values = new HashSet<string>
            {
                "Bottle",
                "Breastfeeding",
                "Solid Food",
                "Toddler"
            }
        },
        new
        {
            Id = Guid.Parse("E0100000-0000-0000-0000-000000000001"),
            Name = "Product Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ScrapbookingSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Adhesives",
                "Dies",
                "Paper",
                "Stamps"
            }
        },
        new
        {
            Id = Guid.Parse("E0200000-0000-0000-0000-000000000001"),
            Name = "Medium",
            IsRequired = true,
            AllowMultiple = false,
            category_id = ArtSuppliesCategoryId,
            _values = new HashSet<string>
            {
                "Acrylic",
                "Oil",
                "Watercolor",
                "Pastel"
            }
        },
        new
        {
            Id = Guid.Parse("E0300000-0000-0000-0000-000000000001"),
            Name = "Fabric Type",
            IsRequired = true,
            AllowMultiple = false,
            category_id = FabricCategoryId,
            _values = new HashSet<string>
            {
                "Cotton",
                "Denim",
                "Fleece",
                "Linen",
                "Silk"
            }
        },
        new
        {
            Id = Guid.Parse("E0400000-0000-0000-0000-000000000001"),
            Name = "Material",
            IsRequired = true,
            AllowMultiple = false,
            category_id = BeadsJewelryMakingCategoryId,
            _values = new HashSet<string>
            {
                "Glass",
                "Gemstone",
                "Metal",
                "Resin",
                "Wood"
            }
        }
    };

    internal static readonly IReadOnlyList<object> CategoryConditions = new object[]
    {
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.ManufacturerRefurbishedConditionId },
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.SellerRefurbishedConditionId },
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = CellPhonesCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.ManufacturerRefurbishedConditionId },
        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.SellerRefurbishedConditionId },
        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = LaptopsCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = CamerasCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CamerasCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = CamerasCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = CamerasCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = MensAthleticShoesCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = MensAthleticShoesCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = MensAthleticShoesCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = WomensDressesCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = WomensDressesCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = WomensDressesCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = KitchenAppliancesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = KitchenAppliancesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = KitchenAppliancesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = FurnitureCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = FurnitureCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = FurnitureCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = TvHomeAudioCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = TvHomeAudioCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = TvHomeAudioCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = VideoGameConsolesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = VideoGameConsolesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = VideoGameConsolesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = VideoGameConsolesCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = WearableTechCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = WearableTechCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = WearableTechCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = SmartHomeCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = SmartHomeCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = SmartHomeCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = VehicleElectronicsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = VehicleElectronicsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = VehicleElectronicsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = MensClothingCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = MensClothingCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = MensClothingCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = WomensHandbagsCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = WomensHandbagsCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = WomensHandbagsCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = WomensShoesCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = WomensShoesCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = WomensShoesCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = WatchesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = WatchesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = WatchesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = JewelryCategoryId, ConditionId = ConditionSeedData.NewWithTagsConditionId },
        new { CategoryId = JewelryCategoryId, ConditionId = ConditionSeedData.NewWithoutTagsConditionId },
        new { CategoryId = JewelryCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = HomeDecorCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = HomeDecorCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = HomeDecorCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = ToolsWorkshopCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ToolsWorkshopCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = ToolsWorkshopCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = YardGardenCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = YardGardenCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = YardGardenCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = HomeImprovementCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = HomeImprovementCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = HomeImprovementCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.ManufacturerRefurbishedConditionId },
        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.SellerRefurbishedConditionId },
        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = CarPartsAccessoriesCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = MotorcyclePartsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = MotorcyclePartsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = MotorcyclePartsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = MotorcyclePartsCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = AutomotiveToolsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = AutomotiveToolsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = AutomotiveToolsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = WheelsTiresCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = WheelsTiresCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = WheelsTiresCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = WheelsTiresCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = TradingCardsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = TradingCardsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = TradingCardsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = ComicsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ComicsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = ArtPrintsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ArtPrintsCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = CoinsPaperMoneyCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CoinsPaperMoneyCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = ActionFiguresCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ActionFiguresCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = ActionFiguresCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = ModelRailroadsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ModelRailroadsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = ModelRailroadsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = RcVehiclesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = RcVehiclesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = RcVehiclesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },
        new { CategoryId = RcVehiclesCategoryId, ConditionId = ConditionSeedData.ForPartsConditionId },

        new { CategoryId = DollsBearsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = DollsBearsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = DollsBearsCategoryId, ConditionId = ConditionSeedData.PreOwnedConditionId },

        new { CategoryId = LegoBuildingToysCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = LegoBuildingToysCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = LegoBuildingToysCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = OutdoorSportsCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = OutdoorSportsCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = OutdoorSportsCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = FitnessRunningYogaCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = FitnessRunningYogaCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = FitnessRunningYogaCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = CyclingCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CyclingCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = CyclingCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = GolfCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = GolfCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = GolfCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = MakeupCategoryId, ConditionId = ConditionSeedData.NewConditionId },

        new { CategoryId = SkinCareCategoryId, ConditionId = ConditionSeedData.NewConditionId },

        new { CategoryId = VitaminsSupplementsCategoryId, ConditionId = ConditionSeedData.NewConditionId },

        new { CategoryId = HairCareCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = HairCareCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },

        new { CategoryId = FragrancesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = FragrancesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = FragrancesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = HeavyEquipmentCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = HeavyEquipmentCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = HeavyEquipmentCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = MroIndustrialSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = MroIndustrialSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = MroIndustrialSuppliesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = RetailServicesCategoryId, ConditionId = ConditionSeedData.NewConditionId },

        new { CategoryId = OfficeEquipmentCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = OfficeEquipmentCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = OfficeEquipmentCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = GuitarsBassesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = GuitarsBassesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = GuitarsBassesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = ProAudioEquipmentCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ProAudioEquipmentCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = ProAudioEquipmentCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = DjEquipmentCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = DjEquipmentCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = DjEquipmentCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = BrassWoodwindCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = BrassWoodwindCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = BrassWoodwindCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = DogSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = DogSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = DogSuppliesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = CatSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = CatSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = CatSuppliesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = FishAquariumCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = FishAquariumCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },

        new { CategoryId = SmallAnimalSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = SmallAnimalSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = SmallAnimalSuppliesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = StrollersCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = StrollersCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = StrollersCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = NurseryFurnitureCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = NurseryFurnitureCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = NurseryFurnitureCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = BabySafetyCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = BabySafetyCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },

        new { CategoryId = BabyFeedingCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = BabyFeedingCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },

        new { CategoryId = ScrapbookingSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ScrapbookingSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },

        new { CategoryId = ArtSuppliesCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = ArtSuppliesCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId },
        new { CategoryId = ArtSuppliesCategoryId, ConditionId = ConditionSeedData.UsedConditionId },

        new { CategoryId = FabricCategoryId, ConditionId = ConditionSeedData.NewConditionId },

        new { CategoryId = BeadsJewelryMakingCategoryId, ConditionId = ConditionSeedData.NewConditionId },
        new { CategoryId = BeadsJewelryMakingCategoryId, ConditionId = ConditionSeedData.OpenBoxConditionId }
    };
}

internal static class ConditionSeedData
{
    internal static readonly Guid NewConditionId = Guid.Parse("40000000-0000-0000-0000-000000000001");
    internal static readonly Guid ManufacturerRefurbishedConditionId = Guid.Parse("40000000-0000-0000-0000-000000000002");
    internal static readonly Guid SellerRefurbishedConditionId = Guid.Parse("40000000-0000-0000-0000-000000000003");
    internal static readonly Guid UsedConditionId = Guid.Parse("40000000-0000-0000-0000-000000000004");
    internal static readonly Guid ForPartsConditionId = Guid.Parse("40000000-0000-0000-0000-000000000005");
    internal static readonly Guid OpenBoxConditionId = Guid.Parse("40000000-0000-0000-0000-000000000006");
    internal static readonly Guid NewWithTagsConditionId = Guid.Parse("40000000-0000-0000-0000-000000000007");
    internal static readonly Guid NewWithoutTagsConditionId = Guid.Parse("40000000-0000-0000-0000-000000000008");
    internal static readonly Guid PreOwnedConditionId = Guid.Parse("40000000-0000-0000-0000-000000000009");

    internal static readonly IReadOnlyList<object> Conditions = new object[]
    {
        new
        {
            Id = NewConditionId,
            Name = "New",
            Description = "Factory sealed, unused item in original packaging.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ManufacturerRefurbishedConditionId,
            Name = "Manufacturer Refurbished",
            Description = "Professionally restored to working order by the manufacturer or certified provider.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = SellerRefurbishedConditionId,
            Name = "Seller Refurbished",
            Description = "Restored to working order by a third-party seller.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = UsedConditionId,
            Name = "Used",
            Description = "Previously owned item that shows signs of use.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = ForPartsConditionId,
            Name = "For parts or not working",
            Description = "Item does not function as intended and is being sold for parts or repair.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = OpenBoxConditionId,
            Name = "Open box",
            Description = "Item is unused but the original packaging has been opened.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = NewWithTagsConditionId,
            Name = "New with tags",
            Description = "Unused apparel item with original tags attached.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = NewWithoutTagsConditionId,
            Name = "New without tags",
            Description = "Unused apparel item missing the retail tags.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        },
        new
        {
            Id = PreOwnedConditionId,
            Name = "Pre-owned",
            Description = "Previously worn item that remains in good condition.",
            CreatedAt = SeedDefaults.Timestamp,
            CreatedBy = (string?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedBy = (string?)null,
            IsDeleted = false
        }
    };
}

internal static class SeedDefaults
{
    internal static readonly DateTime Timestamp = new(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc);
}
