using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class SaleEventDiscountTierConfiguration : IEntityTypeConfiguration<SaleEventDiscountTier>
{
    public void Configure(EntityTypeBuilder<SaleEventDiscountTier> builder)
    {
        builder.ToTable("sale_event_discount_tiers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.SaleEventId)
            .IsRequired();

        builder.Property(t => t.DiscountType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.DiscountValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.Priority)
            .IsRequired();

        builder.Property(t => t.Label)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(t => t.Listings)
            .WithOne()
            .HasForeignKey("DiscountTierId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.SaleEventId, t.Priority })
            .IsUnique();
    }
}
