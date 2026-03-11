using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSellerVerificationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "business_city",
                table: "user",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_country",
                table: "user",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_name",
                table: "user",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_state",
                table: "user",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_street",
                table: "user",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "business_zip_code",
                table: "user",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_business_verified",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_phone_verified",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "user",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000001"),
                columns: new[] { "business_name", "is_business_verified", "is_phone_verified", "phone_number" },
                values: new object[] { null, false, false, null });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000002"),
                columns: new[] { "business_name", "is_business_verified", "is_phone_verified", "phone_number" },
                values: new object[] { null, false, false, null });

            migrationBuilder.UpdateData(
                table: "user",
                keyColumn: "id",
                keyValue: new Guid("70000000-0000-0000-0000-000000000003"),
                columns: new[] { "business_name", "is_business_verified", "is_phone_verified", "phone_number" },
                values: new object[] { null, false, false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "business_city",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_country",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_name",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_state",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_street",
                table: "user");

            migrationBuilder.DropColumn(
                name: "business_zip_code",
                table: "user");

            migrationBuilder.DropColumn(
                name: "is_business_verified",
                table: "user");

            migrationBuilder.DropColumn(
                name: "is_phone_verified",
                table: "user");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "user");
        }
    }
}
