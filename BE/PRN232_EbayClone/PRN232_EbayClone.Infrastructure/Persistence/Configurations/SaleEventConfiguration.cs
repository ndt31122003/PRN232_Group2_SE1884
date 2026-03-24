using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class SaleEventConfiguration : IEntityTypeConfiguration<SaleEvent>
{
    public void Configure(EntityTypeBuilder<SaleEvent> builder)
    {
        builder.ToTable("sale_events");

        builder.HasKey(se => se.Id);

        builder.Property(se => se.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(se => se.Description)
            .HasMaxLength(1000);

        builder.Property(se => se.SellerId)
            .IsRequired();

        builder.Property(se => se.StartDate)
            .IsRequired();

        builder.Property(se => se.EndDate)
            .IsRequired();

        builder.Property(se => se.Mode)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(se => se.HighlightPercentage)
            .HasPrecision(5, 2);

        builder.Property(se => se.OfferFreeShipping)
            .IsRequired();

        builder.Property(se => se.BlockPriceIncreaseRevisions)
            .IsRequired();

        builder.Property(se => se.IncludeSkippedItems)
            .IsRequired();

        builder.Property(se => se.BuyerMessageLabel)
            .HasMaxLength(200);

        builder.Ignore(se => se.Type);
        builder.Ignore(se => se.IsActive);

        builder.Property(se => se.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(se => se.CreatedAt)
            .IsRequired();

        builder.Property(se => se.UpdatedAt);

        builder.HasMany(se => se.DiscountTiers)
            .WithOne()
            .HasForeignKey("SaleEventId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(se => se.Listings)
            .WithOne()
            .HasForeignKey("SaleEventId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(se => se.SellerId);
        builder.HasIndex(se => se.Status);
        builder.HasIndex(se => new { se.StartDate, se.EndDate });
    }
}
