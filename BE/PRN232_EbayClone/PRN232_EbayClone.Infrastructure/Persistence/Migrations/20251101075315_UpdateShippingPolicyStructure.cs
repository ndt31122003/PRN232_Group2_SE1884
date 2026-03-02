using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShippingPolicyStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "shipping_policy");

            migrationBuilder.DropColumn(
                name: "shipping_service_id",
                table: "shipping_policy");

            migrationBuilder.AddColumn<string>(
                name: "carrier",
                table: "shipping_policy",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "service_name",
                table: "shipping_policy",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "carrier",
                table: "shipping_policy");

            migrationBuilder.DropColumn(
                name: "service_name",
                table: "shipping_policy");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "shipping_policy",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "shipping_service_id",
                table: "shipping_policy",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
