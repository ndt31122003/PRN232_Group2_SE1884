using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Stores.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class StoreSubscriptionConfiguration : IEntityTypeConfiguration<StoreSubscription>
{
    public void Configure(EntityTypeBuilder<StoreSubscription> builder)
    {
        builder.ToTable("store_subscription");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StoreId)
            .HasConversion(
                id => id.Value,
                value => new StoreId(value))
            
            .IsRequired();

        builder.Property(s => s.SubscriptionType)
            
            .IsRequired();

        builder.OwnsOne(s => s.MonthlyFee, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("numeric(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(s => s.FinalValueFeePercentage)
            
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        builder.Property(s => s.ListingLimit)
            
            .IsRequired();

        builder.Property(s => s.StartDate)
            
            .IsRequired();

        builder.Property(s => s.EndDate)
            ;

        builder.Property(s => s.Status)
            
            .IsRequired();

        builder.HasIndex(s => s.StoreId)
            .HasDatabaseName("idx_store_subscription_store_id");

        builder.HasIndex(s => new { s.StoreId, s.Status })
            .HasDatabaseName("idx_store_subscription_store_status");
    }
}

