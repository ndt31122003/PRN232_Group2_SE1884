using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class ShippingPolicyConfiguration : IEntityTypeConfiguration<ShippingPolicy>
{
    public void Configure(EntityTypeBuilder<ShippingPolicy> builder)
    {
        builder.ToTable("shipping_policy");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.StoreId)
            .HasConversion(
                id => id.Value,
                value => new StoreId(value))
            .HasColumnName("store_id")
            .IsRequired();

        builder.Property(p => p.Carrier)
            .HasColumnName("carrier")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.ServiceName)
            .HasColumnName("service_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(p => p.Cost, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("cost_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                .HasColumnName("cost_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.HandlingTimeDays)
            .HasColumnName("handling_time_days")
            .IsRequired();

        builder.Property(p => p.IsDefault)
            .HasColumnName("is_default")
            .IsRequired();

        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("idx_shipping_policy_store_id");
    }
}

