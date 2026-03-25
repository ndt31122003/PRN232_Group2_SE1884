using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class SaleEventListingConfiguration : IEntityTypeConfiguration<SaleEventListing>
{
    public void Configure(EntityTypeBuilder<SaleEventListing> builder)
    {
        builder.ToTable("sale_event_listings");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.SaleEventId)
            .IsRequired();

        builder.Property(l => l.DiscountTierId)
            .IsRequired();

        builder.Property(l => l.ListingId)
            .IsRequired();

        builder.Property(l => l.AssignedAt)
            .IsRequired();

        builder.HasIndex(l => l.ListingId);
        builder.HasIndex(l => new { l.SaleEventId, l.ListingId })
            .IsUnique();
    }
}
