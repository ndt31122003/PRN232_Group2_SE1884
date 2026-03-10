using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;
using System.Text.Json;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class FixedPriceListingConfiguration : IEntityTypeConfiguration<FixedPriceListing>
{
    public void Configure(EntityTypeBuilder<FixedPriceListing> builder)
    {
        builder.OwnsOne(f => f.Pricing, nav =>
        {
            nav.HasData(DemoSeedData.FixedPriceListingPricing);
        });

        builder.OwnsOne(f => f.OfferSettings, nav =>
        {
            nav.HasData(DemoSeedData.FixedPriceOfferSettings);
        });

        // Variations
        builder
            .OwnsMany(f => f.Variations, v =>
            {
                v.ToTable("variation");
                v.WithOwner().HasForeignKey("ListingId");

                v.Property<Guid>("ListingId").HasColumnName("listing_id");

                v.HasKey(vr => vr.Id);
                v.Property(vr => vr.Id).ValueGeneratedOnAdd().HasColumnName("id");


                v.Property(vr => vr.Sku).HasColumnName("sku");
                v.Property(vr => vr.Price).HasColumnName("price");
                v.Property(vr => vr.Quantity).HasColumnName("quantity");

                v.Property(vr => vr.VariationSpecifics)
                    .HasColumnName("specifics")
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v ?? new List<VariationSpecific>(), (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<List<VariationSpecific>>(v, (JsonSerializerOptions)null!)!
                    );

                v.Property(vr => vr.Images)
                    .HasColumnName("images")
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v ?? new List<VariationImage>(), (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<List<VariationImage>>(v, (JsonSerializerOptions)null!)!
                    );

                v.HasIndex("ListingId")
                    .HasDatabaseName("idx_variation_listing_id");
            });

        builder.HasData(DemoSeedData.FixedPriceListings);
    }
}
