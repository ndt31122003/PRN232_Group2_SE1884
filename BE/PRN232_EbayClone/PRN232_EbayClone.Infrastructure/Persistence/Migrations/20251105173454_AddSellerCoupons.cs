using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerCoupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "coupon_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coupon_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coupon",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    coupon_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    discount_value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    discount_unit = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    max_discount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    usage_limit = table.Column<int>(type: "integer", nullable: true),
                    usage_per_user = table.Column<int>(type: "integer", nullable: true),
                    minimum_order_value = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    applicable_price_min = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    applicable_price_max = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coupon", x => x.id);
                    table.ForeignKey(
                        name: "fk_coupon_coupon_types_coupon_type_id",
                        column: x => x.coupon_type_id,
                        principalTable: "coupon_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "coupon_condition",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    coupon_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buy_quantity = table.Column<int>(type: "integer", nullable: true),
                    get_quantity = table.Column<int>(type: "integer", nullable: true),
                    get_discount_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    save_every_amount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    save_every_items = table.Column<int>(type: "integer", nullable: true),
                    condition_description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coupon_condition", x => x.id);
                    table.ForeignKey(
                        name: "fk_coupon_condition_coupons_coupon_id",
                        column: x => x.coupon_id,
                        principalTable: "coupon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "coupon_type",
                columns: new[] { "id", "created_at", "created_by", "description", "is_active", "name", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { new Guid("0d0c32fe-349c-4857-b20a-2d3f8db91ed4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount when buying Y or more items", true, "Extra % off Y or more items", null, null },
                    { new Guid("2c5a6a6a-fe7e-4813-a134-70572b5ab90a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount when total order value reaches a minimum value", true, "Extra % off $ or more", null, null },
                    { new Guid("3b980145-62b6-4ae6-9cf8-7838bc7b84e0"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Save a fixed amount for every X items purchased", true, "Save $ for every X items", null, null },
                    { new Guid("51f2ed38-06bb-496e-b5cb-7aa3057c21b7"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Save a fixed amount for every dollar spent", true, "Save $ for every $ spent", null, null },
                    { new Guid("773f8d9b-eb8e-4ff4-a21e-4bb2fa5407f4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount when buying X or more items", true, "Extra $ off X or more items", null, null },
                    { new Guid("7a5a0b7a-ed8f-4b91-a7c3-59e5363b76f3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount for each item purchased", true, "Extra $ off each item", null, null },
                    { new Guid("7eaa19cf-6b36-4a1c-b7b5-a9abcb7eeff2"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Buy X items and get Y items at a percentage discount", true, "Buy X get Y at % off", null, null },
                    { new Guid("990c28b3-753e-41b1-a798-965cf46b7dcd"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount on all eligible items", true, "Extra $ off", null, null },
                    { new Guid("9e1d4ea5-5b09-48be-be90-e2790f6ba537"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount on all eligible items", true, "Extra % off", null, null },
                    { new Guid("cfa2e0f1-b720-4590-a7d4-4ce0844f9671"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount when order value reaches a minimum threshold", true, "Extra $ off $ or more", null, null },
                    { new Guid("ed9d5151-6f8c-4628-a5a9-4c24867e5673"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Buy X items and get Y items for free", true, "Buy X get Y free", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_coupon_coupon_type_id",
                table: "coupon",
                column: "coupon_type_id");

            migrationBuilder.CreateIndex(
                name: "ux_coupon_code",
                table: "coupon",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_coupon_condition_coupon_id",
                table: "coupon_condition",
                column: "coupon_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coupon_condition");

            migrationBuilder.DropTable(
                name: "coupon");

            migrationBuilder.DropTable(
                name: "coupon_type");
        }
    }
}
