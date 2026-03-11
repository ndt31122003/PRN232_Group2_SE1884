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
            .HasColumnName("store_id")
            .IsRequired();

        builder.Property(s => s.SubscriptionType)
            .HasColumnName("subscription_type")
            .IsRequired();

        builder.OwnsOne(s => s.MonthlyFee, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("monthly_fee")
                .HasColumnType("numeric(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                .HasColumnName("monthly_fee_currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(s => s.FinalValueFeePercentage)
            .HasColumnName("final_value_fee_percentage")
            .HasColumnType("numeric(5,2)")
            .IsRequired();

        builder.Property(s => s.ListingLimit)
            .HasColumnName("listing_limit")
            .IsRequired();

        builder.Property(s => s.StartDate)
            .HasColumnName("start_date")
            .IsRequired();

        builder.Property(s => s.EndDate)
            .HasColumnName("end_date");

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .IsRequired();

        builder.HasIndex(s => s.StoreId)
            .HasDatabaseName("idx_store_subscription_store_id");

        builder.HasIndex(s => new { s.StoreId, s.Status })
            .HasDatabaseName("idx_store_subscription_store_status");
    }
}

