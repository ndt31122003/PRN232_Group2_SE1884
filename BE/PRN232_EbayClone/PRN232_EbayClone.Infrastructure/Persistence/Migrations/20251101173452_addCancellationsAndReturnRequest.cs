using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addCancellationsAndReturnRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("1b9c7e7f-9d15-41b0-9417-51c5723a7792"));

            migrationBuilder.DeleteData(
                table: "order_status_transitions",
                keyColumn: "id",
                keyValue: new Guid("70e8c0f9-6fa8-4ff6-bb3f-5b53b22e2afd"));

            migrationBuilder.DeleteData(
                table: "order_statuses",
                keyColumn: "id",
                keyValue: new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb"));

            migrationBuilder.AddColumn<bool>(
                name: "is_voided",
                table: "order_shipping_labels",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "void_reason",
                table: "order_shipping_labels",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "voided_at",
                table: "order_shipping_labels",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "order_cancellation_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    initiated_by = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    buyer_note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    seller_note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    seller_response_deadline_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    seller_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    auto_closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    refund_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    refund_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    order_total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    order_total_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_cancellation_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_cancellation_requests_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_return_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    preferred_resolution = table.Column<int>(type: "integer", nullable: false),
                    buyer_note = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    seller_note = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    requested_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    seller_responded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    buyer_return_due_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    buyer_shipped_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    refund_issued_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    return_carrier = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    tracking_number = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    refund_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    refund_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    restocking_fee_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    restocking_fee_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    order_total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    order_total_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_return_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_return_requests_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Intentionally no seed data for cancellation requests; keep table empty to avoid FK conflicts.


            migrationBuilder.CreateIndex(
                name: "idx_cancellation_requests_order",
                table: "order_cancellation_requests",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "idx_cancellation_requests_status",
                table: "order_cancellation_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_return_requests_order",
                table: "order_return_requests",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "idx_return_requests_status",
                table: "order_return_requests",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_cancellation_requests");

            migrationBuilder.DropTable(
                name: "order_return_requests");

            migrationBuilder.DropColumn(
                name: "is_voided",
                table: "order_shipping_labels");

            migrationBuilder.DropColumn(
                name: "void_reason",
                table: "order_shipping_labels");

            migrationBuilder.DropColumn(
                name: "voided_at",
                table: "order_shipping_labels");

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "id", "code", "color", "description", "name", "sort_order" },
                values: new object[] { new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb"), "Delivered", "#0ea5e9", "Package delivered to buyer", "Delivered", 9 });

            migrationBuilder.InsertData(
                table: "order_status_transitions",
                columns: new[] { "id", "allowed_roles", "from_status_id", "to_status_id" },
                values: new object[,]
                {
                    { new Guid("1b9c7e7f-9d15-41b0-9417-51c5723a7792"), "SELLER,SYSTEM", new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb") },
                    { new Guid("70e8c0f9-6fa8-4ff6-bb3f-5b53b22e2afd"), "SYSTEM", new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb"), new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930") }
                });
        }
    }
}
