using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("listing");

        builder.HasKey(l => l.Id);

        // TPH discriminator
        builder.HasDiscriminator<ListingFormat>("listing_format")
            .HasValue<FixedPriceListing>(ListingFormat.FixedPrice)
            .HasValue<AuctionListing>(ListingFormat.Auction);

        // ItemSpecifics: lưu JSON
        builder.OwnsMany(l => l.ItemSpecifics, b =>
        {
            b.ToTable("listing_item_specific");
            b.WithOwner().HasForeignKey("listing_id");

            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.Values)
                .HasConversion(
                    v => string.Join(";", v),       // string[] -> string
                    v => v.Split(";", StringSplitOptions.RemoveEmptyEntries)) // string -> string[]
                .HasColumnName("val");
        });

        // ListingImages
        builder.OwnsMany(l => l.Images, b =>
        {
            b.ToTable("listing_image");
            b.WithOwner().HasForeignKey("listing_id");
            b.Property(x => x.Url).IsRequired();
            b.Property(x => x.IsPrimary).IsRequired();

            b.HasData(DemoSeedData.ListingImages);
        });

        builder
            .HasQueryFilter(l => !l.IsDeleted);

        builder.HasIndex(l => new { l.CreatedBy, l.Status })
            .HasDatabaseName("idx_listing_owner_status");

        builder.HasIndex(l => new
            {
                l.CreatedBy,
                l.StartDate,
                l.CreatedAt,
                l.Id,
                l.CategoryId,
                l.Format
            })
            .HasDatabaseName("idx_listing_active_owner_sort")
            .IsDescending(false, true, true, false, false, false)
            .HasFilter("status = 3");

        builder.HasIndex(l => l.Title)
            .HasDatabaseName("idx_listing_title_trgm")
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops");

        builder.HasIndex(l => l.Sku)
            .HasDatabaseName("idx_listing_sku_trgm")
            .HasMethod("gin")
            .HasOperators("gin_trgm_ops");
    }
}
