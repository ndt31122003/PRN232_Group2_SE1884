using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OrderDiscountTierConfiguration : IEntityTypeConfiguration<OrderDiscountTier>
{
    public void Configure(EntityTypeBuilder<OrderDiscountTier> builder)
    {
        builder.ToTable("order_discount_tiers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("id");

        builder.Property(t => t.OrderDiscountId)
            .HasColumnName("order_discount_id")
            .IsRequired();

        builder.Property(t => t.ThresholdValue)
            .HasColumnName("threshold_value")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.DiscountValue)
            .HasColumnName("discount_value")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(t => t.TierOrder)
            .HasColumnName("tier_order")
            .IsRequired();

        builder.HasIndex(t => t.OrderDiscountId)
            .HasDatabaseName("ix_order_discount_tiers_order_discount_id");
    }
}
