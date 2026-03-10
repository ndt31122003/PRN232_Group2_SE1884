using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.SaleEvents.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class SaleEventDiscountTierConfiguration : IEntityTypeConfiguration<SaleEventDiscountTier>
{
    public void Configure(EntityTypeBuilder<SaleEventDiscountTier> builder)
    {
        builder.ToTable("sale_event_discount_tier");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id);
        builder.Property(t => t.SaleEventId);
        builder.Property(t => t.DiscountType).HasMaxLength(20).HasConversion<string>();
        builder.Property(t => t.DiscountValue).HasColumnType("numeric(10,2)");
        builder.Property(t => t.Priority);
        builder.Property(t => t.Label).HasMaxLength(100);

        builder.HasMany(t => t.Listings)
            .WithOne()
            .HasForeignKey(l => l.DiscountTierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
