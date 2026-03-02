using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Coupons.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("coupon");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.CouponTypeId)
            .HasColumnName("coupon_type_id");

        builder.Property(c => c.CategoryId)
            .HasColumnName("category_id");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Code)
            .HasDatabaseName("ux_coupon_code")
            .IsUnique();

        builder.Property(c => c.DiscountValue)
            .HasColumnName("discount_value")
            .HasColumnType("numeric(10,2)")
            .IsRequired();

        var discountUnitConverter = new ValueConverter<CouponDiscountUnit, string>(
            v => v == CouponDiscountUnit.Percent ? "PERCENT" : "AMOUNT",
            v => string.Equals(v, "PERCENT", StringComparison.OrdinalIgnoreCase)
                ? CouponDiscountUnit.Percent
                : CouponDiscountUnit.Amount);

        var sellerIdConverter = new ValueConverter<UserId?, Guid?>(
            id => id.HasValue ? id.Value.Value : (Guid?)null,
            value => value.HasValue ? new UserId(value.Value) : null);

        builder.Property(c => c.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(sellerIdConverter)
            .IsRequired(false);

        builder.Property(c => c.DiscountUnit)
            .HasColumnName("discount_unit")
            .HasMaxLength(10)
            .HasConversion(discountUnitConverter)
            .IsRequired();

        builder.Property(c => c.MaxDiscount)
            .HasColumnName("max_discount")
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(c => c.EndDate)
            .HasColumnName("end_date")
            .IsRequired();

        builder.Property(c => c.UsageLimit)
            .HasColumnName("usage_limit");

        builder.Property(c => c.UsagePerUser)
            .HasColumnName("usage_per_user");

        builder.Property(c => c.MinimumOrderValue)
            .HasColumnName("minimum_order_value")
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.ApplicablePriceMin)
            .HasColumnName("applicable_price_min")
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.ApplicablePriceMax)
            .HasColumnName("applicable_price_max")
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(c => c.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.UpdatedBy)
            .HasColumnName("updated_by");

        builder.Property(c => c.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.CouponType)
            .WithMany()
            .HasForeignKey(c => c.CouponTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Conditions)
            .WithOne()
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
