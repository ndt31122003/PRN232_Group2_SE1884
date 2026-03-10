using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        builder.HasDiscriminator<ListingFormat>("ListingFormat")
            .HasValue<FixedPriceListing>(ListingFormat.FixedPrice)
            .HasValue<AuctionListing>(ListingFormat.Auction);

        var stringListComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IEnumerable<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList().AsReadOnly());

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
                
                .Metadata.SetValueComparer(stringListComparer);
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
            .HasFilter("\"Status\" = 3");

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
