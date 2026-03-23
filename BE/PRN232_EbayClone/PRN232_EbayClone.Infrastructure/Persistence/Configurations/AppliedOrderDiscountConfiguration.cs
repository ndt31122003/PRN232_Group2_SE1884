using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Infrastructure.Persistence.Repositories;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class AppliedOrderDiscountConfiguration : IEntityTypeConfiguration<AppliedOrderDiscount>
{
    public void Configure(EntityTypeBuilder<AppliedOrderDiscount> builder)
    {
        builder.ToTable("applied_order_discounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(a => a.OrderDiscountId)
            .HasColumnName("order_discount_id")
            .IsRequired();

        builder.Property(a => a.DiscountAmount)
            .HasColumnName("discount_amount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(a => a.AppliedTierId)
            .HasColumnName("applied_tier_id");

        builder.Property(a => a.AppliedAt)
            .HasColumnName("applied_at")
            .IsRequired();

        builder.HasIndex(a => a.OrderId)
            .HasDatabaseName("ix_applied_order_discounts_order_id");

        builder.HasIndex(a => a.OrderDiscountId)
            .HasDatabaseName("ix_applied_order_discounts_order_discount_id");
    }
}
