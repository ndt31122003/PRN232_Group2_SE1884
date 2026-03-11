using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Coupons.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CouponConditionConfiguration : IEntityTypeConfiguration<CouponCondition>
{
    public void Configure(EntityTypeBuilder<CouponCondition> builder)
    {
        builder.ToTable("coupon_condition");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Id)
            .HasColumnName("id");

        builder.Property(cc => cc.CouponId)
            .HasColumnName("coupon_id")
            .IsRequired();

        builder.Property(cc => cc.BuyQuantity)
            .HasColumnName("buy_quantity");

        builder.Property(cc => cc.GetQuantity)
            .HasColumnName("get_quantity");

        builder.Property(cc => cc.GetDiscountPercent)
            .HasColumnName("get_discount_percent")
            .HasColumnType("numeric(5,2)");

        builder.Property(cc => cc.SaveEveryAmount)
            .HasColumnName("save_every_amount")
            .HasColumnType("numeric(10,2)");

        builder.Property(cc => cc.SaveEveryItems)
            .HasColumnName("save_every_items");

        builder.Property(cc => cc.ConditionDescription)
            .HasColumnName("condition_description")
            .HasMaxLength(255);
    }
}
