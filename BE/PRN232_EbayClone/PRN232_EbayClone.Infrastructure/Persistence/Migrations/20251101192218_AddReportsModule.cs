using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReportsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "report_downloads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    requested_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    file_url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    range_start_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    range_end_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    range_timezone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_downloads", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "report_schedules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    frequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_run_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    next_run_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    delivery_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_report_schedules", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_at", "created_by", "description", "is_deleted", "name", "parent_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Televisions, speakers, and streaming devices.", false, "TV, Video & Home Audio", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Home and handheld gaming systems.", false, "Video Game Consoles", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Smartwatches, fitness trackers, and smart eyewear.", false, "Wearable Technology", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Connected home devices and automation hubs.", false, "Smart Home", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Navigation, dash cams, and in-car entertainment.", false, "Vehicle Electronics & GPS", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Casual, business, and formal apparel for men.", false, "Men's Clothing", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Designer totes, crossbody bags, and backpacks.", false, "Women's Handbags & Bags", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Heels, flats, and casual footwear.", false, "Women's Shoes", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Timepieces ranging from vintage to luxury.", false, "Watches", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rings, necklaces, and bracelets crafted in precious metals.", false, "Fine Jewelry", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Interior accents, wall art, and lighting.", false, "Home Décor", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Power tools and shop essentials.", false, "Tools & Workshop Equipment", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Outdoor décor, landscaping, and patio gear.", false, "Yard, Garden & Outdoor Living", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Building supplies, fixtures, and hardware.", false, "Home Improvement", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Complete automotive marketplace for vehicles and parts.", false, "eBay Motors", null, null, null },
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Treasures from pop culture, history, and fine art.", false, "Collectibles & Art", null, null, null },
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Playsets, model kits, and collector favorites.", false, "Toys & Hobbies", null, null, null },
                    { new Guid("80000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gear for every sport, indoors and out.", false, "Sporting Goods", null, null, null },
                    { new Guid("90000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Wellness essentials and personal care favorites.", false, "Health & Beauty", null, null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Equipment, supplies, and services for every trade.", false, "Business & Industrial", null, null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Instruments, pro audio, and stage equipment.", false, "Musical Instruments & Gear", null, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Care essentials for pets large and small.", false, "Pet Supplies", null, null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nursery gear, travel systems, and feeding must-haves.", false, "Baby Essentials", null, null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "DIY staples spanning every creative discipline.", false, "Crafts", null, null, null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "OEM and aftermarket components for every ride.", false, "Car Parts & Accessories", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Upgrades and replacement parts for bikes.", false, "Motorcycle Parts", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Garage lifts, diagnostics, and specialty tools.", false, "Automotive Tools & Supplies", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rims, tire sets, TPMS sensors, and more.", false, "Wheels, Tires & Parts", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "TCG singles, sealed product, and memorabilia.", false, "Collectible Card Games", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Golden Age through modern runs and collectibles.", false, "Comics & Graphic Novels", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Limited editions, lithographs, and posters.", false, "Art Prints", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Graded coins, bullion, and currency.", false, "Coins & Paper Money", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Superheroes, anime, and pop-culture icons.", false, "Action Figures", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Locomotives, rolling stock, and scenery kits.", false, "Model Railroads & Trains", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Radio-controlled cars, drones, and planes.", false, "RC Model Vehicles & Kits", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Barbie, Blythe, Build-A-Bear, and more.", false, "Dolls & Bears", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modular builds and sealed collectible sets.", false, "LEGO & Building Toys", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Camping, hiking, hunting, and fishing gear.", false, "Outdoor Sports", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Exercise machines, apparel, and accessories.", false, "Fitness, Running & Yoga", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bikes, parts, helmets, and apparel.", false, "Cycling", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clubs, balls, carts, and training aids.", false, "Golf", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cosmetics, palettes, and tools.", false, "Makeup", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Serums, moisturizers, and devices.", false, "Skin Care", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Wellness, immunity, and performance blends.", false, "Vitamins & Dietary Supplements", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Styling tools, treatments, and color.", false, "Hair Care", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Perfumes, colognes, and body mists.", false, "Fragrances", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Excavators, loaders, and industrial vehicles.", false, "Heavy Equipment", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Maintenance, repair, and operations essentials.", false, "MRO & Industrial Supplies", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Point-of-sale, signage, and consulting packages.", false, "Retail & Services", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Printers, copiers, and office machines.", false, "Office Equipment", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Electric, acoustic, and bass guitars.", false, "Guitars & Basses", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mixers, microphones, and studio gear.", false, "Pro Audio Equipment", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Controllers, turntables, and lighting.", false, "DJ Equipment", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Saxes, trumpets, clarinets, and accessories.", false, "Brass & Woodwind", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Beds, crates, and training essentials.", false, "Dog Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Litter, scratchers, and cat furniture.", false, "Cat Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Aquariums, filtration, and décor.", false, "Fish & Aquarium", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Habitat accessories for hamsters, rabbits, and more.", false, "Small Animal Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lightweight, jogging, and convertible options.", false, "Strollers & Travel Systems", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cribs, dressers, and gliders.", false, "Nursery Furniture", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Monitors, gates, and proofing essentials.", false, "Baby Safety", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bottles, warmers, and nursing support.", false, "Baby Feeding", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Stamps, dies, and embellishments.", false, "Scrapbooking & Paper Crafting", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Paints, canvases, and studio tools.", false, "Art Supplies", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yardage, quilting, and upholstery textiles.", false, "Fabric", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Findings, gemstones, and tools.", false, "Beads & Jewelry Making", new Guid("e0000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "category_condition",
                columns: new[] { "category_id", "condition_id" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "category_specific",
                columns: new[] { "id", "allow_multiple", "is_required", "name", "values", "category_id" },
                values: new object[,]
                {
                    { new Guid("10400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"LG\",\"Samsung\",\"Sony\",\"TCL\",\"Vizio\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10400000-0000-0000-0000-000000000002"), false, true, "Type", "[\"4K UHD TV\",\"Soundbar\",\"AV Receiver\",\"Streaming Device\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10400000-0000-0000-0000-000000000003"), false, false, "Smart Platform", "[\"Google TV\",\"Roku\",\"Fire TV\",\"webOS\",\"Tizen\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10500000-0000-0000-0000-000000000001"), false, true, "Platform", "[\"Nintendo\",\"PlayStation\",\"Xbox\",\"Steam Deck\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10500000-0000-0000-0000-000000000002"), false, true, "Model", "[\"PlayStation 5\",\"Xbox Series X\",\"Nintendo Switch OLED\",\"Steam Deck OLED\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10500000-0000-0000-0000-000000000003"), false, false, "Storage Capacity", "[\"64 GB\",\"512 GB\",\"1 TB\",\"2 TB\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10600000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Apple\",\"Fitbit\",\"Garmin\",\"Samsung\",\"Withings\"]", new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("10600000-0000-0000-0000-000000000002"), true, false, "Features", "[\"GPS\",\"Heart Rate Monitor\",\"NFC\",\"SpO2\",\"Sleep Tracking\"]", new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("10700000-0000-0000-0000-000000000001"), false, true, "Device Type", "[\"Smart Speaker\",\"Smart Display\",\"Smart Lighting\",\"Smart Thermostat\",\"Security Camera\"]", new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("10700000-0000-0000-0000-000000000002"), false, false, "Ecosystem", "[\"Alexa\",\"Apple Home\",\"Google Home\",\"Matter\",\"SmartThings\"]", new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("10800000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Dash Cam\",\"GPS\",\"Car Stereo\",\"Backup Camera\",\"Radar Detector\"]", new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("10800000-0000-0000-0000-000000000002"), true, false, "Compatible Vehicle", "[\"Universal\",\"Ford\",\"GM\",\"Toyota\",\"Volkswagen\"]", new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("20300000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Banana Republic\",\"Hugo Boss\",\"Levi\\u0027s\",\"Nike\",\"Ralph Lauren\"]", new Guid("20000000-0000-0000-0000-000000000004") },
                    { new Guid("20300000-0000-0000-0000-000000000002"), false, true, "Size", "[\"S\",\"M\",\"L\",\"XL\",\"XXL\"]", new Guid("20000000-0000-0000-0000-000000000004") },
                    { new Guid("20400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Coach\",\"Gucci\",\"Kate Spade\",\"Louis Vuitton\",\"Tory Burch\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20400000-0000-0000-0000-000000000002"), true, false, "Materials", "[\"Canvas\",\"Leather\",\"Nylon\",\"Patent Leather\",\"Vegan Leather\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20400000-0000-0000-0000-000000000003"), false, false, "Style", "[\"Backpack\",\"Crossbody\",\"Satchel\",\"Shoulder Bag\",\"Tote\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20500000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Birkenstock\",\"Clarks\",\"Dr. Martens\",\"Sam Edelman\",\"Steve Madden\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20500000-0000-0000-0000-000000000002"), false, true, "US Shoe Size", "[\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20500000-0000-0000-0000-000000000003"), false, false, "Style", "[\"Boots\",\"Flats\",\"Heels\",\"Sandals\",\"Sneakers\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20600000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Casio\",\"Citizen\",\"Omega\",\"Rolex\",\"Seiko\"]", new Guid("20000000-0000-0000-0000-000000000007") },
                    { new Guid("20600000-0000-0000-0000-000000000002"), false, false, "Movement", "[\"Automatic\",\"Quartz\",\"Mechanical\",\"Solar\"]", new Guid("20000000-0000-0000-0000-000000000007") },
                    { new Guid("20700000-0000-0000-0000-000000000001"), false, true, "Metal", "[\"Gold\",\"Platinum\",\"Rose Gold\",\"Sterling Silver\",\"White Gold\"]", new Guid("20000000-0000-0000-0000-000000000008") },
                    { new Guid("20700000-0000-0000-0000-000000000002"), false, true, "Jewelry Type", "[\"Bracelet\",\"Earrings\",\"Necklace\",\"Ring\"]", new Guid("20000000-0000-0000-0000-000000000008") },
                    { new Guid("30100000-0000-0000-0000-000000000006"), false, false, "Style", "[\"Bohemian\",\"Farmhouse\",\"Mid-Century\",\"Modern\",\"Traditional\"]", new Guid("30000000-0000-0000-0000-000000000004") },
                    { new Guid("30100000-0000-0000-0000-000000000007"), false, true, "Room", "[\"Bedroom\",\"Dining Room\",\"Kitchen\",\"Living Room\",\"Office\"]", new Guid("30000000-0000-0000-0000-000000000004") },
                    { new Guid("30300000-0000-0000-0000-000000000001"), false, false, "Brand", "[\"Bosch\",\"DeWalt\",\"Hilti\",\"Makita\",\"Milwaukee\"]", new Guid("30000000-0000-0000-0000-000000000005") },
                    { new Guid("30300000-0000-0000-0000-000000000002"), false, true, "Power Source", "[\"Battery\",\"Corded Electric\",\"Compressed Air\",\"Manual\"]", new Guid("30000000-0000-0000-0000-000000000005") },
                    { new Guid("30400000-0000-0000-0000-000000000001"), false, true, "Category", "[\"Flooring\",\"Hardware\",\"Lighting\",\"Plumbing\",\"Storage\"]", new Guid("30000000-0000-0000-0000-000000000007") },
                    { new Guid("30400000-0000-0000-0000-000000000002"), false, false, "Finish", "[\"Brushed Nickel\",\"Chrome\",\"Matte Black\",\"Oil-Rubbed Bronze\"]", new Guid("30000000-0000-0000-0000-000000000007") }
                });

            migrationBuilder.InsertData(
                table: "category_condition",
                columns: new[] { "category_id", "condition_id" },
                values: new object[,]
                {
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "category_specific",
                columns: new[] { "id", "allow_multiple", "is_required", "name", "values", "category_id" },
                values: new object[,]
                {
                    { new Guid("40100000-0000-0000-0000-000000000001"), false, true, "Part Type", "[\"Brakes\",\"Engine\",\"Exterior\",\"Interior\",\"Suspension\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40100000-0000-0000-0000-000000000002"), false, false, "Brand", "[\"ACDelco\",\"Bosch\",\"Denso\",\"Mopar\",\"Motorcraft\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40100000-0000-0000-0000-000000000003"), true, false, "Compatible Make", "[\"Chevrolet\",\"Ford\",\"Honda\",\"Toyota\",\"Universal\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40200000-0000-0000-0000-000000000001"), false, true, "Part Type", "[\"Body \\u0026 Frame\",\"Drivetrain\",\"Electrical\",\"Engine\",\"Suspension\"]", new Guid("50000000-0000-0000-0000-000000000003") },
                    { new Guid("40300000-0000-0000-0000-000000000001"), false, true, "Tool Type", "[\"Diagnostic\",\"Hand Tool\",\"Lifts \\u0026 Jacks\",\"Power Tool\",\"Specialty\"]", new Guid("50000000-0000-0000-0000-000000000004") },
                    { new Guid("40400000-0000-0000-0000-000000000001"), false, true, "Tire Type", "[\"All-Season\",\"Performance\",\"Snow/Winter\",\"Off-Road\"]", new Guid("50000000-0000-0000-0000-000000000005") },
                    { new Guid("40400000-0000-0000-0000-000000000002"), false, false, "Rim Diameter", "[\"16 in\",\"17 in\",\"18 in\",\"19 in\",\"20 in\"]", new Guid("50000000-0000-0000-0000-000000000005") },
                    { new Guid("60100000-0000-0000-0000-000000000001"), false, true, "Franchise", "[\"Magic: The Gathering\",\"Pok\\u00E9mon\",\"Yu-Gi-Oh!\",\"Marvel Snap\",\"Disney Lorcana\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60100000-0000-0000-0000-000000000002"), false, true, "Card Condition", "[\"Gem Mint\",\"Near Mint\",\"Lightly Played\",\"Moderately Played\",\"Heavily Played\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60100000-0000-0000-0000-000000000003"), false, false, "Graded", "[\"BGS\",\"CGC\",\"PSA\",\"SGC\",\"Ungraded\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60200000-0000-0000-0000-000000000001"), false, true, "Publisher", "[\"DC\",\"Dark Horse\",\"IDW\",\"Image\",\"Marvel\"]", new Guid("60000000-0000-0000-0000-000000000003") },
                    { new Guid("60200000-0000-0000-0000-000000000002"), false, false, "Era", "[\"Golden Age\",\"Silver Age\",\"Bronze Age\",\"Modern\"]", new Guid("60000000-0000-0000-0000-000000000003") },
                    { new Guid("60300000-0000-0000-0000-000000000001"), false, true, "Artist", "[\"Andy Warhol\",\"Banksy\",\"Jean-Michel Basquiat\",\"Salvador Dal\\u00ED\",\"Yoshitomo Nara\"]", new Guid("60000000-0000-0000-0000-000000000004") },
                    { new Guid("60300000-0000-0000-0000-000000000002"), false, false, "Medium", "[\"Gicl\\u00E9e\",\"Lithograph\",\"Screenprint\",\"Serigraph\"]", new Guid("60000000-0000-0000-0000-000000000004") },
                    { new Guid("60400000-0000-0000-0000-000000000001"), false, false, "Certification", "[\"ANACS\",\"NGC\",\"PCGS\",\"PMG\",\"Uncertified\"]", new Guid("60000000-0000-0000-0000-000000000005") },
                    { new Guid("60400000-0000-0000-0000-000000000002"), false, false, "Grade", "[\"MS 70\",\"MS 69\",\"MS 65\",\"AU 55\",\"XF 45\"]", new Guid("60000000-0000-0000-0000-000000000005") },
                    { new Guid("70100000-0000-0000-0000-000000000001"), false, true, "Franchise", "[\"DC\",\"Dragon Ball\",\"Marvel\",\"Star Wars\",\"Transformers\"]", new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("70100000-0000-0000-0000-000000000002"), false, false, "Scale", "[\"1:6\",\"1:12\",\"1:18\",\"6 in\",\"12 in\"]", new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("70200000-0000-0000-0000-000000000001"), false, true, "Scale", "[\"HO\",\"N\",\"O\",\"G\",\"Z\"]", new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("70200000-0000-0000-0000-000000000002"), false, false, "Power Type", "[\"DC\",\"DCC\",\"Battery\"]", new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("70300000-0000-0000-0000-000000000001"), false, true, "Vehicle Type", "[\"Car\",\"Truck\",\"Boat\",\"Plane\",\"Drone\"]", new Guid("70000000-0000-0000-0000-000000000004") },
                    { new Guid("70300000-0000-0000-0000-000000000002"), false, false, "Power", "[\"Electric\",\"Gas\",\"Nitro\"]", new Guid("70000000-0000-0000-0000-000000000004") },
                    { new Guid("70400000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Barbie\",\"Fashion Doll\",\"Teddy Bear\",\"Vintage Doll\"]", new Guid("70000000-0000-0000-0000-000000000005") },
                    { new Guid("70500000-0000-0000-0000-000000000001"), false, false, "Theme", "[\"Architecture\",\"City\",\"Ideas\",\"Star Wars\",\"Technic\"]", new Guid("70000000-0000-0000-0000-000000000006") },
                    { new Guid("80100000-0000-0000-0000-000000000001"), false, true, "Sport", "[\"Camping\",\"Climbing\",\"Fishing\",\"Hunting\",\"Water Sports\"]", new Guid("80000000-0000-0000-0000-000000000002") },
                    { new Guid("80200000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"Cardio Machine\",\"Free Weights\",\"Resistance Bands\",\"Yoga Mat\"]", new Guid("80000000-0000-0000-0000-000000000003") },
                    { new Guid("80300000-0000-0000-0000-000000000001"), false, true, "Bicycle Type", "[\"Mountain\",\"Road\",\"Hybrid\",\"Gravel\",\"Electric\"]", new Guid("80000000-0000-0000-0000-000000000004") },
                    { new Guid("80300000-0000-0000-0000-000000000002"), false, false, "Frame Size", "[\"Small\",\"Medium\",\"Large\",\"X-Large\"]", new Guid("80000000-0000-0000-0000-000000000004") },
                    { new Guid("80400000-0000-0000-0000-000000000001"), false, true, "Club Type", "[\"Driver\",\"Fairway Wood\",\"Hybrid\",\"Iron Set\",\"Putter\"]", new Guid("80000000-0000-0000-0000-000000000005") },
                    { new Guid("80400000-0000-0000-0000-000000000002"), false, false, "Flex", "[\"Ladies\",\"Regular\",\"Stiff\",\"Extra Stiff\"]", new Guid("80000000-0000-0000-0000-000000000005") },
                    { new Guid("90100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Charlotte Tilbury\",\"Dior\",\"Fenty Beauty\",\"MAC\",\"Rare Beauty\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90100000-0000-0000-0000-000000000002"), false, true, "Product Type", "[\"Foundation\",\"Eyeshadow\",\"Lipstick\",\"Mascara\",\"Primer\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90100000-0000-0000-0000-000000000003"), false, false, "Shade", "[\"Fair\",\"Light\",\"Medium\",\"Tan\",\"Deep\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"CeraVe\",\"Dermalogica\",\"Drunk Elephant\",\"La Roche-Posay\",\"Tatcha\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90200000-0000-0000-0000-000000000002"), false, true, "Skin Type", "[\"Dry\",\"Normal\",\"Oily\",\"Combination\",\"Sensitive\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90200000-0000-0000-0000-000000000003"), true, false, "Concern", "[\"Acne\",\"Anti-Aging\",\"Brightening\",\"Hydration\",\"Sun Protection\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90300000-0000-0000-0000-000000000001"), false, true, "Formulation", "[\"Capsule\",\"Gummy\",\"Powder\",\"Softgel\",\"Tablet\"]", new Guid("90000000-0000-0000-0000-000000000004") },
                    { new Guid("90300000-0000-0000-0000-000000000002"), true, false, "Main Purpose", "[\"Energy\",\"General Wellness\",\"Immune Support\",\"Joint Health\",\"Sleep\"]", new Guid("90000000-0000-0000-0000-000000000004") },
                    { new Guid("90400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Amika\",\"Dyson\",\"GHD\",\"Olaplex\",\"Redken\"]", new Guid("90000000-0000-0000-0000-000000000005") },
                    { new Guid("90400000-0000-0000-0000-000000000002"), true, false, "Hair Type", "[\"Coily\",\"Curly\",\"Straight\",\"Wavy\",\"Fine\"]", new Guid("90000000-0000-0000-0000-000000000005") },
                    { new Guid("90500000-0000-0000-0000-000000000001"), false, true, "Fragrance Type", "[\"Eau de Parfum\",\"Eau de Toilette\",\"Eau de Cologne\",\"Perfume Oil\"]", new Guid("90000000-0000-0000-0000-000000000006") },
                    { new Guid("90500000-0000-0000-0000-000000000002"), false, false, "Volume", "[\"30 ml\",\"50 ml\",\"75 ml\",\"100 ml\"]", new Guid("90000000-0000-0000-0000-000000000006") },
                    { new Guid("a0100000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"Backhoe\",\"Bulldozer\",\"Excavator\",\"Forklift\",\"Skid Steer\"]", new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("a0100000-0000-0000-0000-000000000002"), false, false, "Hours", "[\"0-500\",\"501-1500\",\"1501-3000\",\"3001\\u002B\"]", new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("a0200000-0000-0000-0000-000000000001"), false, true, "Supply Type", "[\"Fasteners\",\"Hydraulics\",\"HVAC\",\"Pneumatics\",\"Safety\"]", new Guid("a0000000-0000-0000-0000-000000000003") },
                    { new Guid("a0300000-0000-0000-0000-000000000001"), false, true, "Service Type", "[\"Consulting\",\"Installation\",\"Maintenance\",\"Marketing\",\"Training\"]", new Guid("a0000000-0000-0000-0000-000000000004") },
                    { new Guid("a0400000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"3D Printer\",\"Laser Printer\",\"Multifunction\",\"Scanner\",\"Shredder\"]", new Guid("a0000000-0000-0000-0000-000000000005") },
                    { new Guid("b0100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Fender\",\"Gibson\",\"Ibanez\",\"PRS\",\"Taylor\"]", new Guid("b0000000-0000-0000-0000-000000000002") },
                    { new Guid("b0100000-0000-0000-0000-000000000002"), false, false, "Body Type", "[\"Solid\",\"Semi-Hollow\",\"Hollow\"]", new Guid("b0000000-0000-0000-0000-000000000002") },
                    { new Guid("b0200000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Mixer\",\"Microphone\",\"Monitor\",\"Interface\",\"Outboard Gear\"]", new Guid("b0000000-0000-0000-0000-000000000003") },
                    { new Guid("b0300000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Controller\",\"Mixer\",\"Turntable\",\"Lighting\"]", new Guid("b0000000-0000-0000-0000-000000000004") },
                    { new Guid("b0400000-0000-0000-0000-000000000001"), false, true, "Instrument", "[\"Clarinet\",\"Flute\",\"Saxophone\",\"Trombone\",\"Trumpet\"]", new Guid("b0000000-0000-0000-0000-000000000005") },
                    { new Guid("c0100000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Apparel\",\"Crates\",\"Food\",\"Grooming\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0100000-0000-0000-0000-000000000002"), false, false, "Size", "[\"XS\",\"S\",\"M\",\"L\",\"XL\"]", new Guid("c0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0200000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Food\",\"Litter\",\"Scratchers\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000003") },
                    { new Guid("c0300000-0000-0000-0000-000000000001"), false, true, "Aquarium Type", "[\"Freshwater\",\"Marine\",\"Reef\",\"Brackish\"]", new Guid("c0000000-0000-0000-0000-000000000004") },
                    { new Guid("c0300000-0000-0000-0000-000000000002"), false, false, "Tank Capacity", "[\"1-10 gal\",\"11-30 gal\",\"31-55 gal\",\"56\\u002B gal\"]", new Guid("c0000000-0000-0000-0000-000000000004") },
                    { new Guid("c0400000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Bedding\",\"Cages\",\"Food\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000005") },
                    { new Guid("d0100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Baby Jogger\",\"Bugaboo\",\"Chicco\",\"Graco\",\"UPPAbaby\"]", new Guid("d0000000-0000-0000-0000-000000000002") },
                    { new Guid("d0100000-0000-0000-0000-000000000002"), false, true, "Stroller Type", "[\"Full-Size\",\"Jogging\",\"Travel System\",\"Umbrella\"]", new Guid("d0000000-0000-0000-0000-000000000002") },
                    { new Guid("d0200000-0000-0000-0000-000000000001"), false, true, "Furniture Piece", "[\"Crib\",\"Changing Table\",\"Glider\",\"Dresser\"]", new Guid("d0000000-0000-0000-0000-000000000003") },
                    { new Guid("d0300000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Baby Monitor\",\"Safety Gate\",\"Outlet Cover\",\"Cabinet Lock\"]", new Guid("d0000000-0000-0000-0000-000000000004") },
                    { new Guid("d0400000-0000-0000-0000-000000000001"), false, true, "Feeding Type", "[\"Bottle\",\"Breastfeeding\",\"Solid Food\",\"Toddler\"]", new Guid("d0000000-0000-0000-0000-000000000005") },
                    { new Guid("e0100000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Adhesives\",\"Dies\",\"Paper\",\"Stamps\"]", new Guid("e0000000-0000-0000-0000-000000000002") },
                    { new Guid("e0200000-0000-0000-0000-000000000001"), false, true, "Medium", "[\"Acrylic\",\"Oil\",\"Watercolor\",\"Pastel\"]", new Guid("e0000000-0000-0000-0000-000000000003") },
                    { new Guid("e0300000-0000-0000-0000-000000000001"), false, true, "Fabric Type", "[\"Cotton\",\"Denim\",\"Fleece\",\"Linen\",\"Silk\"]", new Guid("e0000000-0000-0000-0000-000000000004") },
                    { new Guid("e0400000-0000-0000-0000-000000000001"), false, true, "Material", "[\"Glass\",\"Gemstone\",\"Metal\",\"Resin\",\"Wood\"]", new Guid("e0000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.CreateIndex(
                name: "ix_report_downloads_user_id_reference_code",
                table: "report_downloads",
                columns: new[] { "user_id", "reference_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_report_schedules_user_id_source_type_is_active",
                table: "report_schedules",
                columns: new[] { "user_id", "source", "type", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "report_downloads");

            migrationBuilder.DropTable(
                name: "report_schedules");

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000008") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000008") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000008") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000007") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000008") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000005") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "category_condition",
                keyColumns: new[] { "category_id", "condition_id" },
                keyValues: new object[] { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10400000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10500000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10500000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10500000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10600000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10600000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10700000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10700000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10800000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("10800000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20400000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20500000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20500000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20500000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20600000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20600000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20700000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("20700000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30100000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("30400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40100000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("40400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60100000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60200000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("60400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70200000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("70500000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("80400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90100000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90200000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90200000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90400000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90500000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("90500000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("a0100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("a0100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("a0200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("a0300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("a0400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("b0100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("b0100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("b0200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("b0300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("b0400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0300000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("c0400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("d0100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("d0100000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("d0200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("d0300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("d0400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("e0100000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("e0200000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("e0300000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category_specific",
                keyColumn: "id",
                keyValue: new Guid("e0400000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("d0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("d0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("d0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("d0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("80000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("90000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("d0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "category",
                keyColumn: "id",
                keyValue: new Guid("e0000000-0000-0000-0000-000000000001"));
        }
    }
}
