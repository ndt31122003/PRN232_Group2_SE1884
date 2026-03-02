using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemShipments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "order_shipping_labels",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "order_shipping_labels",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "order_item_shipments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipping_label_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tracking_number = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    carrier = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    shipped_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_item_shipments", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_item_shipments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_item_shipments_shipping_labels_shipping_label_id",
                        column: x => x.shipping_label_id,
                        principalTable: "order_shipping_labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[,]
                {
                    { new Guid("6f1f9f0c-898f-4c7b-bb38-1b689e9f7331"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Realized I ordered the wrong variation, please cancel.", null, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), "seed", 114.69m, "USD" },
                    { new Guid("c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753"), new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), null, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), "seed", 2, false, new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), 99, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), "Order auto-cancelled after missing shipping deadline.", null, null, 5, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), "seed", 97.37m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency", "refund_amount", "refund_currency" },
                values: new object[] { new Guid("d3f7d907-6b71-47d8-8651-922629540277"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Need to update the delivery address; requesting cancellation.", null, new DateTime(2025, 10, 19, 12, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 3, new DateTime(2025, 10, 19, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), "Approved – refund processing with payment provider.", new DateTime(2025, 10, 19, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 19, 18, 0, 0, 0, DateTimeKind.Utc), "seed", 87.83m, "USD", 87.83m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[] { new Guid("8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf"), new Guid("70000000-0000-0000-0000-000000000001"), "Item color differs from the listing photos.", null, null, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 0, 0, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, 0, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), "seed", 108.88m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency", "refund_amount", "refund_currency", "restocking_fee_amount", "restocking_fee_currency" },
                values: new object[] { new Guid("dc3329e1-14fb-4d00-a395-e76e25a6822b"), new Guid("70000000-0000-0000-0000-000000000002"), "Shoes run smaller than expected.", new DateTime(2025, 11, 9, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 9, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 17, 45, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 8, 16, 20, 0, 0, DateTimeKind.Utc), false, new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 3, 4, new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 17, 45, 0, 0, DateTimeKind.Utc), "USPS", new Guid("70000000-0000-0000-0000-000000000003"), "Refunded minus restocking fee.", new DateTime(2025, 11, 4, 20, 30, 0, 0, DateTimeKind.Utc), 5, "9405511899223857264837", new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), "seed", 177.37m, "USD", 150.00m, "USD", 5.00m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "id", "buyer_id", "buyer_note", "buyer_return_due_at", "buyer_shipped_at", "closed_at", "created_at", "created_by", "delivered_at", "is_deleted", "order_id", "preferred_resolution", "reason", "refund_issued_at", "requested_at", "return_carrier", "seller_id", "seller_note", "seller_responded_at", "status", "tracking_number", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[] { new Guid("fd21bed5-6c0c-4bcf-b099-31c8b0d08f27"), new Guid("70000000-0000-0000-0000-000000000001"), "Screen arrived cracked; requesting replacement.", new DateTime(2025, 11, 5, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 3, 10, 15, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 1, 1, null, new DateTime(2025, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "UPS", new Guid("70000000-0000-0000-0000-000000000003"), "Please return using the provided UPS label.", new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, "1Z999AA10123456784", new DateTime(2025, 11, 3, 10, 15, 0, 0, DateTimeKind.Utc), "seed", 169.27m, "USD" });

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_order_id",
                table: "order_item_shipments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_order_item_id",
                table: "order_item_shipments",
                column: "order_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_shipping_label_id",
                table: "order_item_shipments",
                column: "shipping_label_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_item_shipments");

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("6f1f9f0c-898f-4c7b-bb38-1b689e9f7331"));

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753"));

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("d3f7d907-6b71-47d8-8651-922629540277"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("dc3329e1-14fb-4d00-a395-e76e25a6822b"));

            migrationBuilder.DeleteData(
                table: "order_return_requests",
                keyColumn: "id",
                keyValue: new Guid("fd21bed5-6c0c-4bcf-b099-31c8b0d08f27"));

            migrationBuilder.AlterColumn<Guid>(
                name: "order_id",
                table: "order_shipping_labels",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "order_shipping_labels",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");
        }
    }
}
