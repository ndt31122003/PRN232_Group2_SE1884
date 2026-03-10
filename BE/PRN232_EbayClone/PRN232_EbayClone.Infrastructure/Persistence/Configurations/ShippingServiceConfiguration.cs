using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class ShippingServiceConfiguration : IEntityTypeConfiguration<ShippingService>
{
    public void Configure(EntityTypeBuilder<ShippingService> builder)
    {
        builder.ToTable("shipping_services");

        builder.HasKey(s => s.Id);

        builder.HasIndex(s => s.Slug)
            .IsUnique();

        builder.Property(s => s.Carrier)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Slug)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(s => s.ServiceCode)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(s => s.ServiceName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.SavingsDescription)
            .HasMaxLength(150);

        builder.Property(s => s.CoverageDescription)
            .HasMaxLength(150);

        builder.Property(s => s.Notes)
            .HasMaxLength(256);

        builder.Property(s => s.DeliveryWindowLabel)
            
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(s => s.PrinterRequired)
            
            .HasDefaultValue(false);

        builder.Property(s => s.SupportsQrCode)
            
            .HasDefaultValue(false);

        builder.Property(s => s.MinEstimatedDeliveryDays)
            
            .IsRequired();

        builder.Property(s => s.MaxEstimatedDeliveryDays)
            
            .IsRequired();

        builder.OwnsOne(s => s.BaseCost, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(ShippingServiceSeed.BaseCosts);
        });

        builder.HasData(ShippingServiceSeed.Services);
    }
}
