using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLastPriceChangeDateToListing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sale_event_listing");

            migrationBuilder.DropTable(
                name: "sale_event_discount_tier");

            migrationBuilder.DropTable(
                name: "sale_event");

            migrationBuilder.AddColumn<DateTime>(
                name: "last_price_change_date",
                table: "listing",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "applied_sale_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_tier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    discount_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    applied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applied_sale_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_performance_metrics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_count = table.Column<int>(type: "integer", nullable: false),
                    total_discount_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_sales_revenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_items_sold = table.Column<int>(type: "integer", nullable: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_performance_metrics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_price_snapshots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    snapshot_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_price_snapshots", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    mode = table.Column<int>(type: "integer", nullable: false),
                    highlight_percentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    offer_free_shipping = table.Column<bool>(type: "boolean", nullable: false),
                    block_price_increase_revisions = table.Column<bool>(type: "boolean", nullable: false),
                    include_skipped_items = table.Column<bool>(type: "boolean", nullable: false),
                    buyer_message_label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_discount_tiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_type = table.Column<int>(type: "integer", nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_discount_tiers", x => x.id);
                    table.ForeignKey(
                        name: "fk_sale_event_discount_tiers_sale_events_sale_event_id",
                        column: x => x.sale_event_id,
                        principalTable: "sale_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_listings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_tier_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_listings", x => x.id);
                    table.ForeignKey(
                        name: "fk_sale_event_listings_sale_event_discount_tiers_discount_tier",
                        column: x => x.discount_tier_id,
                        principalTable: "sale_event_discount_tiers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sale_event_listings_sale_events_sale_event_id",
                        column: x => x.sale_event_id,
                        principalTable: "sale_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000001"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000002"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000003"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000004"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000005"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000006"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000007"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000008"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000009"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000010"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000011"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000012"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000013"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000014"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000015"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000016"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000017"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000018"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000019"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000020"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000021"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000022"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000023"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000024"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000025"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000026"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000027"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000028"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000029"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000030"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000031"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000032"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000033"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000034"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000035"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000036"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000037"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000038"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000039"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000040"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000041"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000042"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000043"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000044"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000045"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000046"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000047"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000048"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000049"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000050"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000051"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000052"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000053"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000054"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000055"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000056"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000057"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000058"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000059"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000060"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000061"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000062"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000063"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000064"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000001"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000002"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000003"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000004"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000005"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000006"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000007"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000008"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000009"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000010"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000011"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000012"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000013"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000014"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000015"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000016"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000017"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000018"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000019"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000020"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000021"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000022"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000023"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000024"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000025"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000026"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000027"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000028"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000029"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000030"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000031"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000032"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000033"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000034"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000035"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000036"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000037"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000038"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000039"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000040"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000041"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000042"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000043"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000044"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000045"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000046"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000047"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000048"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000049"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000050"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000051"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000052"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000053"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000054"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000055"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000056"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000057"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000058"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000059"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000060"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000061"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000062"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000063"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000064"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000001"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000002"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000003"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000004"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000005"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000006"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000007"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000008"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000009"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000010"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000011"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000012"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000013"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000014"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000015"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000016"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000017"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000018"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000019"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000020"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000021"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000022"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000023"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000024"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000025"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000026"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000027"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000028"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000029"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000030"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000031"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000032"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000033"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000034"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000035"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000036"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000037"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000038"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000039"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000040"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000041"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000042"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000043"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000044"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000045"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000046"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000047"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000048"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000049"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000050"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000051"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000052"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000053"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000054"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000055"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000056"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000057"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000058"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000059"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005a"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005b"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005c"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005d"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005e"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005f"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000060"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000061"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000062"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000063"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000064"),
                column: "last_price_change_date",
                value: null);

            migrationBuilder.CreateIndex(
                name: "ix_applied_sale_events_order_id",
                table: "applied_sale_events",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_applied_sale_events_sale_event_id",
                table: "applied_sale_events",
                column: "sale_event_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_discount_tiers_sale_event_id_priority",
                table: "sale_event_discount_tiers",
                columns: new[] { "sale_event_id", "priority" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_listings_discount_tier_id",
                table: "sale_event_listings",
                column: "discount_tier_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_listings_listing_id",
                table: "sale_event_listings",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_listings_sale_event_id_listing_id",
                table: "sale_event_listings",
                columns: new[] { "sale_event_id", "listing_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_performance_metrics_sale_event_id",
                table: "sale_event_performance_metrics",
                column: "sale_event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_price_snapshots_sale_event_id_listing_id",
                table: "sale_event_price_snapshots",
                columns: new[] { "sale_event_id", "listing_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sale_events_seller_id",
                table: "sale_events",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_events_start_date_end_date",
                table: "sale_events",
                columns: new[] { "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_sale_events_status",
                table: "sale_events",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applied_sale_events");

            migrationBuilder.DropTable(
                name: "sale_event_listings");

            migrationBuilder.DropTable(
                name: "sale_event_performance_metrics");

            migrationBuilder.DropTable(
                name: "sale_event_price_snapshots");

            migrationBuilder.DropTable(
                name: "sale_event_discount_tiers");

            migrationBuilder.DropTable(
                name: "sale_events");

            migrationBuilder.DropColumn(
                name: "last_price_change_date",
                table: "listing");

            migrationBuilder.CreateTable(
                name: "sale_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    block_price_increase_revisions = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    highlight_percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    include_skipped_items = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false),
                    offer_free_shipping = table.Column<bool>(type: "boolean", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_discount_tier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_discount_tier", x => x.id);
                    table.ForeignKey(
                        name: "fk_sale_event_discount_tier_sale_event_sale_event_id",
                        column: x => x.sale_event_id,
                        principalTable: "sale_event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_listing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_tier_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sale_event_listing", x => x.id);
                    table.ForeignKey(
                        name: "fk_sale_event_listing_sale_event_discount_tier_discount_tier_id",
                        column: x => x.discount_tier_id,
                        principalTable: "sale_event_discount_tier",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_sale_event_listing_sale_event_sale_event_id",
                        column: x => x.sale_event_id,
                        principalTable: "sale_event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_discount_tier_sale_event_id",
                table: "sale_event_discount_tier",
                column: "sale_event_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_listing_discount_tier_id",
                table: "sale_event_listing",
                column: "discount_tier_id");

            migrationBuilder.CreateIndex(
                name: "ix_sale_event_listing_sale_event_id_listing_id",
                table: "sale_event_listing",
                columns: new[] { "sale_event_id", "listing_id" },
                unique: true);
        }
    }
}
