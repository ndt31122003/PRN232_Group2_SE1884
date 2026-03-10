using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        var variationSpecificsComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IReadOnlyCollection<VariationSpecific>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList().AsReadOnly());

        var variationImagesComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IReadOnlyCollection<VariationImage>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList().AsReadOnly());

        builder
            .OwnsMany(f => f.Variations, v =>
            {
                v.ToTable("variation");
                v.WithOwner().HasForeignKey("ListingId");

                v.Property<Guid>("ListingId");

                v.HasKey(vr => vr.Id);
                v.Property(vr => vr.Id).ValueGeneratedOnAdd();


                v.Property(vr => vr.Sku);
                v.Property(vr => vr.Price);
                v.Property(vr => vr.Quantity);

                v.Property(vr => vr.VariationSpecifics)
                    
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v ?? new List<VariationSpecific>(), (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<List<VariationSpecific>>(v, (JsonSerializerOptions)null!)!
                    )
                    .Metadata.SetValueComparer(variationSpecificsComparer);

                v.Property(vr => vr.Images)
                    
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v ?? new List<VariationImage>(), (JsonSerializerOptions)null!),
                        v => JsonSerializer.Deserialize<List<VariationImage>>(v, (JsonSerializerOptions)null!)!
                    )
                    .Metadata.SetValueComparer(variationImagesComparer);

                v.HasIndex("ListingId")
                    .HasDatabaseName("idx_variation_listing_id");
            });

        builder.HasData(DemoSeedData.FixedPriceListings);
    }
}
