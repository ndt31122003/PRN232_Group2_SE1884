using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OrderDiscountConfiguration : IEntityTypeConfiguration<OrderDiscount>
{
    public void Configure(EntityTypeBuilder<OrderDiscount> builder)
    {
        builder.ToTable("order_discounts");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id).HasColumnName("id");

        var sellerIdConverter = new ValueConverter<UserId, Guid>(
            id => id.Value,
            value => new UserId(value));

        builder.Property(d => d.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(sellerIdConverter)
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(d => d.ThresholdType)
            .HasColumnName("threshold_type")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.ThresholdAmount)
            .HasColumnName("threshold_amount")
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.ThresholdQuantity)
            .HasColumnName("threshold_quantity");

        builder.Property(d => d.DiscountValue)
            .HasColumnName("discount_value")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.DiscountUnit)
            .HasColumnName("discount_unit")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.MaxDiscount)
            .HasColumnName("max_discount")
            .HasColumnType("decimal(18,2)");

        builder.Property(d => d.ApplyToAllItems)
            .HasColumnName("apply_to_all_items")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(d => d.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(d => d.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(d => d.SellerId)
            .HasDatabaseName("ix_order_discounts_seller_id");

        builder.HasIndex(d => new { d.IsActive, d.StartDate, d.EndDate })
            .HasDatabaseName("ix_order_discounts_active");

        builder.HasMany(d => d.Tiers)
            .WithOne()
            .HasForeignKey("OrderDiscountId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.ItemRules)
            .WithOne()
            .HasForeignKey("OrderDiscountId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.CategoryRules)
            .WithOne()
            .HasForeignKey("OrderDiscountId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
