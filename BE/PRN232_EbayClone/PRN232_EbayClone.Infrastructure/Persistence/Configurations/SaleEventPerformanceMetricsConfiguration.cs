using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class SaleEventPerformanceMetricsConfiguration : IEntityTypeConfiguration<SaleEventPerformanceMetrics>
{
    public void Configure(EntityTypeBuilder<SaleEventPerformanceMetrics> builder)
    {
        builder.ToTable("sale_event_performance_metrics");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.SaleEventId)
            .IsRequired();

        builder.Property(m => m.OrderCount)
            .IsRequired();

        builder.Property(m => m.TotalDiscountAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(m => m.TotalSalesRevenue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(m => m.TotalItemsSold)
            .IsRequired();

        builder.Property(m => m.LastUpdated)
            .IsRequired();

        builder.Ignore(m => m.AverageDiscountPerOrder);
        builder.Ignore(m => m.AverageOrderValue);

        builder.HasIndex(m => m.SaleEventId)
            .IsUnique();
    }
}
