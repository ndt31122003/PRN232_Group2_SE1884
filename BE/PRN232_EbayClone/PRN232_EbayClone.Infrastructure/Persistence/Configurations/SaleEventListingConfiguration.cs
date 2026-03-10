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
        builder.Property(l => l.Id);
        builder.Property(l => l.SaleEventId);
        builder.Property(l => l.DiscountTierId);
        builder.Property(l => l.ListingId);

        builder.HasIndex(l => new { l.SaleEventId, l.ListingId }).IsUnique();
    }
}
