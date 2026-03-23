using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BidderId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(b => b.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne(b => b.Listing)
            .WithMany()
            .HasForeignKey(b => b.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => b.ListingId);
        builder.HasIndex(b => b.BidderId);
    }
}
