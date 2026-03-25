using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Domain.Orders.Constants;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class OrderStatusSeed
{
    internal static readonly Guid DraftId = Guid.Parse("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1");
    internal static readonly Guid AwaitingPaymentId = Guid.Parse("4d128ab1-64a7-4c65-b8f5-434a258f0c52");
    internal static readonly Guid AwaitingShipmentId = Guid.Parse("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91");
    internal static readonly Guid AwaitingShipmentOverdueId = Guid.Parse("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad");
    internal static readonly Guid AwaitingShipmentShipWithin24hId = Guid.Parse("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060");
    internal static readonly Guid AwaitingExpeditedShipmentId = Guid.Parse("859b47f4-0d05-4f43-8ff5-57acb8d5da1d");
    internal static readonly Guid PaidAndShippedId = Guid.Parse("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8");
    internal static readonly Guid PaidAwaitingFeedbackId = Guid.Parse("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9");
    internal static readonly Guid ShippedAwaitingFeedbackId = Guid.Parse("c21a6b64-f0e9-4947-8b1b-38ef45aa4930");
    internal static readonly Guid DeliveredId = Guid.Parse("949ce7f8-6d6b-4d65-9032-b9f51c4508eb");
    internal static readonly Guid DeliveryFailedId = Guid.Parse("970c8d97-6081-43db-9083-8f3c026ded84");
    internal static readonly Guid ArchivedId = Guid.Parse("0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e");
    internal static readonly Guid CancelledId = Guid.Parse("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd");

    internal static IEnumerable<object> Statuses => new[]
    {
        Status(DraftId, OrderStatusCodes.Draft, "Draft", "Order created but not submitted", "#94a3b8", 0),
        Status(AwaitingPaymentId, OrderStatusCodes.AwaitingPayment, "Awaiting payment", "Order awaits buyer payment", "#fb923c", 1),
        Status(AwaitingShipmentId, OrderStatusCodes.AwaitingShipment, "Awaiting shipment", "Payment received; waiting to ship", "#3b82f6", 2),
        Status(AwaitingShipmentOverdueId, OrderStatusCodes.AwaitingShipmentOverdue, "Shipment overdue", "Shipment overdue based on handling time", "#ef4444", 3),
        Status(AwaitingShipmentShipWithin24hId, OrderStatusCodes.AwaitingShipmentShipWithin24h, "Ship within 24h", "Must ship within 24 hours", "#fbbf24", 4),
        Status(AwaitingExpeditedShipmentId, OrderStatusCodes.AwaitingExpeditedShipment, "Expedited shipment", "Expedited shipping requested", "#22c55e", 5),
        Status(PaidAndShippedId, OrderStatusCodes.PaidAndShipped, "Paid & shipped", "Order shipped to buyer", "#10b981", 6),
        Status(PaidAwaitingFeedbackId, OrderStatusCodes.PaidAwaitingFeedback, "Awaiting feedback", "Waiting for buyer feedback", "#a855f7", 7),
        Status(ShippedAwaitingFeedbackId, OrderStatusCodes.ShippedAwaitingFeedback, "Shipped - awaiting feedback", "Shipped and awaiting buyer confirmation", "#38bdf8", 8),
        Status(DeliveryFailedId, OrderStatusCodes.DeliveryFailed, "Delivery failed", "Delivery attempt unsuccessful", "#f97316", 10),
        Status(ArchivedId, OrderStatusCodes.Archived, "Archived", "Order archived", "#64748b", 11),
        Status(CancelledId, OrderStatusCodes.Cancelled, "Cancelled", "Order cancelled", "#ef4444", 12)
    };

    internal static IEnumerable<object> Transitions => new[]
    {
        Transition("8ac18f4b-ea8d-4b72-b6cf-01c3d233cbea", DraftId, AwaitingPaymentId, OrderRoles.Seller, OrderRoles.System),
        Transition("b62c4a77-6a54-47d9-8d09-af22bd0caf23", DraftId, CancelledId, OrderRoles.Seller, OrderRoles.System),
        Transition("42059f6f-8e43-4b6a-9b59-cf9670091b8f", AwaitingPaymentId, AwaitingShipmentId, OrderRoles.System),
        Transition("5c76de08-97eb-43d5-9c01-8c7c6262ec66", AwaitingPaymentId, CancelledId, OrderRoles.Seller, OrderRoles.Buyer, OrderRoles.System),
        Transition("d10a4517-efbb-4b8d-af6f-baf2b022a850", AwaitingShipmentId, AwaitingShipmentShipWithin24hId, OrderRoles.System),
        Transition("3334f1c8-0fb7-4b17-974a-16f4f492ade4", AwaitingShipmentId, AwaitingShipmentOverdueId, OrderRoles.System),
        Transition("94b12ce3-6d7c-4ea1-86f9-72f65e75d8de", AwaitingShipmentId, AwaitingExpeditedShipmentId, OrderRoles.System),
        Transition("ee0a6840-bf0b-46f3-9c41-96b5a91a02ab", AwaitingShipmentId, PaidAndShippedId, OrderRoles.Seller, OrderRoles.System),
        Transition("3cf5a7f5-8f3f-4dcb-907e-e4d27744ef98", AwaitingShipmentId, CancelledId, OrderRoles.Seller, OrderRoles.System),
        Transition("8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e", AwaitingShipmentShipWithin24hId, AwaitingShipmentId, OrderRoles.System),
        Transition("5a3f5769-6c6d-4b89-9347-118bd3fba3d6", AwaitingShipmentShipWithin24hId, AwaitingShipmentOverdueId, OrderRoles.System),
        Transition("55b5fadc-7f2f-4f43-ac4c-c6eb6f633d58", AwaitingShipmentShipWithin24hId, PaidAndShippedId, OrderRoles.Seller, OrderRoles.System),
        Transition("7cf6e659-8025-49e8-94d5-3a4dd3b5a793", AwaitingShipmentOverdueId, AwaitingShipmentShipWithin24hId, OrderRoles.System),
        Transition("1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9", AwaitingShipmentOverdueId, AwaitingShipmentId, OrderRoles.System),
        Transition("2abdffad-037d-48a0-8c3d-a8dd0f00c5ba", AwaitingShipmentOverdueId, PaidAndShippedId, OrderRoles.Seller, OrderRoles.System),
        Transition("64648c83-2c87-47b8-8c2a-32e96c369f41", AwaitingExpeditedShipmentId, PaidAndShippedId, OrderRoles.Seller, OrderRoles.System),
        Transition("c6a927ee-4fb6-48cc-bbf0-d2624de3458f", PaidAndShippedId, PaidAwaitingFeedbackId, OrderRoles.System),
        Transition("d0cb2575-023a-45dc-840a-8e09b2f4c4c8", PaidAndShippedId, ShippedAwaitingFeedbackId, OrderRoles.System),
        Transition("b8fa2c60-13ad-4e83-9516-8f406bcf8414", PaidAndShippedId, DeliveryFailedId, OrderRoles.Seller, OrderRoles.Support, OrderRoles.System),
        Transition("6cb6fa65-3d6c-45f0-9f27-cf5d292743ff", DeliveryFailedId, AwaitingShipmentId, OrderRoles.Seller, OrderRoles.Support, OrderRoles.System),
        Transition("a4c5df71-b5bb-4f13-9659-a5047cf4f087", DeliveryFailedId, CancelledId, OrderRoles.Seller, OrderRoles.Support, OrderRoles.System),
        Transition("6fbe36c4-98e4-4d1d-8c3c-1ea29fe8d08c", PaidAwaitingFeedbackId, ShippedAwaitingFeedbackId, OrderRoles.System),
        Transition("ce68729c-6df0-466b-ae26-737d1b10dd93", ShippedAwaitingFeedbackId, ArchivedId, OrderRoles.System),
        Transition("f5ceb762-3d65-4f6d-b052-053c55c1a08d", CancelledId, ArchivedId, OrderRoles.System)
    };

    private static object Status(Guid id, string code, string name, string description, string color, int sortOrder) => new
    {
        Id = id,
        Code = code,
        Name = name,
        Description = description,
        Color = color,
        SortOrder = sortOrder
    };

    private static object Transition(string key, Guid fromStatusId, Guid toStatusId, params string[] roles) => new
    {
        Id = Guid.Parse(key),
        FromStatusId = fromStatusId,
        ToStatusId = toStatusId,
        AllowedRoles = roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Select(role => role.Trim().ToUpperInvariant())
            .Distinct()
            .ToList()
    };
}
