using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class AuctionListingConfiguration : IEntityTypeConfiguration<AuctionListing>
{
    public void Configure(EntityTypeBuilder<AuctionListing> builder)
    {
        builder.OwnsOne(a => a.Pricing);
    }
}

