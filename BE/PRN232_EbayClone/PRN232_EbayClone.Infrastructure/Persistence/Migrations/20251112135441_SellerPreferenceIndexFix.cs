using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SellerPreferenceIndexFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seller_preference",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listings_stay_active_when_out_of_stock = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    show_exact_quantity_available = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    buyers_can_see_vat_number = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    vat_number = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    block_unpaid_item_strikes = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    unpaid_item_strikes_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    unpaid_item_strikes_period_months = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    block_primary_address_outside_shipping_location = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    block_max_items_last_ten_days = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    max_items_last_ten_days = table.Column<int>(type: "integer", nullable: true),
                    apply_feedback_score_threshold = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    feedback_score_threshold = table.Column<int>(type: "integer", nullable: true),
                    update_block_settings_active_listings = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    require_payment_method_before_bid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    require_payment_method_before_offer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    prevent_blocked_buyers_contacting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    invoice_format = table.Column<int>(type: "integer", nullable: false),
                    invoice_send_email_copy = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    invoice_apply_credits_automatically = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seller_preference", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "seller_blocked_buyer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    normalized_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    seller_preference_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seller_blocked_buyer", x => x.id);
                    table.ForeignKey(
                        name: "fk_seller_blocked_buyer_seller_preference_seller_preference_id",
                        column: x => x.seller_preference_id,
                        principalTable: "seller_preference",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_exempt_buyer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    normalized_identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    seller_preference_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seller_exempt_buyer", x => x.id);
                    table.ForeignKey(
                        name: "fk_seller_exempt_buyer_seller_preference_seller_preference_id",
                        column: x => x.seller_preference_id,
                        principalTable: "seller_preference",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_seller_blocked_buyer_seller_preference_id",
                table: "seller_blocked_buyer",
                column: "seller_preference_id");

            migrationBuilder.CreateIndex(
                name: "ux_seller_blocked_buyer_identifier",
                table: "seller_blocked_buyer",
                columns: new[] { "normalized_identifier", "seller_preference_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_seller_exempt_buyer_seller_preference_id",
                table: "seller_exempt_buyer",
                column: "seller_preference_id");

            migrationBuilder.CreateIndex(
                name: "ux_seller_exempt_buyer_identifier",
                table: "seller_exempt_buyer",
                columns: new[] { "normalized_identifier", "seller_preference_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_seller_preference_seller",
                table: "seller_preference",
                column: "seller_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seller_blocked_buyer");

            migrationBuilder.DropTable(
                name: "seller_exempt_buyer");

            migrationBuilder.DropTable(
                name: "seller_preference");
        }
    }
}
