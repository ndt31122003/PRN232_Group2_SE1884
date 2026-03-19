using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ListingIdMappingConfiguration : IEntityTypeConfiguration<ListingIdMapping>
{
    public void Configure(EntityTypeBuilder<ListingIdMapping> builder)
    {
        builder.ToTable("listing_id_mappings");

        builder.HasKey(x => x.ListingId);

        builder.Property(x => x.ListingId)
            .HasColumnName("listing_id")
            .IsRequired();

        builder.Property(x => x.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(
                sellerId => sellerId.Value,
                value => new UserId(value))
            .IsRequired();

        // Add foreign key relationship to User (seller)
        builder.HasOne<PRN232_EbayClone.Domain.Users.Entities.User>()
            .WithMany()
            .HasForeignKey(x => x.SellerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}