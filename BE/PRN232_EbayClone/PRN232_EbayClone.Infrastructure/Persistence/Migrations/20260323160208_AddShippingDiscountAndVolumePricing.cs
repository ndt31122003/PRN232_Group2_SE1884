using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingDiscountAndVolumePricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shipping_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    discount_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_unit = table.Column<int>(type: "integer", nullable: false),
                    is_free_shipping = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    minimum_order_value = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipping_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volume_pricings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volume_pricings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "volume_pricing_tiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    volume_pricing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    min_quantity = table.Column<int>(type: "integer", nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_unit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volume_pricing_tiers", x => x.id);
                    table.ForeignKey(
                        name: "fk_volume_pricing_tiers_volume_pricings_volume_pricing_id",
                        column: x => x.volume_pricing_id,
                        principalTable: "volume_pricings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_shipping_discounts_active",
                table: "shipping_discounts",
                columns: new[] { "is_active", "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_shipping_discounts_seller_id",
                table: "shipping_discounts",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "ix_volume_pricing_tiers_volume_pricing_id",
                table: "volume_pricing_tiers",
                column: "volume_pricing_id");

            migrationBuilder.CreateIndex(
                name: "ix_volume_pricings_active",
                table: "volume_pricings",
                columns: new[] { "is_active", "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_volume_pricings_listing_id",
                table: "volume_pricings",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_volume_pricings_seller_id",
                table: "volume_pricings",
                column: "seller_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shipping_discounts");

            migrationBuilder.DropTable(
                name: "volume_pricing_tiers");

            migrationBuilder.DropTable(
                name: "volume_pricings");
        }
    }
}
