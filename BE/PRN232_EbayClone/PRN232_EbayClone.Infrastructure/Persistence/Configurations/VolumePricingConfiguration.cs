using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class VolumePricingConfiguration : IEntityTypeConfiguration<VolumePricing>
{
    public void Configure(EntityTypeBuilder<VolumePricing> builder)
    {
        builder.ToTable("volume_pricings");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id).HasColumnName("id");

        var sellerIdConverter = new ValueConverter<UserId, Guid>(
            id => id.Value,
            value => new UserId(value));

        builder.Property(v => v.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(sellerIdConverter)
            .IsRequired();

        builder.Property(v => v.ListingId)
            .HasColumnName("listing_id");

        builder.Property(v => v.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(v => v.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(v => v.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(v => v.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(v => v.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasMany(v => v.Tiers)
            .WithOne()
            .HasForeignKey("VolumePricingId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.SellerId)
            .HasDatabaseName("ix_volume_pricings_seller_id");

        builder.HasIndex(v => v.ListingId)
            .HasDatabaseName("ix_volume_pricings_listing_id");

        builder.HasIndex(v => new { v.IsActive, v.StartDate, v.EndDate })
            .HasDatabaseName("ix_volume_pricings_active");
    }
}
