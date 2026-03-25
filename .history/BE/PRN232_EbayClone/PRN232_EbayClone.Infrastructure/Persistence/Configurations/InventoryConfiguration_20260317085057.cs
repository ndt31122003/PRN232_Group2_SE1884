using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("inventory");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_inventory");
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => InventoryId.From(value))
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(x => x.ListingId)
            .HasConversion(
                id => id.Value,
                value => new ListingId(value))
            .HasColumnName("listing_id")
            .IsRequired();
        
        builder.Property(x => x.SellerId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value))
            .HasColumnName("seller_id")
            .IsRequired();
        
        builder.Property(x => x.TotalQuantity)
            .HasColumnName("total_quantity")
            .IsRequired();
        
        builder.Property(x => x.AvailableQuantity)
            .HasColumnName("available_quantity")
            .IsRequired();
        
        builder.Property(x => x.ReservedQuantity)
            .HasColumnName("reserved_quantity")
            .IsRequired();
        
        builder.Property(x => x.SoldQuantity)
            .HasColumnName("sold_quantity")
            .IsRequired();
        
        builder.Property(x => x.ThresholdQuantity)
            .HasColumnName("threshold_quantity")
            .IsRequired(false);
        
        builder.Property(x => x.IsLowStock)
            .HasColumnName("is_low_stock")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.LastLowStockNotificationAt)
            .HasColumnName("last_low_stock_notification_at")
            .IsRequired(false);
        
        builder.Property(x => x.LastUpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne<Listing>()
            .WithMany()
            .HasForeignKey(x => x.ListingId)
            .HasConstraintName("fk_inventory_listing_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.SellerId)
            .HasConstraintName("fk_inventory_seller_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        // Unique constraint
        builder.HasIndex(x => x.ListingId)
            .IsUnique()
            .HasDatabaseName("uk_inventory_listing_id");
        
        // Regular indexes
        builder.HasIndex(x => x.SellerId)
            .HasDatabaseName("idx_inventory_seller_id");
        
        builder.HasIndex(x => new { x.SellerId, x.IsLowStock })
            .HasDatabaseName("idx_inventory_is_low_stock");
        
        builder.HasIndex(x => x.LastUpdatedAt)
            .HasDatabaseName("idx_inventory_updated_at")
            .IsDescending();
    }
}
