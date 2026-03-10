using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CancellationRequestConfiguration : IEntityTypeConfiguration<CancellationRequest>
{
    public void Configure(EntityTypeBuilder<CancellationRequest> builder)
    {
        builder.ToTable("order_cancellation_requests");
        builder.HasKey(request => request.Id);

        builder.Property(request => request.OrderId)
            
            .IsRequired();

        builder.Property(request => request.BuyerId)
            
            .IsRequired();

        builder.Property(request => request.SellerId)
            
            .IsRequired();

        builder.Property(request => request.InitiatedBy)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Reason)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Status)
            
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.BuyerNote)
            
            .HasMaxLength(1000);

        builder.Property(request => request.SellerNote)
            
            .HasMaxLength(1000);

        builder.Property(request => request.RequestedAt)
            
            .IsRequired();

        builder.Property(request => request.SellerResponseDeadlineUtc)
            ;

        builder.Property(request => request.SellerRespondedAt)
            ;

        builder.Property(request => request.AutoClosedAt)
            ;

        builder.Property(request => request.CompletedAt)
            ;

        builder.OwnsOne(request => request.RefundAmount, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                
                .HasMaxLength(3);

            money.HasData(CancellationRequestSeedData.CancellationRequestRefunds);
        });

        builder.OwnsOne(request => request.OrderTotalSnapshot, money =>
        {
            money.Property(m => m.Amount)
                
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(CancellationRequestSeedData.CancellationRequestOrderTotals);
        });

        builder.HasIndex(request => request.OrderId).HasDatabaseName("idx_cancellation_requests_order");
        builder.HasIndex(request => request.Status).HasDatabaseName("idx_cancellation_requests_status");

        builder.HasData(CancellationRequestSeedData.CancellationRequests);
    }
}
