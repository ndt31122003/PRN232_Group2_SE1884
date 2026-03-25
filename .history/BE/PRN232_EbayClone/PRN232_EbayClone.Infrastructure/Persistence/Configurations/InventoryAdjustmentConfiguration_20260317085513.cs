using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class InventoryAdjustmentConfiguration : IEntityTypeConfiguration<InventoryAdjustment>
{
    public void Configure(EntityTypeBuilder<InventoryAdjustment> builder)
    {
        builder.ToTable("inventory_adjustment");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_inventory_adjustment");
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(x => x.InventoryId)
            .HasConversion(
                id => id.Value,
                value => new InventoryId(value))
            .HasColumnName("inventory_id")
            .IsRequired();
        
        builder.Property(x => x.SellerId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .HasColumnName("seller_id")
            .IsRequired();
        
        builder.Property(x => x.AdjustmentType)
            .HasColumnName("adjustment_type")
            .IsRequired();
        
        builder.Property(x => x.QuantityChange)
            .HasColumnName("quantity_change")
            .IsRequired();
        
        builder.Property(x => x.Reason)
            .HasColumnName("reason")
            .IsRequired(false);
        
        builder.Property(x => x.AdjustedAt)
            .HasColumnName("adjusted_at")
            .IsRequired();
        
        builder.Property(x => x.AdjustedBy)
            .HasColumnName("adjusted_by")
            .IsRequired(false);
        
        // Indexes
        builder.HasIndex(x => x.InventoryId)
            .HasDatabaseName("idx_inventory_adjustment_inventory_id");
    }
}
