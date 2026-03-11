using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(oi => oi.Id);

        builder.Property(i => i.ListingId)
            .IsRequired();

        builder.Property(i => i.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(500);

        builder.Property(i => i.Sku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.OwnsOne(i => i.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired();
            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(OrderSeedData.OrderItemUnitPrices);
        });

        builder.OwnsOne(i => i.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .IsRequired();
            money.Property(m => m.Currency)
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(OrderSeedData.OrderItemTotalPrices);
        });

        builder.Ignore(i => i.DomainEvents);

        builder.HasIndex(i => i.ListingId)
            .HasDatabaseName("idx_order_items_listing_id");

        builder.HasOne<Listing>()
            .WithMany()
            .HasForeignKey(i => i.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(OrderSeedData.OrderItems);
    }
}
