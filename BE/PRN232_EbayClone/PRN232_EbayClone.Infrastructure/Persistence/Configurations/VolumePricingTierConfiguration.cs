using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class VolumePricingTierConfiguration : IEntityTypeConfiguration<VolumePricingTier>
{
    public void Configure(EntityTypeBuilder<VolumePricingTier> builder)
    {
        builder.ToTable("volume_pricing_tiers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("id");

        builder.Property(t => t.VolumePricingId)
            .HasColumnName("volume_pricing_id")
            .IsRequired();

        builder.Property(t => t.MinQuantity)
            .HasColumnName("min_quantity")
            .IsRequired();

        builder.Property(t => t.DiscountValue)
            .HasColumnName("discount_value")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.DiscountUnit)
            .HasColumnName("discount_unit")
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(t => t.VolumePricingId)
            .HasDatabaseName("ix_volume_pricing_tiers_volume_pricing_id");
    }
}
