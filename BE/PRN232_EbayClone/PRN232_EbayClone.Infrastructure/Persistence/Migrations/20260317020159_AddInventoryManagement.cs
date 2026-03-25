using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_quantity = table.Column<int>(type: "integer", nullable: false),
                    available_quantity = table.Column<int>(type: "integer", nullable: false),
                    reserved_quantity = table.Column<int>(type: "integer", nullable: false),
                    sold_quantity = table.Column<int>(type: "integer", nullable: false),
                    threshold_quantity = table.Column<int>(type: "integer", nullable: true),
                    is_low_stock = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_low_stock_notification_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_adjustment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    adjustment_type = table.Column<byte>(type: "smallint", nullable: false),
                    quantity_change = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    adjusted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    adjusted_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory_adjustment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventory_reservation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: true),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservation_type = table.Column<byte>(type: "smallint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    reserved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    released_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    committed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory_reservation", x => x.id);
                    table.ForeignKey(
                        name: "fk_inventory_reservation_inventory_inventory_id",
                        column: x => x.inventory_id,
                        principalTable: "inventory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_inventory_is_low_stock",
                table: "inventory",
                columns: new[] { "seller_id", "is_low_stock" });

            migrationBuilder.CreateIndex(
                name: "idx_inventory_seller_id",
                table: "inventory",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "idx_inventory_updated_at",
                table: "inventory",
                column: "last_updated_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "uk_inventory_listing_id",
                table: "inventory",
                column: "listing_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_inventory_adjustment_inventory_id",
                table: "inventory_adjustment",
                column: "inventory_id");

            migrationBuilder.CreateIndex(
                name: "idx_inventory_reservation_expires_at",
                table: "inventory_reservation",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_inventory_reservation_inventory_id",
                table: "inventory_reservation",
                column: "inventory_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory_adjustment");

            migrationBuilder.DropTable(
                name: "inventory_reservation");

            migrationBuilder.DropTable(
                name: "inventory");
        }
    }
}
