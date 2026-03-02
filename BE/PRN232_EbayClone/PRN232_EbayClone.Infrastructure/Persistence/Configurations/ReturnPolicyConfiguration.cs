using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class ReturnPolicyConfiguration : IEntityTypeConfiguration<ReturnPolicy>
{
    public void Configure(EntityTypeBuilder<ReturnPolicy> builder)
    {
        builder.ToTable("return_policy");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.StoreId)
            .HasConversion(
                id => id.Value,
                value => new StoreId(value))
            .HasColumnName("store_id")
            .IsRequired();

        builder.Property(p => p.AcceptReturns)
            .HasColumnName("accept_returns")
            .IsRequired();

        builder.Property(p => p.ReturnPeriodDays)
            .HasColumnName("return_period_days");

        builder.Property(p => p.RefundMethod)
            .HasColumnName("refund_method");

        builder.Property(p => p.ReturnShippingPaidBy)
            .HasColumnName("return_shipping_paid_by");

        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("idx_return_policy_store_id")
            .IsUnique();
    }
}

