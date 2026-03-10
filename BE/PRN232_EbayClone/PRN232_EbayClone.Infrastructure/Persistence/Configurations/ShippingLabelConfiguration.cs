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
                     .HasColumnName("id")
                     .HasDefaultValueSql("gen_random_uuid()")
                     .ValueGeneratedOnAdd();

              builder.Property(label => label.OrderId)
                     .HasColumnName("order_id")
                     .IsRequired();

              builder.HasIndex(label => label.OrderId)
                     .HasDatabaseName("ix_order_shipping_labels_order_id");

              builder.Property(label => label.ShippingServiceId)
                     .HasColumnName("shipping_service_id")
                     .IsRequired();

              builder.Property(label => label.Carrier)
                     .HasColumnName("carrier")
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.ServiceCode)
                     .HasColumnName("service_code")
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.ServiceName)
                     .HasColumnName("service_name")
                     .HasMaxLength(150)
                     .IsRequired();

              builder.Property(label => label.TrackingNumber)
                     .HasColumnName("tracking_number")
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(label => label.LabelUrl)
                     .HasColumnName("label_url")
                     .HasMaxLength(1024)
                     .IsRequired();

              builder.Property(label => label.LabelFileName)
                     .HasColumnName("label_file_name")
                     .HasMaxLength(260)
                     .IsRequired();

              builder.Property(label => label.PackageType)
                     .HasColumnName("package_type")
                     .HasMaxLength(80)
                     .IsRequired();

              builder.Property(label => label.WeightOz)
                     .HasColumnName("weight_oz")
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.LengthIn)
                     .HasColumnName("length_in")
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.WidthIn)
                     .HasColumnName("width_in")
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.HeightIn)
                     .HasColumnName("height_in")
                     .HasColumnType("decimal(18,2)");

              builder.Property(label => label.PurchasedAt)
                     .HasColumnName("purchased_at")
                     .IsRequired();

              builder.Property(label => label.EstimatedDelivery)
                     .HasColumnName("estimated_delivery");

              builder.Property(label => label.LabelDocumentId)
                     .HasColumnName("label_document_id")
                     .HasMaxLength(100);

              builder.Property(label => label.IsVoided)
                     .HasColumnName("is_voided")
                     .HasDefaultValue(false)
                     .IsRequired();

              builder.Property(label => label.VoidedAt)
                     .HasColumnName("voided_at");

              builder.Property(label => label.VoidReason)
                     .HasColumnName("void_reason")
                     .HasMaxLength(250);

              builder.HasOne<Order>()
                     .WithMany()
                     .HasForeignKey(label => label.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.OwnsOne(label => label.Cost, money =>
              {
                     money.Property(m => m.Amount)
                    .HasColumnName("cost_amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                     money.Property(m => m.Currency)
                    .HasColumnName("cost_currency")
                    .HasMaxLength(3)
                    .IsRequired();
              });

              builder.OwnsOne(label => label.Insurance, money =>
              {
                     money.Property(m => m.Amount)
                    .HasColumnName("insurance_amount")
                    .HasColumnType("decimal(18,2)");
                     money.Property(m => m.Currency)
                    .HasColumnName("insurance_currency")
                    .HasMaxLength(3)
                    .IsRequired();
              });
       }
}
