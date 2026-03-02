using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addCancellationsAndReturnRequest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("0d5a8f1d-3c95-4dba-9d59-5d0f41ec134a"));

            migrationBuilder.DeleteData(
                table: "order_cancellation_requests",
                keyColumn: "id",
                keyValue: new Guid("da7790f6-4c70-40ec-9fb4-1f1a3d41d3a9"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency" },
                values: new object[] { new Guid("0d5a8f1d-3c95-4dba-9d59-5d0f41ec134a"), null, new Guid("70000000-0000-0000-0000-000000000001"), "Accidentally ordered the wrong lens.", null, new DateTime(2025, 10, 19, 15, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, new DateTime(2025, 10, 19, 15, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 22, 15, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 10, 19, 15, 0, 0, 0, DateTimeKind.Utc), "seed", 107.25m, "USD" });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "id", "auto_closed_at", "buyer_id", "buyer_note", "completed_at", "created_at", "created_by", "initiated_by", "is_deleted", "order_id", "reason", "requested_at", "seller_id", "seller_note", "seller_responded_at", "seller_response_deadline_utc", "status", "updated_at", "updated_by", "order_total_amount", "order_total_currency", "refund_amount", "refund_currency" },
                values: new object[] { new Guid("da7790f6-4c70-40ec-9fb4-1f1a3d41d3a9"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, new DateTime(2025, 10, 18, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 17, 9, 30, 0, 0, DateTimeKind.Utc), "seed", 1, false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 3, new DateTime(2025, 10, 17, 9, 30, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), "Buyer asked to ship to a different address not supported.", new DateTime(2025, 10, 17, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 19, 9, 30, 0, 0, DateTimeKind.Utc), 3, new DateTime(2025, 10, 18, 9, 30, 0, 0, DateTimeKind.Utc), "seed", 107.25m, "USD", 107.25m, "USD" });
        }
    }
}
