using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OrderDiscountPerformanceMetricsConfiguration : IEntityTypeConfiguration<OrderDiscountPerformanceMetrics>
{
    public void Configure(EntityTypeBuilder<OrderDiscountPerformanceMetrics> builder)
    {
        builder.ToTable("order_discount_performance_metrics");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id).HasColumnName("id");

        builder.Property(m => m.OrderDiscountId)
            .HasColumnName("order_discount_id")
            .IsRequired();

        builder.Property(m => m.OrderCount)
            .HasColumnName("order_count")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(m => m.TotalDiscountAmount)
            .HasColumnName("total_discount_amount")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(m => m.TotalSalesRevenue)
            .HasColumnName("total_sales_revenue")
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(m => m.TotalItemsSold)
            .HasColumnName("total_items_sold")
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(m => m.LastUpdated)
            .HasColumnName("last_updated")
            .IsRequired();

        builder.Ignore(m => m.AverageOrderValue);
    }
}
