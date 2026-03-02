using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderShipmentDto(
    Guid Id,
    Guid OrderItemId,
    string TrackingNumber,
    string Carrier,
    DateTimeOffset ShippedAt,
    Guid? ShippingLabelId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);