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
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(request => request.BuyerId)
            .HasColumnName("buyer_id")
            .IsRequired();

        builder.Property(request => request.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();

        builder.Property(request => request.Reason)
            .HasColumnName("reason")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.PreferredResolution)
            .HasColumnName("preferred_resolution")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(request => request.BuyerNote)
            .HasColumnName("buyer_note")
            .HasMaxLength(1500);

        builder.Property(request => request.SellerNote)
            .HasColumnName("seller_note")
            .HasMaxLength(1500);

        builder.Property(request => request.RequestedAt)
            .HasColumnName("requested_at")
            .IsRequired();

        builder.Property(request => request.SellerRespondedAt)
            .HasColumnName("seller_responded_at");

        builder.Property(request => request.BuyerReturnDueAt)
            .HasColumnName("buyer_return_due_at");

        builder.Property(request => request.BuyerShippedAt)
            .HasColumnName("buyer_shipped_at");

        builder.Property(request => request.DeliveredAt)
            .HasColumnName("delivered_at");

        builder.Property(request => request.RefundIssuedAt)
            .HasColumnName("refund_issued_at");

        builder.Property(request => request.ClosedAt)
            .HasColumnName("closed_at");

        builder.Property(request => request.ReturnCarrier)
            .HasColumnName("return_carrier")
            .HasMaxLength(120);

        builder.Property(request => request.TrackingNumber)
            .HasColumnName("tracking_number")
            .HasMaxLength(120);

        builder.OwnsOne(request => request.RefundAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("refund_amount")
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                .HasColumnName("refund_currency")
                .HasMaxLength(3);

            money.HasData(ReturnRequestSeedData.ReturnRequestRefunds);
        });

        builder.OwnsOne(request => request.RestockingFee, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("restocking_fee_amount")
                .HasColumnType("decimal(18,2)");
            money.Property(m => m.Currency)
                .HasColumnName("restocking_fee_currency")
                .HasMaxLength(3);

            money.HasData(ReturnRequestSeedData.ReturnRequestRestockingFees);
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

            money.HasData(ReturnRequestSeedData.ReturnRequestOrderTotals);
        });

        builder.HasIndex(request => request.OrderId).HasDatabaseName("idx_return_requests_order");
        builder.HasIndex(request => request.Status).HasDatabaseName("idx_return_requests_status");

        builder.HasData(ReturnRequestSeedData.ReturnRequests);
    }
}
