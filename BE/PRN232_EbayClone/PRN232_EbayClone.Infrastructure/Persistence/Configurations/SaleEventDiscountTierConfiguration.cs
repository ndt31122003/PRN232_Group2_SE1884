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
        builder.Property(t => t.Id).HasColumnName("id");
        builder.Property(t => t.SaleEventId).HasColumnName("sale_event_id");
        builder.Property(t => t.DiscountType).HasColumnName("discount_type").HasMaxLength(20).HasConversion<string>();
        builder.Property(t => t.DiscountValue).HasColumnName("discount_value").HasColumnType("numeric(10,2)");
        builder.Property(t => t.Priority).HasColumnName("priority");
        builder.Property(t => t.Label).HasColumnName("label").HasMaxLength(100);

        builder.HasMany(t => t.Listings)
            .WithOne()
            .HasForeignKey(l => l.DiscountTierId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
