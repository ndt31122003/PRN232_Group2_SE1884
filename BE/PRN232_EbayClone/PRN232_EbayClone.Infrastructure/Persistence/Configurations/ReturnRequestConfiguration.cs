using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ReturnRequestConfiguration : IEntityTypeConfiguration<ReturnRequest>
{
    public void Configure(EntityTypeBuilder<ReturnRequest> builder)
    {
        builder.ToTable("order_return_requests");
        builder.HasKey(request => request.Id);

        builder.Property(request => request.OrderId)
            
            .IsRequired();

        builder.Property(request => request.BuyerId)
            
            .IsRequired();

        builder.Property(request => request.SellerId)
            
            .IsRequired();

        builder.Property(request => request.Reason)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.PreferredResolution)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Status)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.BuyerNote)
            
            .HasMaxLength(1500);

        builder.Property(request => request.SellerNote)
            
            .HasMaxLength(1500);

        builder.Property(request => request.RequestedAt)
            
            .IsRequired();

        builder.Property(request => request.SellerRespondedAt)
            ;

        builder.Property(request => request.BuyerReturnDueAt)
            ;

        builder.Property(request => request.BuyerShippedAt)
            ;

        builder.Property(request => request.DeliveredAt)
            ;

        builder.Property(request => request.RefundIssuedAt)
            ;

        builder.Property(request => request.ClosedAt)
            ;

        builder.Property(request => request.ReturnCarrier)
            
            .HasMaxLength(120);

        builder.Property(request => request.TrackingNumber)
            
            .HasMaxLength(120);

        builder.OwnsOne(request => request.RefundAmount, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                
                .HasMaxLength(3);

            money.HasData(ReturnRequestSeedData.ReturnRequestRefunds);
        });

        builder.OwnsOne(request => request.RestockingFee, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                
                .HasMaxLength(3);

            money.HasData(ReturnRequestSeedData.ReturnRequestRestockingFees);
        });

        builder.OwnsOne(request => request.OrderTotalSnapshot, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(ReturnRequestSeedData.ReturnRequestOrderTotals);
        });

        builder.HasIndex(request => request.OrderId).HasDatabaseName("idx_return_requests_order");
        builder.HasIndex(request => request.Status).HasDatabaseName("idx_return_requests_status");

        builder.HasData(ReturnRequestSeedData.ReturnRequests);
    }
}
