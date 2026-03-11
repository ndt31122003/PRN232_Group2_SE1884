using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN232_EbayClone.Domain.SaleEvents.Entities;
using PRN232_EbayClone.Domain.SaleEvents.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class SaleEventConfiguration : IEntityTypeConfiguration<SaleEvent>
{
    public void Configure(EntityTypeBuilder<SaleEvent> builder)
    {
        builder.ToTable("sale_event");

        builder.HasKey(se => se.Id);
        builder.Property(se => se.Id).HasColumnName("id");

        var modeConverter = new EnumToStringConverter<SaleEventMode>();
        var statusConverter = new EnumToStringConverter<SaleEventStatus>();

        builder.Property(se => se.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(se => se.Name)
            .HasColumnName("name")
            .HasMaxLength(90)
            .IsRequired();

        builder.Property(se => se.Description)
            .HasColumnName("description")
            .HasMaxLength(255);

        builder.Property(se => se.Mode)
            .HasColumnName("mode")
            .HasConversion(modeConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(se => se.Status)
            .HasColumnName("status")
            .HasConversion(statusConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(se => se.StartDate).HasColumnName("start_date").IsRequired();
        builder.Property(se => se.EndDate).HasColumnName("end_date").IsRequired();

        builder.Property(se => se.OfferFreeShipping).HasColumnName("offer_free_shipping");
        builder.Property(se => se.IncludeSkippedItems).HasColumnName("include_skipped_items");
        builder.Property(se => se.BlockPriceIncreaseRevisions).HasColumnName("block_price_increase_revisions");
        builder.Property(se => se.HighlightPercentage).HasColumnName("highlight_percentage").HasColumnType("numeric(5,2)");

        builder.Property(se => se.CreatedAt).HasColumnName("created_at");
        builder.Property(se => se.CreatedBy).HasColumnName("created_by");
        builder.Property(se => se.UpdatedAt).HasColumnName("updated_at");
        builder.Property(se => se.UpdatedBy).HasColumnName("updated_by");
        builder.Property(se => se.IsDeleted).HasColumnName("is_deleted");

        builder.HasMany(se => se.DiscountTiers)
            .WithOne()
            .HasForeignKey(dt => dt.SaleEventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(se => se.Listings)
            .WithOne()
            .HasForeignKey(l => l.SaleEventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
