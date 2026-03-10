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
            ;

        builder.Property(c => c.CouponTypeId)
            ;

        builder.Property(c => c.CategoryId)
            ;

        builder.Property(c => c.Name)
            
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Code)
            
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Code)
            .HasDatabaseName("ux_coupon_code")
            .IsUnique();

        builder.Property(c => c.DiscountValue)
            
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
            
            .HasConversion(sellerIdConverter)
            .IsRequired(false);

        builder.Property(c => c.DiscountUnit)
            
            .HasMaxLength(10)
            .HasConversion(discountUnitConverter)
            .IsRequired();

        builder.Property(c => c.MaxDiscount)
            
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.StartDate)
            
            .IsRequired();

        builder.Property(c => c.EndDate)
            
            .IsRequired();

        builder.Property(c => c.UsageLimit)
            ;

        builder.Property(c => c.UsagePerUser)
            ;

        builder.Property(c => c.MinimumOrderValue)
            
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.ApplicablePriceMin)
            
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.ApplicablePriceMax)
            
            .HasColumnType("numeric(10,2)");

        builder.Property(c => c.IsActive)
            
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            ;

        builder.Property(c => c.CreatedBy)
            ;

        builder.Property(c => c.UpdatedAt)
            ;

        builder.Property(c => c.UpdatedBy)
            ;

        builder.Property(c => c.IsDeleted)
            
            .HasDefaultValue(false);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.CouponType)
            .WithMany()
            .HasForeignKey(c => c.CouponTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Conditions)
            .WithOne(cc => cc.Coupon)
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasMany(c => c.ExcludedCategories)
            .WithOne(cc => cc.Coupon)
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasMany(c => c.ExcludedItems)
            .WithOne(cc => cc.Coupon)
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasMany(c => c.TargetAudiences)
            .WithOne(cc => cc.Coupon)
            .HasForeignKey(cc => cc.CouponId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
