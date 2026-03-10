using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class OrderItemShipmentConfiguration : IEntityTypeConfiguration<OrderItemShipment>
{
    public void Configure(EntityTypeBuilder<OrderItemShipment> builder)
    {
        builder.ToTable("order_item_shipments");

        builder.HasKey(shipment => shipment.Id)
            .HasName("pk_order_item_shipments");

        builder.Property(shipment => shipment.Id)
            
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(shipment => shipment.OrderId)
            
            .IsRequired();

        builder.Property(shipment => shipment.OrderItemId)
            
            .IsRequired();

        builder.Property(shipment => shipment.ShippingLabelId)
            ;

        builder.Property(shipment => shipment.TrackingNumber)
            
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(shipment => shipment.Carrier)
            
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(shipment => shipment.ShippedAt)
            
            .IsRequired();

        builder.Property(shipment => shipment.CreatedAt)
            
            .IsRequired();

        builder.Property(shipment => shipment.UpdatedAt)
            ;

        builder.HasIndex(shipment => shipment.OrderId)
            .HasDatabaseName("ix_order_item_shipments_order_id");

        builder.HasIndex(shipment => shipment.OrderItemId)
            .HasDatabaseName("ix_order_item_shipments_order_item_id");

        builder.HasIndex(shipment => shipment.ShippingLabelId)
            .HasDatabaseName("ix_order_item_shipments_shipping_label_id");

        builder.HasOne<ShippingLabel>()
            .WithMany()
            .HasForeignKey(shipment => shipment.ShippingLabelId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
