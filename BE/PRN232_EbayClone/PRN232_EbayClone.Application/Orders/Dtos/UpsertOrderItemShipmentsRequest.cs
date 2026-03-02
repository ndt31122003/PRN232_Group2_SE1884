using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record UpsertOrderItemShipmentsRequest(
    IReadOnlyCollection<OrderItemShipmentDraft> Shipments,
    IReadOnlyCollection<Guid>? ClearedOrderItemIds);

public sealed record OrderItemShipmentDraft(
    Guid? ShipmentId,
    Guid OrderItemId,
    string TrackingNumber,
    string Carrier,
    DateTimeOffset? ShippedAt,
    Guid? ShippingLabelId);