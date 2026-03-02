using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("964d1131-9f9c-4db8-8e6c-86fdb46f1520"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("1c41a5ab-d8cb-4525-b2cf-4188c49dd9b2"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("8fd217f6-9356-4d3e-bf68-5cb15f2a1d86"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"));

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d"),
                columns: new[] { "image_url", "listing_id", "sku", "title", "total_price_amount", "unit_price_amount" },
                values: new object[] { "https://picsum.photos/seed/1-3/640/640", new Guid("71000000-0000-0000-0000-000000000003"), "DEMO-1-0003", "Alice's Item #3", 63.98m, 31.99m });

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6cbb0f3e-9fd9-4c83-b181-74d3432fb953"),
                columns: new[] { "image_url", "listing_id", "sku", "title", "total_price_amount", "unit_price_amount" },
                values: new object[] { "https://picsum.photos/seed/1-1/640/640", new Guid("71000000-0000-0000-0000-000000000001"), "DEMO-1-0001", "Alice's Item #1", 29.99m, 29.99m });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "archived_at", "buyer_id", "cancelled_at", "coupon_code", "created_at", "created_by", "delivered_at", "fulfillment_type", "is_deleted", "order_number", "ordered_at", "paid_at", "promotion_id", "seller_id", "shipped_at", "shipping_status", "status_id", "updated_at", "updated_by", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[] { new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, "OCTDEAL", new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1002", new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 15, 15, 15, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", 5.00m, "USD", 3.35m, "USD", 12.00m, "USD", 66.98m, "USD", 5.36m, "USD", 82.69m, "USD" });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[,]
                {
                    { new Guid("3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f"), new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-4/640/640", false, new Guid("71000000-0000-0000-0000-000000000004"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "DEMO-1-0004", "Alice's Item #4", new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", null, 32.99m, "USD", 32.99m, "USD" },
                    { new Guid("a9d23977-7d99-4d44-bb79-4cff5ec2f56f"), new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-5/640/640", false, new Guid("71000000-0000-0000-0000-000000000005"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "DEMO-1-0005", "Alice's Item #5", new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", null, 33.99m, "USD", 33.99m, "USD" }
                });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"),
                columns: new[] { "buyer_id", "platform_fee_amount", "sub_total_amount", "tax_amount", "total_amount" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000002"), 4.70m, 93.97m, 7.52m, 114.69m });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "archived_at", "buyer_id", "cancelled_at", "coupon_code", "created_at", "created_by", "delivered_at", "fulfillment_type", "is_deleted", "order_number", "ordered_at", "paid_at", "promotion_id", "seller_id", "shipped_at", "shipping_status", "status_id", "updated_at", "updated_by", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[,]
                {
                    { new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, "HOLIDAY10", new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1009", new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 28, 13, 10, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 28, 22, 10, 0, 0, DateTimeKind.Utc), 2, new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930"), new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", 7.50m, "USD", 6.20m, "USD", 14.00m, "USD", 145.97m, "USD", 10.60m, "USD", 169.27m, "USD" },
                    { new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1004", new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("70000000-0000-0000-0000-000000000001"), null, 6, new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd"), new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.25m, "USD", 0.00m, "USD", 74.98m, "USD", 5.50m, "USD", 83.73m, "USD" },
                    { new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1003", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 10, 45, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new DateTime(2025, 10, 18, 10, 45, 0, 0, DateTimeKind.Utc), "seed", 2.00m, "USD", 3.40m, "USD", 9.25m, "USD", 70.98m, "USD", 6.20m, "USD", 87.83m, "USD" },
                    { new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1006", new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 24, 11, 50, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new DateTime(2025, 10, 24, 11, 50, 0, 0, DateTimeKind.Utc), "seed", 4.00m, "USD", 3.99m, "USD", 11.00m, "USD", 79.98m, "USD", 6.40m, "USD", 97.37m, "USD" },
                    { new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1008", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 26, 19, 40, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), new DateTime(2025, 10, 26, 19, 40, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.60m, "USD", 13.00m, "USD", 92.98m, "USD", 8.60m, "USD", 119.18m, "USD" },
                    { new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1007", new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 25, 17, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 26, 15, 30, 0, 0, DateTimeKind.Utc), 5, new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.20m, "USD", 12.50m, "USD", 84.98m, "USD", 7.20m, "USD", 108.88m, "USD" },
                    { new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1005", new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 22, 9, 20, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.10m, "USD", 10.00m, "USD", 76.98m, "USD", 6.16m, "USD", 97.24m, "USD" },
                    { new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, "BULKBUY", new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1010", new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 10, 5, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 30, 22, 5, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", 10.00m, "USD", 7.20m, "USD", 15.00m, "USD", 152.97m, "USD", 12.20m, "USD", 177.37m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[,]
                {
                    { new Guid("0a3e9070-0a5e-4114-8634-8e9353a5369e"), new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-9/640/640", false, new Guid("71000000-0000-0000-0000-000000000009"), new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), 1, "DEMO-1-0009", "Alice's Item #9", new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 37.99m, "USD", 37.99m, "USD" },
                    { new Guid("30f2c0f3-09bb-4f52-93a9-6e98b0171c3f"), new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-3/640/640", false, new Guid("73000000-0000-0000-0000-000000000003"), new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 1, "DEMO-3-0003", "Cecilia's Item #3", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", null, 47.99m, "USD", 47.99m, "USD" },
                    { new Guid("4a1ab1de-4a10-4326-a0be-5d3ab27c9df7"), new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-2/640/640", false, new Guid("72000000-0000-0000-0000-000000000002"), new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), 1, "DEMO-2-0002", "Brian's Item #2", new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", null, 38.99m, "USD", 38.99m, "USD" },
                    { new Guid("55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4"), new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-7/640/640", false, new Guid("73000000-0000-0000-0000-000000000007"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0007", "Cecilia's Item #7", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 51.99m, "USD", 51.99m, "USD" },
                    { new Guid("5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92"), new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-3/640/640", false, new Guid("72000000-0000-0000-0000-000000000003"), new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), 2, "DEMO-2-0003", "Brian's Item #3", new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", null, 79.98m, "USD", 39.99m, "USD" },
                    { new Guid("6bd3f47d-4f1e-467f-8797-3b2a151dd09f"), new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-4/640/640", false, new Guid("73000000-0000-0000-0000-000000000004"), new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 2, "DEMO-3-0004", "Cecilia's Item #4", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", null, 97.98m, "USD", 48.99m, "USD" },
                    { new Guid("6ccf331f-2863-411a-8f9e-1a28857e2a31"), new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-6/640/640", false, new Guid("73000000-0000-0000-0000-000000000006"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0006", "Cecilia's Item #6", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 50.99m, "USD", 50.99m, "USD" },
                    { new Guid("7fdde15f-acca-41c7-97a3-e1df2c6a4b8d"), new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-8/640/640", false, new Guid("71000000-0000-0000-0000-000000000008"), new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), 1, "DEMO-1-0008", "Alice's Item #8", new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 36.99m, "USD", 36.99m, "USD" },
                    { new Guid("8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c"), new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-1/640/640", false, new Guid("73000000-0000-0000-0000-000000000001"), new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), 1, "DEMO-3-0001", "Cecilia's Item #1", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 45.99m, "USD", 45.99m, "USD" },
                    { new Guid("9be4d720-31f2-4456-94d7-2bf0c76fa0ec"), new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-2/640/640", false, new Guid("73000000-0000-0000-0000-000000000002"), new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), 1, "DEMO-3-0002", "Cecilia's Item #2", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 46.99m, "USD", 46.99m, "USD" },
                    { new Guid("a3d8f848-7cf3-4058-9f09-3a78d4d64a5d"), new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-5/640/640", false, new Guid("72000000-0000-0000-0000-000000000005"), new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 1, "DEMO-2-0005", "Brian's Item #5", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", null, 41.99m, "USD", 41.99m, "USD" },
                    { new Guid("b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a"), new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-4/640/640", false, new Guid("72000000-0000-0000-0000-000000000004"), new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 1, "DEMO-2-0004", "Brian's Item #4", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", null, 42.99m, "USD", 42.99m, "USD" },
                    { new Guid("c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f"), new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-6/640/640", false, new Guid("71000000-0000-0000-0000-000000000006"), new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 1, "DEMO-1-0006", "Alice's Item #6", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 34.99m, "USD", 34.99m, "USD" },
                    { new Guid("e1d40241-43f4-4d93-b9ed-4ac8c9e52088"), new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-1/640/640", false, new Guid("72000000-0000-0000-0000-000000000001"), new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), 1, "DEMO-2-0001", "Brian's Item #1", new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", null, 37.99m, "USD", 37.99m, "USD" },
                    { new Guid("e9ad9da9-07b8-42ae-9ce2-764f76d4b657"), new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-5/640/640", false, new Guid("73000000-0000-0000-0000-000000000005"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0005", "Cecilia's Item #5", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 49.99m, "USD", 49.99m, "USD" },
                    { new Guid("f2a8249e-2643-49b5-bd73-0cac89fb4fc5"), new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-7/640/640", false, new Guid("71000000-0000-0000-0000-000000000007"), new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 1, "DEMO-1-0007", "Alice's Item #7", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 35.99m, "USD", 35.99m, "USD" }
                });

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_listing_listing_id",
                table: "order_items",
                column: "listing_id",
                principalTable: "listing",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_items_listing_listing_id",
                table: "order_items");

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("0a3e9070-0a5e-4114-8634-8e9353a5369e"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("30f2c0f3-09bb-4f52-93a9-6e98b0171c3f"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("4a1ab1de-4a10-4326-a0be-5d3ab27c9df7"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6bd3f47d-4f1e-467f-8797-3b2a151dd09f"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6ccf331f-2863-411a-8f9e-1a28857e2a31"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("7fdde15f-acca-41c7-97a3-e1df2c6a4b8d"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("9be4d720-31f2-4456-94d7-2bf0c76fa0ec"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("a3d8f848-7cf3-4058-9f09-3a78d4d64a5d"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("a9d23977-7d99-4d44-bb79-4cff5ec2f56f"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("e1d40241-43f4-4d93-b9ed-4ac8c9e52088"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("e9ad9da9-07b8-42ae-9ce2-764f76d4b657"));

            migrationBuilder.DeleteData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("f2a8249e-2643-49b5-bd73-0cac89fb4fc5"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"));

            migrationBuilder.DeleteData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"));

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d"),
                columns: new[] { "image_url", "listing_id", "sku", "title", "total_price_amount", "unit_price_amount" },
                values: new object[] { "https://example.com/images/strap.jpg", new Guid("c1dbcf74-221e-4e10-9cd6-c4a4060b1baa"), "SKU-ACC-004", "Camera strap pack", 45.00m, 22.50m });

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6cbb0f3e-9fd9-4c83-b181-74d3432fb953"),
                columns: new[] { "image_url", "listing_id", "sku", "title", "total_price_amount", "unit_price_amount" },
                values: new object[] { "https://example.com/images/lens.jpg", new Guid("cbebba4e-72dc-4d5d-83b7-2fdd7ecb79d9"), "SKU-VCAM-001", "Vintage camera lens", 59.99m, 59.99m });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[] { new Guid("964d1131-9f9c-4db8-8e6c-86fdb46f1520"), new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://example.com/images/tripod.jpg", false, new Guid("fbe6aa87-7114-4184-a4f5-89b2b36c27e3"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "SKU-TRI-010", "Travel tripod kit", new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), "seed", null, 89.95m, "USD", 89.95m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[] { new Guid("1c41a5ab-d8cb-4525-b2cf-4188c49dd9b2"), new Guid("70000000-0000-0000-0000-000000000001"), "Tripod arrived with scratches on the head.", new DateTime(2025, 11, 4, 11, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 27, 11, 20, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 10, 25, 11, 20, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 0, 0, null, new DateTime(2025, 10, 25, 11, 20, 0, 0, DateTimeKind.Utc), "USPS", new Guid("70000000-0000-0000-0000-000000000001"), "Awaiting item return to inspect.", new DateTime(2025, 10, 25, 17, 20, 0, 0, DateTimeKind.Utc), 2, "9400111111111111111111", new DateTime(2025, 10, 27, 11, 20, 0, 0, DateTimeKind.Utc), "seed", 107.25m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency", "refund_amount", "refund_currency", "restocking_fee_amount", "restocking_fee_currency" },
                values: new object[] { new Guid("8fd217f6-9356-4d3e-bf68-5cb15f2a1d86"), new Guid("70000000-0000-0000-0000-000000000001"), "Strap was shorter than expected.", new DateTime(2025, 10, 19, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 16, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 21, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 12, 8, 45, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 10, 20, 8, 45, 0, 0, DateTimeKind.Utc), false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 3, 4, new DateTime(2025, 10, 21, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 12, 8, 45, 0, 0, DateTimeKind.Utc), "FedEx", new Guid("70000000-0000-0000-0000-000000000001"), "Issued partial refund less restocking fee.", new DateTime(2025, 10, 12, 11, 45, 0, 0, DateTimeKind.Utc), 5, "612345678901", new DateTime(2025, 10, 21, 8, 45, 0, 0, DateTimeKind.Utc), "seed", 107.25m, "USD", 80.00m, "USD", 5.00m, "USD" });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"),
                columns: new[] { "buyer_id", "created_at", "ordered_at", "paid_at", "shipped_at", "updated_at", "platform_fee_amount", "sub_total_amount", "tax_amount", "total_amount" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 15, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 19, 2, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 19, 2, 15, 0, 0, DateTimeKind.Utc), 4.00m, 89.95m, 6.30m, 107.25m });

            migrationBuilder.UpdateData(
                table: "orders",
                keyColumn: "id",
                keyValue: new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"),
                columns: new[] { "buyer_id", "platform_fee_amount", "sub_total_amount", "tax_amount", "total_amount" },
                values: new object[] { new Guid("70000000-0000-0000-0000-000000000001"), 5.25m, 104.99m, 7.10m, 125.84m });
        }
    }
}
