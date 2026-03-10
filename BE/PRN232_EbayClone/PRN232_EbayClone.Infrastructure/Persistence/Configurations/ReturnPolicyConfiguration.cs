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
            
            .IsRequired();

        builder.Property(p => p.AcceptReturns)
            
            .IsRequired();

        builder.Property(p => p.ReturnPeriodDays)
            ;

        builder.Property(p => p.RefundMethod)
            ;

        builder.Property(p => p.ReturnShippingPaidBy)
            ;

        builder.HasIndex(p => p.StoreId)
            .HasDatabaseName("idx_return_policy_store_id")
            .IsUnique();
    }
}

