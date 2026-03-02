using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addShippingServiceSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimated_delivery_time",
                table: "shipping_services");

            migrationBuilder.AddColumn<string>(
                name: "coverage_description",
                table: "shipping_services",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "shipping_services",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "shipping_services",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "delivery_window_label",
                table: "shipping_services",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "shipping_services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "max_estimated_delivery_days",
                table: "shipping_services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "min_estimated_delivery_days",
                table: "shipping_services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "notes",
                table: "shipping_services",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "printer_required",
                table: "shipping_services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "savings_description",
                table: "shipping_services",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "service_code",
                table: "shipping_services",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "shipping_services",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "supports_qr_code",
                table: "shipping_services",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "shipping_services",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "shipping_services",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "id", "carrier", "coverage_description", "created_at", "created_by", "delivery_window_label", "is_deleted", "max_estimated_delivery_days", "min_estimated_delivery_days", "notes", "printer_required", "savings_description", "service_code", "service_name", "slug", "updated_at", "updated_by", "base_cost_amount", "base_cost_currency" },
                values: new object[] { new Guid("5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec"), "UPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 28 - Apr 2", false, 6, 3, "Reliable ground service - Includes tracking", true, "On eBay you save 21%", "UPS_GROUND", "UPS Ground", "ups-ground", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 15.62m, "USD" });

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "id", "carrier", "coverage_description", "created_at", "created_by", "delivery_window_label", "is_deleted", "max_estimated_delivery_days", "min_estimated_delivery_days", "notes", "savings_description", "service_code", "service_name", "slug", "supports_qr_code", "updated_at", "updated_by", "base_cost_amount", "base_cost_currency" },
                values: new object[] { new Guid("6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 28 - Apr 1", false, 5, 3, "Max weight 70 lb - Max dimensions 130\" (length + girth)", "On eBay you save 28%", "USPS_GROUND_ADVANTAGE", "USPS Ground Advantage", "usps-ground", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 11.45m, "USD" });

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "id", "carrier", "coverage_description", "created_at", "created_by", "delivery_window_label", "is_deleted", "max_estimated_delivery_days", "min_estimated_delivery_days", "notes", "printer_required", "savings_description", "service_code", "service_name", "slug", "updated_at", "updated_by", "base_cost_amount", "base_cost_currency" },
                values: new object[,]
                {
                    { new Guid("9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d"), "FedEx", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 29 - Apr 3", false, 7, 4, "2-5 business days - Ideal for small parcels", true, "On eBay you save 18%", "FEDEX_GROUND_ECONOMY", "FedEx Ground Economy", "fedex-ground", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 14.10m, "USD" },
                    { new Guid("a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 27 - 31", false, 4, 2, "Legal-size documents - Insured up to $100", true, "On eBay you save 12%", "USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE", "USPS Priority Mail Flat Rate Legal Envelope", "usps-priority-legal", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 9.05m, "USD" },
                    { new Guid("c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 27 - 31", false, 4, 2, "Best for documents - Includes tracking", true, "On eBay you save 13%", "USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE", "USPS Priority Mail Flat Rate Envelope", "usps-priority-flat", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 8.75m, "USD" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_shipping_services_slug",
                table: "shipping_services",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_shipping_services_slug",
                table: "shipping_services");

            migrationBuilder.DeleteData(
                table: "shipping_services",
                keyColumn: "id",
                keyValue: new Guid("5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec"));

            migrationBuilder.DeleteData(
                table: "shipping_services",
                keyColumn: "id",
                keyValue: new Guid("6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950"));

            migrationBuilder.DeleteData(
                table: "shipping_services",
                keyColumn: "id",
                keyValue: new Guid("9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d"));

            migrationBuilder.DeleteData(
                table: "shipping_services",
                keyColumn: "id",
                keyValue: new Guid("a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7"));

            migrationBuilder.DeleteData(
                table: "shipping_services",
                keyColumn: "id",
                keyValue: new Guid("c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51"));

            migrationBuilder.DropColumn(
                name: "coverage_description",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "delivery_window_label",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "max_estimated_delivery_days",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "min_estimated_delivery_days",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "notes",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "printer_required",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "savings_description",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "service_code",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "supports_qr_code",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "shipping_services");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "shipping_services");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "estimated_delivery_time",
                table: "shipping_services",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
