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

        builder.Property(x => x.EmailNotificationsEnabled)
            .HasColumnName("low_stock_email_enabled")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.AdditionalNotificationEmails)
            .HasColumnName("additional_low_stock_emails")
            .HasMaxLength(1000)
            .IsRequired()
            .HasDefaultValue(string.Empty);
        
        builder.Property(x => x.LastLowStockNotificationAt)
            .HasColumnName("last_low_stock_notification_at")
            .IsRequired(false);
        
        builder.Property(x => x.LastUpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        // No explicit foreign key configuration needed - EF will infer from property naming
        // We'll rely on the value object conversion to handle the mapping
        
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
        
        // Owned InventoryReservation collection
        builder.OwnsMany(x => x.Reservations, b =>
        {
            b.ToTable("inventory_reservation");
            b.WithOwner().HasForeignKey(x => x.InventoryId);
            
            b.HasKey(x => x

.Id);
            
            b.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => new InventoryReservationId(value))
                .HasColumnName("id");
            
            b.Property(x => x.InventoryId)
                .HasConversion(
                    id => id.Value,
                    value => new InventoryId(value))
                .HasColumnName("inventory_id");
            
            b.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired(false);
            
            b.Property(x => x.BuyerId)
                .HasConversion(
                    id => id.Value,
                    value => new UserId(value))
                .HasColumnName("buyer_id");
            
            b.Property(x => x.ReservationType)
                .HasColumnName("reservation_type");
            
            b.Property(x => x.Quantity)
                .HasColumnName("quantity");
            
            b.Property(x => x.ReservedAt)
                .HasColumnName("reserved_at");
            
            b.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at");
            
            b.Property(x => x.ReleasedAt)
                .HasColumnName("released_at")
                .IsRequired(false);
            
            b.Property(x => x.CommittedAt)
                .HasColumnName("committed_at")
                .IsRequired(false);
            
            b.HasIndex(x => x.ExpiresAt)
                .HasDatabaseName("idx_inventory_reservation_expires_at");
        });
    }
}
