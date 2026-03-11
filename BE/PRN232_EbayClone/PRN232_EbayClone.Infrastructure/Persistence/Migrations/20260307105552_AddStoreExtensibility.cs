using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreExtensibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contact_email",
                table: "store",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "contact_phone",
                table: "store",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "layout_config",
                table: "store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_links",
                table: "store",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "theme_color",
                table: "store",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contact_email",
                table: "store");

            migrationBuilder.DropColumn(
                name: "contact_phone",
                table: "store");

            migrationBuilder.DropColumn(
                name: "layout_config",
                table: "store");

            migrationBuilder.DropColumn(
                name: "social_links",
                table: "store");

            migrationBuilder.DropColumn(
                name: "theme_color",
                table: "store");
        }
    }
}
