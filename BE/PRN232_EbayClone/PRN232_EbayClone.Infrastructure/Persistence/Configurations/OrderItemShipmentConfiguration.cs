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
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()")
            .ValueGeneratedOnAdd();

        builder.Property(shipment => shipment.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(shipment => shipment.OrderItemId)
            .HasColumnName("order_item_id")
            .IsRequired();

        builder.Property(shipment => shipment.ShippingLabelId)
            .HasColumnName("shipping_label_id");

        builder.Property(shipment => shipment.TrackingNumber)
            .HasColumnName("tracking_number")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(shipment => shipment.Carrier)
            .HasColumnName("carrier")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(shipment => shipment.ShippedAt)
            .HasColumnName("shipped_at")
            .IsRequired();

        builder.Property(shipment => shipment.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(shipment => shipment.UpdatedAt)
            .HasColumnName("updated_at");

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
