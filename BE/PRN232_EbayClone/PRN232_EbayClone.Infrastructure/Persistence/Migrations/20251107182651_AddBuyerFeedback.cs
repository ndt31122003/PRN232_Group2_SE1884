using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBuyerFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_buyer_feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    uses_stored_comment = table.Column<bool>(type: "boolean", nullable: false),
                    stored_comment_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    follow_up_comment = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    follow_up_commented_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_buyer_feedback", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_buyer_feedback_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_buyer_feedback_user_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "archived_at", "buyer_id", "cancelled_at", "coupon_code", "created_at", "created_by", "delivered_at", "fulfillment_type", "is_deleted", "order_number", "ordered_at", "paid_at", "promotion_id", "seller_id", "shipped_at", "shipping_status", "status_id", "updated_at", "updated_by", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[,]
                {
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000011"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1011", new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.10m, "USD", 9.95m, "USD", 58.99m, "USD", 5.30m, "USD", 77.34m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000012"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1012", new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), "seed", 2.50m, "USD", 3.45m, "USD", 8.25m, "USD", 64.50m, "USD", 4.86m, "USD", 78.56m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000013"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1013", new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.90m, "USD", 10.00m, "USD", 72.00m, "USD", 6.12m, "USD", 92.02m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000014"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1014", new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), "seed", 1.20m, "USD", 2.95m, "USD", 7.80m, "USD", 55.75m, "USD", 4.46m, "USD", 69.76m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[,]
                {
                    { new Guid("c579fb6b-b172-4e17-b610-000000000021"), new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-10/640/640", false, new Guid("71000000-0000-0000-0000-00000000000a"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000011"), 1, "DEMO-1-0010", "Alice's Item #10", new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), "seed", null, 58.99m, "USD", 58.99m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000022"), new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-6/640/640", false, new Guid("72000000-0000-0000-0000-000000000006"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000012"), 1, "DEMO-2-0006", "Brian's Item #6", new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), "seed", null, 64.50m, "USD", 64.50m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000023"), new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-8/640/640", false, new Guid("73000000-0000-0000-0000-000000000008"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000013"), 1, "DEMO-3-0008", "Cecilia's Item #8", new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), "seed", null, 72.00m, "USD", 72.00m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000024"), new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-11/640/640", false, new Guid("71000000-0000-0000-0000-00000000000b"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000014"), 1, "DEMO-1-0011", "Alice's Item #11", new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), "seed", null, 55.75m, "USD", 55.75m, "USD" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_buyer_feedback_buyer_id",
                table: "order_buyer_feedback",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "ux_buyer_feedback_order",
                table: "order_buyer_feedback",
                column: "order_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_buyer_feedback");

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000021"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000022"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000023"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000024"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000011"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000012"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000013"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000014"));
        }
    }
}
