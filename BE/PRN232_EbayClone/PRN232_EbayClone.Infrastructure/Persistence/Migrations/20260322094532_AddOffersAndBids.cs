using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOffersAndBids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bids",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bidder_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bids", x => x.id);
                    table.ForeignKey(
                        name: "fk_bids_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "offers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    buyer_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_offers", x => x.id);
                    table.ForeignKey(
                        name: "fk_offers_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bids_bidder_id",
                table: "bids",
                column: "bidder_id");

            migrationBuilder.CreateIndex(
                name: "ix_bids_listing_id",
                table: "bids",
                column: "listing_id");

            migrationBuilder.CreateIndex(
                name: "ix_offers_buyer_id",
                table: "offers",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "ix_offers_listing_id",
                table: "offers",
                column: "listing_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bids");

            migrationBuilder.DropTable(
                name: "offers");
        }
    }
}
