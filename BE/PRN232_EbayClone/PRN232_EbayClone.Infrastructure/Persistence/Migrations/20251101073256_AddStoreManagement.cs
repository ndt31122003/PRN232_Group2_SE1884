using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "return_policy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    accept_returns = table.Column<bool>(type: "boolean", nullable: false),
                    return_period_days = table.Column<int>(type: "integer", nullable: true),
                    refund_method = table.Column<int>(type: "integer", nullable: true),
                    return_shipping_paid_by = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_return_policy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_policy",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipping_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    cost_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    cost_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    handling_time_days = table.Column<int>(type: "integer", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipping_policy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    banner_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    store_type = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_store", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "store_subscription",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subscription_type = table.Column<int>(type: "integer", nullable: false),
                    monthly_fee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    monthly_fee_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    final_value_fee_percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    listing_limit = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_store_subscription", x => x.id);
                    table.ForeignKey(
                        name: "fk_store_subscription_store_store_id",
                        column: x => x.store_id,
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_return_policy_store_id",
                table: "return_policy",
                column: "store_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_shipping_policy_store_id",
                table: "shipping_policy",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "idx_store_slug",
                table: "store",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_store_user_id",
                table: "store",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_store_subscription_store_id",
                table: "store_subscription",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "idx_store_subscription_store_status",
                table: "store_subscription",
                columns: new[] { "store_id", "status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "return_policy");

            migrationBuilder.DropTable(
                name: "shipping_policy");

            migrationBuilder.DropTable(
                name: "store_subscription");

            migrationBuilder.DropTable(
                name: "store");
        }
    }
}
