using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;
using static PRN232_EbayClone.Infrastructure.Persistence.Seeds.OrderSeedData;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class ReturnRequestSeedData
{
    internal static readonly Guid ReturnRequest1Id = Guid.Parse("8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf");
    internal static readonly Guid ReturnRequest2Id = Guid.Parse("fd21bed5-6c0c-4bcf-b099-31c8b0d08f27");
    internal static readonly Guid ReturnRequest3Id = Guid.Parse("dc3329e1-14fb-4d00-a395-e76e25a6822b");
    internal static readonly Guid ReturnRequest4Id = Guid.Parse("9a7f6b12-5e2d-4d91-8c22-000000000004");
    internal static readonly Guid ReturnRequest5Id = Guid.Parse("9a7f6b12-5e2d-4d91-8c22-000000000005");

    private static readonly DateTime ReturnRequest1RequestedAt = new DateTime(2025, 10, 29, 10, 0, 0, DateTimeKind.Utc);

    private static readonly DateTime ReturnRequest2RequestedAt = new DateTime(2025, 11, 1, 9, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest2SellerRespondedAt = new DateTime(2025, 11, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest2BuyerDueAt = new DateTime(2025, 11, 5, 23, 59, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest2BuyerShippedAt = new DateTime(2025, 11, 3, 10, 15, 0, DateTimeKind.Utc);

    private static readonly DateTime ReturnRequest3RequestedAt = new DateTime(2025, 11, 4, 17, 45, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest3SellerRespondedAt = new DateTime(2025, 11, 4, 20, 30, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest3BuyerDueAt = new DateTime(2025, 11, 9, 23, 59, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest3BuyerShippedAt = new DateTime(2025, 11, 6, 9, 10, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest3DeliveredAt = new DateTime(2025, 11, 8, 16, 20, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest3RefundIssuedAt = new DateTime(2025, 11, 9, 14, 0, 0, DateTimeKind.Utc);

    private static readonly DateTime ReturnRequest4RequestedAt = new DateTime(2025, 11, 8, 9, 30, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest4SellerRespondedAt = new DateTime(2025, 11, 8, 12, 45, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest4BuyerDueAt = new DateTime(2025, 11, 13, 23, 59, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest4BuyerShippedAt = new DateTime(2025, 11, 10, 10, 20, 0, DateTimeKind.Utc);

    private static readonly DateTime ReturnRequest5RequestedAt = new DateTime(2025, 11, 9, 15, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest5SellerRespondedAt = new DateTime(2025, 11, 9, 17, 15, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest5BuyerDueAt = new DateTime(2025, 11, 14, 23, 59, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest5BuyerShippedAt = new DateTime(2025, 11, 11, 8, 40, 0, DateTimeKind.Utc);
    private static readonly DateTime ReturnRequest5DeliveredAt = new DateTime(2025, 11, 13, 16, 5, 0, DateTimeKind.Utc);

    internal static IEnumerable<object> ReturnRequests => new object[]
    {
        new
        {
            Id = ReturnRequest1Id,
            OrderId = Order7Id,
            BuyerId = BuyerAliceId.Value,
            SellerId = SellerBrianId,
            Reason = ReturnReason.NotAsDescribed,
            PreferredResolution = ReturnResolution.Refund,
            BuyerNote = "Item color differs from the listing photos.",
            SellerNote = (string?)null,
            RequestedAt = ReturnRequest1RequestedAt,
            SellerRespondedAt = (DateTime?)null,
            BuyerReturnDueAt = (DateTime?)null,
            BuyerShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            RefundIssuedAt = (DateTime?)null,
            ClosedAt = (DateTime?)null,
            ReturnCarrier = (string?)null,
            TrackingNumber = (string?)null,
            Status = ReturnStatus.PendingSellerResponse,
            CreatedAt = ReturnRequest1RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = ReturnRequest1RequestedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = ReturnRequest2Id,
            OrderId = Order9Id,
            BuyerId = BuyerAliceId.Value,
            SellerId = SellerCeciliaId,
            Reason = ReturnReason.DamagedOrDefective,
            PreferredResolution = ReturnResolution.Replacement,
            BuyerNote = "Screen arrived cracked; requesting replacement.",
            SellerNote = "Please return using the provided UPS label.",
            RequestedAt = ReturnRequest2RequestedAt,
            SellerRespondedAt = ReturnRequest2SellerRespondedAt,
            BuyerReturnDueAt = ReturnRequest2BuyerDueAt,
            BuyerShippedAt = ReturnRequest2BuyerShippedAt,
            DeliveredAt = (DateTime?)null,
            RefundIssuedAt = (DateTime?)null,
            ClosedAt = (DateTime?)null,
            ReturnCarrier = "UPS",
            TrackingNumber = "1Z999AA10123456784",
            Status = ReturnStatus.InTransitBackToSeller,
            CreatedAt = ReturnRequest2RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = ReturnRequest2BuyerShippedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = ReturnRequest3Id,
            OrderId = Order10Id,
            BuyerId = BuyerBrianId.Value,
            SellerId = SellerCeciliaId,
            Reason = ReturnReason.DoesNotFit,
            PreferredResolution = ReturnResolution.PartialRefund,
            BuyerNote = "Shoes run smaller than expected.",
            SellerNote = "Refunded minus restocking fee.",
            RequestedAt = ReturnRequest3RequestedAt,
            SellerRespondedAt = ReturnRequest3SellerRespondedAt,
            BuyerReturnDueAt = ReturnRequest3BuyerDueAt,
            BuyerShippedAt = ReturnRequest3BuyerShippedAt,
            DeliveredAt = ReturnRequest3DeliveredAt,
            RefundIssuedAt = ReturnRequest3RefundIssuedAt,
            ClosedAt = ReturnRequest3RefundIssuedAt,
            ReturnCarrier = "USPS",
            TrackingNumber = "9405511899223857264837",
            Status = ReturnStatus.RefundCompleted,
            CreatedAt = ReturnRequest3RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = ReturnRequest3RefundIssuedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = ReturnRequest4Id,
            OrderId = Order20Id,
            BuyerId = BuyerAliceId.Value,
            SellerId = SellerCeciliaId,
            Reason = ReturnReason.WrongItemReceived,
            PreferredResolution = ReturnResolution.Exchange,
            BuyerNote = "Received the 64GB variant instead of 128GB.",
            SellerNote = "Exchange approved once return is in transit.",
            RequestedAt = ReturnRequest4RequestedAt,
            SellerRespondedAt = ReturnRequest4SellerRespondedAt,
            BuyerReturnDueAt = ReturnRequest4BuyerDueAt,
            BuyerShippedAt = ReturnRequest4BuyerShippedAt,
            DeliveredAt = (DateTime?)null,
            RefundIssuedAt = (DateTime?)null,
            ClosedAt = (DateTime?)null,
            ReturnCarrier = "FedEx",
            TrackingNumber = "612999AA10NEWRT4",
            Status = ReturnStatus.InTransitBackToSeller,
            CreatedAt = ReturnRequest4RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = ReturnRequest4BuyerShippedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = ReturnRequest5Id,
            OrderId = Order23Id,
            BuyerId = BuyerBrianId.Value,
            SellerId = SellerCeciliaId,
            Reason = ReturnReason.ChangedMind,
            PreferredResolution = ReturnResolution.Refund,
            BuyerNote = "Decided to keep a different model instead.",
            SellerNote = "Refund pending inspection of returned item.",
            RequestedAt = ReturnRequest5RequestedAt,
            SellerRespondedAt = ReturnRequest5SellerRespondedAt,
            BuyerReturnDueAt = ReturnRequest5BuyerDueAt,
            BuyerShippedAt = ReturnRequest5BuyerShippedAt,
            DeliveredAt = ReturnRequest5DeliveredAt,
            RefundIssuedAt = (DateTime?)null,
            ClosedAt = (DateTime?)null,
            ReturnCarrier = "USPS",
            TrackingNumber = "9405511899223857264999",
            Status = ReturnStatus.RefundPending,
            CreatedAt = ReturnRequest5RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = ReturnRequest5DeliveredAt,
            UpdatedBy = "seed",
            IsDeleted = false
        }
    };

    internal static IEnumerable<object> ReturnRequestOrderTotals => new object[]
    {
        new { ReturnRequestId = ReturnRequest1Id, Amount = 108.88m, Currency = "USD" },
        new { ReturnRequestId = ReturnRequest2Id, Amount = 169.27m, Currency = "USD" },
        new { ReturnRequestId = ReturnRequest3Id, Amount = 177.37m, Currency = "USD" },
        new { ReturnRequestId = ReturnRequest4Id, Amount = 80.59m, Currency = "USD" },
        new { ReturnRequestId = ReturnRequest5Id, Amount = 112.57m, Currency = "USD" }
    };

    internal static IEnumerable<object> ReturnRequestRefunds => new object[]
    {
        new { ReturnRequestId = ReturnRequest3Id, Amount = 150.00m, Currency = "USD" }
    };

    internal static IEnumerable<object> ReturnRequestRestockingFees => new object[]
    {
        new { ReturnRequestId = ReturnRequest3Id, Amount = 5.00m, Currency = "USD" }
    };
}
