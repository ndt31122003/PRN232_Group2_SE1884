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
            ;

        builder.Property(cc => cc.CouponId)
            
            .IsRequired();

        builder.Property(cc => cc.BuyQuantity)
            ;

        builder.Property(cc => cc.GetQuantity)
            ;

        builder.Property(cc => cc.GetDiscountPercent)
            
            .HasColumnType("numeric(5,2)");

        builder.Property(cc => cc.SaveEveryAmount)
            
            .HasColumnType("numeric(10,2)");

        builder.Property(cc => cc.SaveEveryItems)
            ;

        builder.Property(cc => cc.ConditionDescription)
            
            .HasMaxLength(255);
    }
}
