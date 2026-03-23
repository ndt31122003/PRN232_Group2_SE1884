using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderDiscountPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applied_order_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    applied_tier_id = table.Column<Guid>(type: "uuid", nullable: true),
                    applied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_applied_order_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_discount_performance_metrics",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    total_discount_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    total_sales_revenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    total_items_sold = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_discount_performance_metrics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    threshold_type = table.Column<int>(type: "integer", nullable: false),
                    threshold_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    threshold_quantity = table.Column<int>(type: "integer", nullable: true),
                    discount_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_unit = table.Column<int>(type: "integer", nullable: false),
                    max_discount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    apply_to_all_items = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("pk_order_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_discount_category_rules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_exclusion = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_discount_category_rules", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_discount_category_rules_order_discounts_order_discoun",
                        column: x => x.order_discount_id,
                        principalTable: "order_discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_discount_item_rules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_exclusion = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_discount_item_rules", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_discount_item_rules_order_discounts_order_discount_id",
                        column: x => x.order_discount_id,
                        principalTable: "order_discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_discount_tiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_discount_id = table.Column<Guid>(type: "uuid", nullable: false),
                    threshold_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tier_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_discount_tiers", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_discount_tiers_order_discounts_order_discount_id",
                        column: x => x.order_discount_id,
                        principalTable: "order_discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_applied_order_discounts_order_discount_id",
                table: "applied_order_discounts",
                column: "order_discount_id");

            migrationBuilder.CreateIndex(
                name: "ix_applied_order_discounts_order_id",
                table: "applied_order_discounts",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_discount_category_rules_order_discount_id",
                table: "order_discount_category_rules",
                column: "order_discount_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_discount_item_rules_listing_id",
                table: "order_discount_item_rules",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_discount_item_rules_order_discount_id",
                table: "order_discount_item_rules",
                column: "order_discount_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_discount_tiers_order_discount_id",
                table: "order_discount_tiers",
                column: "order_discount_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_discounts_active",
                table: "order_discounts",
                columns: new[] { "is_active", "start_date", "end_date" });

            migrationBuilder.CreateIndex(
                name: "ix_order_discounts_seller_id",
                table: "order_discounts",
                column: "seller_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applied_order_discounts");

            migrationBuilder.DropTable(
                name: "order_discount_category_rules");

            migrationBuilder.DropTable(
                name: "order_discount_item_rules");

            migrationBuilder.DropTable(
                name: "order_discount_performance_metrics");

            migrationBuilder.DropTable(
                name: "order_discount_tiers");

            migrationBuilder.DropTable(
                name: "order_discounts");
        }
    }
}
