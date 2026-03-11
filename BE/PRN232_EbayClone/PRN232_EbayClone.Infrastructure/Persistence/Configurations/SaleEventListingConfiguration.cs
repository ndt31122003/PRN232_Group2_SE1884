using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.SaleEvents.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class SaleEventListingConfiguration : IEntityTypeConfiguration<SaleEventListing>
{
    public void Configure(EntityTypeBuilder<SaleEventListing> builder)
    {
        builder.ToTable("sale_event_listing");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id");
        builder.Property(l => l.SaleEventId).HasColumnName("sale_event_id");
        builder.Property(l => l.DiscountTierId).HasColumnName("discount_tier_id");
        builder.Property(l => l.ListingId).HasColumnName("listing_id");

        builder.HasIndex(l => new { l.SaleEventId, l.ListingId }).IsUnique();
    }
}
