using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSeed2s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "order_status_transitions",
                columns: new[] { "id", "allowed_roles", "from_status_id", "to_status_id" },
                values: new object[,]
                {
                    { new Guid("1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9"), "SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("5a3f5769-6c6d-4b89-9347-118bd3fba3d6"), "SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad") },
                    { new Guid("7cf6e659-8025-49e8-94d5-3a4dd3b5a793"), "SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060") },
                    { new Guid("8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e"), "SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "archived_at", "buyer_id", "cancelled_at", "coupon_code", "created_at", "created_by", "delivered_at", "fulfillment_type", "is_deleted", "order_number", "ordered_at", "paid_at", "promotion_id", "seller_id", "shipped_at", "shipping_status", "status_id", "updated_at", "updated_by", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[,]
                {
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000015"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1015", new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), "seed", 2.50m, "USD", 3.15m, "USD", 8.25m, "USD", 62.75m, "USD", 5.02m, "USD", 76.67m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000016"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1016", new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 2.45m, "USD", 7.50m, "USD", 48.40m, "USD", 3.87m, "USD", 62.22m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000017"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1017", new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.95m, "USD", 9.95m, "USD", 79.90m, "USD", 6.39m, "USD", 100.19m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1018", new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), "seed", 3.00m, "USD", 4.30m, "USD", 10.25m, "USD", 88.60m, "USD", 7.08m, "USD", 107.23m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000019"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1019", new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.55m, "USD", 8.75m, "USD", 71.25m, "USD", 5.70m, "USD", 89.25m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1020", new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), "seed", 1.80m, "USD", 3.25m, "USD", 8.40m, "USD", 65.80m, "USD", 4.94m, "USD", 80.59m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1021", new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 2.95m, "USD", 7.95m, "USD", 59.10m, "USD", 4.43m, "USD", 74.43m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001c"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1022", new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), "seed", 2.20m, "USD", 3.80m, "USD", 9.10m, "USD", 83.45m, "USD", 6.68m, "USD", 100.83m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1023", new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.45m, "USD", 10.60m, "USD", 90.30m, "USD", 7.22m, "USD", 112.57m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001e"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1024", new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), "seed", 1.50m, "USD", 3.60m, "USD", 8.90m, "USD", 74.95m, "USD", 5.83m, "USD", 91.78m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency", "refund_amount", "refund_currency" },
                values: new object[] { new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000004"), null, new Guid("70000000-0000-0000-0000-000000000003"), "Item still not handed to carrier, requesting cancellation.", null, new DateTime(2025, 11, 6, 18, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), 4, new DateTime(2025, 11, 6, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), "Approved – refund issued to buyer's original payment method.", new DateTime(2025, 11, 7, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 18, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 7, 9, 30, 0, 0, DateTimeKind.Utc), "seed", 107.23m, "USD", 107.23m, "USD" });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[] { new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000005"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Accidentally placed duplicate order.", null, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), 1, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 10, 11, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), "seed", 74.43m, "USD" });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[,]
                {
                    { new Guid("c579fb6b-b172-4e17-b610-000000000025"), new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-12/640/640", false, new Guid("71000000-0000-0000-0000-00000000000c"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000015"), 1, "DEMO-1-0012", "Alice's Item #12", new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 62.75m, "USD", 62.75m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000026"), new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-7/640/640", false, new Guid("72000000-0000-0000-0000-000000000007"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000016"), 1, "DEMO-2-0007", "Brian's Item #7", new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), "seed", null, 48.40m, "USD", 48.40m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000027"), new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-9/640/640", false, new Guid("73000000-0000-0000-0000-000000000009"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000017"), 1, "DEMO-3-0009", "Cecilia's Item #9", new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), "seed", null, 79.90m, "USD", 79.90m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000028"), new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-13/640/640", false, new Guid("71000000-0000-0000-0000-00000000000d"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), 1, "DEMO-1-0013", "Alice's Item #13", new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), "seed", null, 88.60m, "USD", 88.60m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000029"), new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-8/640/640", false, new Guid("72000000-0000-0000-0000-000000000008"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000019"), 1, "DEMO-2-0008", "Brian's Item #8", new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), "seed", null, 71.25m, "USD", 71.25m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002a"), new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-10/640/640", false, new Guid("73000000-0000-0000-0000-00000000000a"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), 1, "DEMO-3-0010", "Cecilia's Item #10", new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), "seed", null, 65.80m, "USD", 65.80m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002b"), new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-14/640/640", false, new Guid("71000000-0000-0000-0000-00000000000e"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), 1, "DEMO-1-0014", "Alice's Item #14", new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), "seed", null, 59.10m, "USD", 59.10m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002c"), new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-9/640/640", false, new Guid("72000000-0000-0000-0000-000000000009"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001c"), 1, "DEMO-2-0009", "Brian's Item #9", new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), "seed", null, 83.45m, "USD", 83.45m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002d"), new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-11/640/640", false, new Guid("73000000-0000-0000-0000-00000000000b"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), 1, "DEMO-3-0011", "Cecilia's Item #11", new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), "seed", null, 90.30m, "USD", 90.30m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002e"), new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-15/640/640", false, new Guid("71000000-0000-0000-0000-00000000000f"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001e"), 1, "DEMO-1-0015", "Alice's Item #15", new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), "seed", null, 74.95m, "USD", 74.95m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[,]
                {
                    { new Guid("9a7f6b12-5e2d-4d91-8c22-000000000004"), new Guid("70000000-0000-0000-0000-000000000001"), "Received the 64GB variant instead of 128GB.", new DateTime(2025, 11, 13, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 10, 10, 20, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 8, 9, 30, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), 2, 2, null, new DateTime(2025, 11, 8, 9, 30, 0, 0, DateTimeKind.Utc), "FedEx", new Guid("70000000-0000-0000-0000-000000000003"), "Exchange approved once return is in transit.", new DateTime(2025, 11, 8, 12, 45, 0, 0, DateTimeKind.Utc), 2, "612999AA10NEWRT4", new DateTime(2025, 11, 10, 10, 20, 0, 0, DateTimeKind.Utc), "seed", 80.59m, "USD" },
                    { new Guid("9a7f6b12-5e2d-4d91-8c22-000000000005"), new Guid("70000000-0000-0000-0000-000000000002"), "Decided to keep a different model instead.", new DateTime(2025, 11, 14, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 11, 8, 40, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 9, 15, 0, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 13, 16, 5, 0, 0, DateTimeKind.Utc), false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), 0, 6, null, new DateTime(2025, 11, 9, 15, 0, 0, 0, DateTimeKind.Utc), "USPS", new Guid("70000000-0000-0000-0000-000000000003"), "Refund pending inspection of returned item.", new DateTime(2025, 11, 9, 17, 15, 0, 0, DateTimeKind.Utc), 4, "9405511899223857264999", new DateTime(2025, 11, 13, 16, 5, 0, 0, DateTimeKind.Utc), "seed", 112.57m, "USD" }
                });

            migrationBuilder.CreateTable(
                name: "dispute",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    raised_by_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    reason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dispute", x => x.id);
                    table.ForeignKey(
                        name: "fk_dispute_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewer_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    reviewer_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Buyer"),
                    recipient_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recipient_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Seller"),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    reply = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    replied_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    revision_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "None"),
                    revision_requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review", x => x.id);
                    table.ForeignKey(
                        name: "fk_review_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_dispute_listing_id",
                table: "dispute",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispute_raised_by_id",
                table: "dispute",
                column: "raised_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_dispute_status",
                table: "dispute",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_review_listing_id",
                table: "review",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_review_recipient_id",
                table: "review",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_review_recipient_role",
                table: "review",
                column: "recipient_role");

            migrationBuilder.CreateIndex(
                name: "ix_review_reviewer_id",
                table: "review",
                column: "reviewer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dispute");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000004"));

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000005"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000025"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000026"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000027"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000028"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000029"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002a"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002b"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002c"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002d"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002e"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("9a7f6b12-5e2d-4d91-8c22-000000000004"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("9a7f6b12-5e2d-4d91-8c22-000000000005"));

            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9"));

            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("5a3f5769-6c6d-4b89-9347-118bd3fba3d6"));

            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("7cf6e659-8025-49e8-94d5-3a4dd3b5a793"));

            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000015"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000016"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000017"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-000000000019"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001c"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001e"));
        }
    }
}
