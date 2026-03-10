using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ShippingLabelConfiguration : IEntityTypeConfiguration<ShippingLabel>
{
       public void Configure(EntityTypeBuilder<ShippingLabel> builder)
       {
              builder.ToTable("order_shipping_labels");
              builder.HasKey(label => label.Id);

              builder.Property(label => label.Id)
                     
                     .HasDefaultValueSql("gen_random_uuid()")
                     .ValueGeneratedOnAdd();

              builder.Property(label => label.OrderId)
                     
                     .IsRequired();

              builder.HasIndex(label => label.OrderId)
                     .HasDatabaseName("ix_order_shipping_labels_order_id");

              builder.Property(label => label.ShippingServiceId)
                     
                     .IsRequired();

              builder.Property(label => label.Carrier)
                     
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.ServiceCode)
                     
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.ServiceName)
                     
                     .HasMaxLength(150)
                     .IsRequired();

              builder.Property(label => label.TrackingNumber)
                     
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.LabelUrl)
                     
                     .HasMaxLength(1024)
                     .IsRequired();

              builder.Property(label => label.LabelFileName)
                     
                     .HasMaxLength(260)
                     .IsRequired();

              builder.Property(label => label.PackageType)
                     
                     .HasMaxLength(80)
                     .IsRequired();

              builder.Property(label => label.WeightOz)
                     
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.LengthIn)
                     
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.WidthIn)
                     
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.HeightIn)
                     
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.PurchasedAt)
                     
                     .IsRequired();

              builder.Property(label => label.EstimatedDelivery)
                     ;

              builder.Property(label => label.LabelDocumentId)
                     
                     .HasMaxLength(100);

              builder.Property(label => label.IsVoided)
                     
                     .HasDefaultValue(false)
                     .IsRequired();

              builder.Property(label => label.VoidedAt)
                     ;

              builder.Property(label => label.VoidReason)
                     
                     .HasMaxLength(250);

              builder.HasOne<Order>()
                     .WithMany()
                     .HasForeignKey(label => label.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.OwnsOne(label => label.Cost, money =>
              {
                     money.Property(m => m.Amount)
                    
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                     money.Property(m => m.Currency)
                    
                    .HasMaxLength(3)
                    .IsRequired();
              });

              builder.OwnsOne(label => label.Insurance, money =>
              {
                     money.Property(m => m.Amount)
                    
                    .HasColumnType("decimal(18,2)");
                     money.Property(m => m.Currency)
                    
                    .HasMaxLength(3)
                    .IsRequired();
              });
       }
}
