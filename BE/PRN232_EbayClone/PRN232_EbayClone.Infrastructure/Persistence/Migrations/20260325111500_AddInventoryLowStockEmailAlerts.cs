using Microsoft.EntityFrameworkCore.Migrations;

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations;

public partial class AddInventoryLowStockEmailAlerts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "low_stock_email_enabled",
            table: "inventory",
            type: "boolean",
            nullable: false,
            defaultValue: false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "low_stock_email_enabled",
            table: "inventory");
    }
}