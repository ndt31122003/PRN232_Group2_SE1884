using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.BuyerId)
            .IsRequired()
            .HasMaxLength(450); // Typical Identity User ID length

        builder.Property(o => o.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(o => o.Listing)
            .WithMany()
            .HasForeignKey(o => o.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(o => o.ListingId);
        builder.HasIndex(o => o.BuyerId);
    }
}
