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
            .HasColumnName("delivery_window_label")
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(s => s.PrinterRequired)
            .HasColumnName("printer_required")
            .HasDefaultValue(false);

        builder.Property(s => s.SupportsQrCode)
            .HasColumnName("supports_qr_code")
            .HasDefaultValue(false);

        builder.Property(s => s.MinEstimatedDeliveryDays)
            .HasColumnName("min_estimated_delivery_days")
            .IsRequired();

        builder.Property(s => s.MaxEstimatedDeliveryDays)
            .HasColumnName("max_estimated_delivery_days")
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
