using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;
using static PRN232_EbayClone.Infrastructure.Persistence.Seeds.OrderSeedData;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class CancellationRequestSeedData
{
    internal static readonly Guid CancellationRequest1Id = Guid.Parse("6f1f9f0c-898f-4c7b-bb38-1b689e9f7331");
    internal static readonly Guid CancellationRequest2Id = Guid.Parse("d3f7d907-6b71-47d8-8651-922629540277");
    internal static readonly Guid CancellationRequest3Id = Guid.Parse("c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753");
    internal static readonly Guid CancellationRequest4Id = Guid.Parse("5d4e7a11-0c4e-4a6f-9f2f-000000000004");
    internal static readonly Guid CancellationRequest5Id = Guid.Parse("5d4e7a11-0c4e-4a6f-9f2f-000000000005");

    private static readonly DateTime CancellationRequest1RequestedAt = new DateTime(2025, 10, 13, 14, 15, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest1Deadline = new DateTime(2025, 10, 15, 14, 15, 0, DateTimeKind.Utc);

    private static readonly DateTime CancellationRequest2RequestedAt = new DateTime(2025, 10, 19, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest2SellerRespondedAt = new DateTime(2025, 10, 19, 18, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest2Deadline = new DateTime(2025, 10, 21, 12, 0, 0, DateTimeKind.Utc);

    private static readonly DateTime CancellationRequest3RequestedAt = new DateTime(2025, 10, 30, 9, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest4RequestedAt = new DateTime(2025, 11, 6, 18, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest4SellerRespondedAt = new DateTime(2025, 11, 7, 9, 30, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest4Deadline = new DateTime(2025, 11, 8, 18, 0, 0, DateTimeKind.Utc);

    private static readonly DateTime CancellationRequest5RequestedAt = new DateTime(2025, 11, 8, 11, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime CancellationRequest5Deadline = new DateTime(2025, 11, 10, 11, 0, 0, DateTimeKind.Utc);

    internal static IEnumerable<object> CancellationRequests => new object[]
    {
        new
        {
            Id = CancellationRequest1Id,
            OrderId = Order2Id,
            BuyerId = BuyerBrianId.Value,
            SellerId = SellerAliceId,
            InitiatedBy = CancellationInitiator.Buyer,
            Reason = CancellationReason.BuyerChangedMind,
            BuyerNote = "Realized I ordered the wrong variation, please cancel.",
            SellerNote = (string?)null,
            RequestedAt = CancellationRequest1RequestedAt,
            SellerResponseDeadlineUtc = CancellationRequest1Deadline,
            SellerRespondedAt = (DateTime?)null,
            AutoClosedAt = (DateTime?)null,
            CompletedAt = (DateTime?)null,
            Status = CancellationStatus.PendingSellerResponse,
            CreatedAt = CancellationRequest1RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = CancellationRequest1RequestedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = CancellationRequest2Id,
            OrderId = Order3Id,
            BuyerId = BuyerBrianId.Value,
            SellerId = SellerAliceId,
            InitiatedBy = CancellationInitiator.Buyer,
            Reason = CancellationReason.IncorrectAddress,
            BuyerNote = "Need to update the delivery address; requesting cancellation.",
            SellerNote = "Approved – refund processing with payment provider.",
            RequestedAt = CancellationRequest2RequestedAt,
            SellerResponseDeadlineUtc = CancellationRequest2Deadline,
            SellerRespondedAt = CancellationRequest2SellerRespondedAt,
            AutoClosedAt = (DateTime?)null,
            CompletedAt = (DateTime?)null,
            Status = CancellationStatus.AwaitingRefund,
            CreatedAt = CancellationRequest2RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = CancellationRequest2SellerRespondedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = CancellationRequest3Id,
            OrderId = Order6Id,
            BuyerId = BuyerCeciliaId.Value,
            SellerId = SellerBrianId,
            InitiatedBy = CancellationInitiator.System,
            Reason = CancellationReason.Other,
            BuyerNote = (string?)null,
            SellerNote = "Order auto-cancelled after missing shipping deadline.",
            RequestedAt = CancellationRequest3RequestedAt,
            SellerResponseDeadlineUtc = (DateTime?)null,
            SellerRespondedAt = (DateTime?)null,
            AutoClosedAt = CancellationRequest3RequestedAt,
            CompletedAt = CancellationRequest3RequestedAt,
            Status = CancellationStatus.AutoCancelled,
            CreatedAt = CancellationRequest3RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = CancellationRequest3RequestedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = CancellationRequest4Id,
            OrderId = Order18Id,
            BuyerId = BuyerCeciliaId.Value,
            SellerId = SellerAliceId,
            InitiatedBy = CancellationInitiator.Buyer,
            Reason = CancellationReason.OutOfStock,
            BuyerNote = "Item still not handed to carrier, requesting cancellation.",
            SellerNote = "Approved – refund issued to buyer's original payment method.",
            RequestedAt = CancellationRequest4RequestedAt,
            SellerResponseDeadlineUtc = CancellationRequest4Deadline,
            SellerRespondedAt = CancellationRequest4SellerRespondedAt,
            AutoClosedAt = (DateTime?)null,
            CompletedAt = (DateTime?)null,
            Status = CancellationStatus.AwaitingRefund,
            CreatedAt = CancellationRequest4RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = CancellationRequest4SellerRespondedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = CancellationRequest5Id,
            OrderId = Order21Id,
            BuyerId = BuyerBrianId.Value,
            SellerId = SellerAliceId,
            InitiatedBy = CancellationInitiator.Buyer,
            Reason = CancellationReason.BuyerChangedMind,
            BuyerNote = "Accidentally placed duplicate order.",
            SellerNote = (string?)null,
            RequestedAt = CancellationRequest5RequestedAt,
            SellerResponseDeadlineUtc = CancellationRequest5Deadline,
            SellerRespondedAt = (DateTime?)null,
            AutoClosedAt = (DateTime?)null,
            CompletedAt = (DateTime?)null,
            Status = CancellationStatus.PendingSellerResponse,
            CreatedAt = CancellationRequest5RequestedAt,
            CreatedBy = "seed",
            UpdatedAt = CancellationRequest5RequestedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        }
    };

    internal static IEnumerable<object> CancellationRequestOrderTotals => new object[]
    {
        new { CancellationRequestId = CancellationRequest1Id, Amount = 114.69m, Currency = "USD" },
        new { CancellationRequestId = CancellationRequest2Id, Amount = 87.83m, Currency = "USD" },
        new { CancellationRequestId = CancellationRequest3Id, Amount = 97.37m, Currency = "USD" },
        new { CancellationRequestId = CancellationRequest4Id, Amount = 107.23m, Currency = "USD" },
        new { CancellationRequestId = CancellationRequest5Id, Amount = 74.43m, Currency = "USD" }
    };

    internal static IEnumerable<object> CancellationRequestRefunds => new object[]
    {
        new { CancellationRequestId = CancellationRequest2Id, Amount = 87.83m, Currency = "USD" },
        new { CancellationRequestId = CancellationRequest4Id, Amount = 107.23m, Currency = "USD" }
    };
}
