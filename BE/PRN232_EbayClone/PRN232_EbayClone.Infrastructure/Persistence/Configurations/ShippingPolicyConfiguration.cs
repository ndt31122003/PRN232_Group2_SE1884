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
            
            .IsRequired();

        builder.Property(p => p.Carrier)
            
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.ServiceName)
            
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(p => p.Cost, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.HandlingTimeDays)
            
            .IsRequired();

        builder.Property(p => p.IsDefault)
            
            .IsRequired();

        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("idx_shipping_policy_store_id");
    }
}

