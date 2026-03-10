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
        builder.Property(se => se.Id);

        var modeConverter = new EnumToStringConverter<SaleEventMode>();
        var statusConverter = new EnumToStringConverter<SaleEventStatus>();

        builder.Property(se => se.SellerId)
            
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(se => se.Name)
            
            .HasMaxLength(90)
            .IsRequired();

        builder.Property(se => se.Description)
            
            .HasMaxLength(255);

        builder.Property(se => se.Mode)
            
            .HasConversion(modeConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(se => se.Status)
            
            .HasConversion(statusConverter)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(se => se.StartDate).IsRequired();
        builder.Property(se => se.EndDate).IsRequired();

        builder.Property(se => se.OfferFreeShipping);
        builder.Property(se => se.IncludeSkippedItems);
        builder.Property(se => se.BlockPriceIncreaseRevisions);
        builder.Property(se => se.HighlightPercentage).HasColumnType("numeric(5,2)");

        builder.Property(se => se.CreatedAt);
        builder.Property(se => se.CreatedBy);
        builder.Property(se => se.UpdatedAt);
        builder.Property(se => se.UpdatedBy);
        builder.Property(se => se.IsDeleted);

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
