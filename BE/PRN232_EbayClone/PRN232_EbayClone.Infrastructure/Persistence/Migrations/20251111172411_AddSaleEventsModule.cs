using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleEventsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sale_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    offer_free_shipping = table.Column<bool>(type: "boolean", nullable: false),
                    include_skipped_items = table.Column<bool>(type: "boolean", nullable: false),
                    block_price_increase_revisions = table.Column<bool>(type: "boolean", nullable: false),
                    highlight_percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
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
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
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
                    sale_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_tier_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sale_event_listing");

            migrationBuilder.DropTable(
                name: "sale_event_discount_tier");

            migrationBuilder.DropTable(
                name: "sale_event");
        }
    }
}
