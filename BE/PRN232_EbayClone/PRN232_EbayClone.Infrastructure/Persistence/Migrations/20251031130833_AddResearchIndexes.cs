using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResearchIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_variation_listing_id",
                table: "variation",
                newName: "idx_variation_listing_id");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.CreateIndex(
                name: "idx_order_items_listing_id",
                table: "order_items",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "idx_listing_active_owner_sort",
                table: "listing",
                columns: new[] { "created_by", "start_date", "created_at", "id", "category_id", "format" },
                descending: new[] { false, true, true, false, false, false },
                filter: "status = 3");

            migrationBuilder.CreateIndex(
                name: "idx_listing_owner_status",
                table: "listing",
                columns: new[] { "created_by", "status" });

            migrationBuilder.CreateIndex(
                name: "idx_listing_sku_trgm",
                table: "listing",
                column: "sku")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "idx_listing_title_trgm",
                table: "listing",
                column: "title")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_order_items_listing_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "idx_listing_active_owner_sort",
                table: "listing");

            migrationBuilder.DropIndex(
                name: "idx_listing_owner_status",
                table: "listing");

            migrationBuilder.DropIndex(
                name: "idx_listing_sku_trgm",
                table: "listing");

            migrationBuilder.DropIndex(
                name: "idx_listing_title_trgm",
                table: "listing");

            migrationBuilder.RenameIndex(
                name: "idx_variation_listing_id",
                table: "variation",
                newName: "ix_variation_listing_id");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:pg_trgm", ",,");
        }
    }
}
