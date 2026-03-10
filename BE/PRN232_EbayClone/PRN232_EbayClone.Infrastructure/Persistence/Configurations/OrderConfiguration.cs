using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
       public void Configure(EntityTypeBuilder<Order> builder)
       {
              builder.ToTable("orders");
              builder.HasKey(o => o.Id);

              builder.Property(o => o.OrderNumber)
                     
                     .HasMaxLength(50)
                     .IsRequired();

              builder.HasIndex(o => o.OrderNumber)
                     .IsUnique();

              ConfigureMoney(builder, o => o.SubTotal, "sub_total", OrderSeedData.SubTotals);
              ConfigureMoney(builder, o => o.ShippingCost, "shipping_cost", OrderSeedData.ShippingCosts);
              ConfigureMoney(builder, o => o.PlatformFee, "platform_fee", OrderSeedData.PlatformFees);
              ConfigureMoney(builder, o => o.TaxAmount, "tax", OrderSeedData.TaxAmounts);
              ConfigureMoney(builder, o => o.DiscountAmount, "discount", OrderSeedData.DiscountAmounts);
              ConfigureMoney(builder, o => o.Total, "total", OrderSeedData.Totals);

              builder.Property(o => o.OrderedAt).IsRequired();
              builder.Property(o => o.PaidAt);
              builder.Property(o => o.ShippedAt);
              builder.Property(o => o.CancelledAt);
              builder.Property(o => o.ArchivedAt);

              builder.HasMany(o => o.Items)
                     .WithOne()
                     .HasForeignKey("OrderId")
                     .IsRequired();


              builder.HasMany(o => o.ItemShipments)
                     .WithOne()
                     .HasForeignKey(shipment => shipment.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.Navigation(o => o.ItemShipments)
                     .HasField("_itemShipments")
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.HasMany(o => o.CancellationRequests)
                     .WithOne(request => request.Order)
                     .HasForeignKey(request => request.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.Navigation(o => o.CancellationRequests)
                     .HasField("_cancellationRequests")
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.HasMany(o => o.ReturnRequests)
                     .WithOne(request => request.Order)
                     .HasForeignKey(request => request.OrderId)
                     .OnDelete(DeleteBehavior.Cascade);

              builder.Navigation(o => o.ReturnRequests)
                     .HasField("_returnRequests")
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.HasOne(o => o.Status)
                     .WithMany()
                     .HasForeignKey("StatusId")
                     .IsRequired();

              builder.HasMany(o => o.StatusHistory)
                     .WithOne()
                     .HasForeignKey("OrderId");

              builder.Navigation(o => o.StatusHistory)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

               builder.HasOne(o => o.Buyer)
                      .WithMany()
                      .HasForeignKey(o => o.BuyerId)
                      .IsRequired(false);

               builder.HasData(OrderSeedData.Orders);

              builder.Ignore(o => o.DomainEvents);
       }

       private void ConfigureMoney(EntityTypeBuilder<Order> builder,
                                                           System.Linq.Expressions.Expression<Func<Order, Money?>> property,
                                                           string prefix,
                                                           IEnumerable<object>? seedData = null)
       {
              builder.OwnsOne(property, money =>
              {
                     money.Property(m => m.Amount)
                    .HasColumnName($"{prefix}_amount")
                    .IsRequired();
                     money.Property(m => m.Currency)
                    .HasColumnName($"{prefix}_currency")
                    .HasMaxLength(3)
                    .IsRequired();

                     if (seedData is not null)
                     {
                            money.HasData(seedData);
                     }
              });
       }



}
