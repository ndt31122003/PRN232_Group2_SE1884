using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class SaleEventPriceSnapshotConfiguration : IEntityTypeConfiguration<SaleEventPriceSnapshot>
{
    public void Configure(EntityTypeBuilder<SaleEventPriceSnapshot> builder)
    {
        builder.ToTable("sale_event_price_snapshots");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleEventId)
            .IsRequired();

        builder.Property(s => s.ListingId)
            .IsRequired();

        builder.Property(s => s.OriginalPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(s => s.SnapshotAt)
            .IsRequired();

        builder.HasIndex(s => new { s.SaleEventId, s.ListingId })
            .IsUnique();
    }
}
