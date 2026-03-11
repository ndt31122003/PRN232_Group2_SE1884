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
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(request => request.BuyerId)
            .HasColumnName("buyer_id")
            .IsRequired();

        builder.Property(request => request.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();

        builder.Property(request => request.InitiatedBy)
            .HasColumnName("initiated_by")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Reason)
            .HasColumnName("reason")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.BuyerNote)
            .HasColumnName("buyer_note")
            .HasMaxLength(1000);

        builder.Property(request => request.SellerNote)
            .HasColumnName("seller_note")
            .HasMaxLength(1000);

        builder.Property(request => request.RequestedAt)
            .HasColumnName("requested_at")
            .IsRequired();

        builder.Property(request => request.SellerResponseDeadlineUtc)
            .HasColumnName("seller_response_deadline_utc");

        builder.Property(request => request.SellerRespondedAt)
            .HasColumnName("seller_responded_at");

        builder.Property(request => request.AutoClosedAt)
            .HasColumnName("auto_closed_at");

        builder.Property(request => request.CompletedAt)
            .HasColumnName("completed_at");

        builder.OwnsOne(request => request.RefundAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("refund_amount")
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                .HasColumnName("refund_currency")
                .HasMaxLength(3);

            money.HasData(CancellationRequestSeedData.CancellationRequestRefunds);
        });

        builder.OwnsOne(request => request.OrderTotalSnapshot, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("order_total_amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            money.Property(m => m.Currency)
                .HasColumnName("order_total_currency")
                .HasMaxLength(3)
                .IsRequired();

            money.HasData(CancellationRequestSeedData.CancellationRequestOrderTotals);
        });

        builder.HasIndex(request => request.OrderId).HasDatabaseName("idx_cancellation_requests_order");
        builder.HasIndex(request => request.Status).HasDatabaseName("idx_cancellation_requests_status");

        builder.HasData(CancellationRequestSeedData.CancellationRequests);
    }
}
