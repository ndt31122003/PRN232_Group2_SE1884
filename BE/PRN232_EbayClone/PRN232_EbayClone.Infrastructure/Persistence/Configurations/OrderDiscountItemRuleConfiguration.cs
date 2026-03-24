using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OrderDiscountItemRuleConfiguration : IEntityTypeConfiguration<OrderDiscountItemRule>
{
    public void Configure(EntityTypeBuilder<OrderDiscountItemRule> builder)
    {
        builder.ToTable("order_discount_item_rules");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasColumnName("id");

        builder.Property(r => r.OrderDiscountId)
            .HasColumnName("order_discount_id")
            .IsRequired();

        builder.Property(r => r.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(r => r.IsExclusion)
            .HasColumnName("is_exclusion")
            .IsRequired();

        builder.HasIndex(r => r.OrderDiscountId)
            .HasDatabaseName("ix_order_discount_item_rules_order_discount_id");

        builder.HasIndex(r => r.ListingId)
            .HasDatabaseName("ix_order_discount_item_rules_listing_id");
    }
}
