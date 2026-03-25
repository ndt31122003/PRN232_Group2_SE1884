using Microsoft.EntityFrameworkCore.Migrations;

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations;

public partial class AddInventoryAdditionalLowStockEmails : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "additional_low_stock_emails",
            table: "inventory",
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: false,
            defaultValue: string.Empty);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "additional_low_stock_emails",
            table: "inventory");
    }
}