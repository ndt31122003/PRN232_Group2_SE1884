using System;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public sealed class OrderItemShipment
{
    private OrderItemShipment()
    {
    }

    private OrderItemShipment(
        Guid orderId,
        Guid orderItemId,
        string trackingNumber,
        string carrier,
        DateTimeOffset shippedAt,
        Guid? shippingLabelId,
        DateTimeOffset createdAtUtc,
        DateTimeOffset? updatedAtUtc)
    {
        OrderId = orderId;
        OrderItemId = orderItemId;
        TrackingNumber = trackingNumber;
        Carrier = carrier;
        ShippedAt = shippedAt;
        ShippingLabelId = shippingLabelId;
        CreatedAt = createdAtUtc;
        UpdatedAt = updatedAtUtc;
    }
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }

    public Guid OrderItemId { get; private set; }

    public Guid? ShippingLabelId { get; private set; }

    public string TrackingNumber { get; private set; } = string.Empty;

    public string Carrier { get; private set; } = string.Empty;

    public DateTimeOffset ShippedAt { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }
    public bool IsDeleted { get; set; } = false;

    public static Result<OrderItemShipment> Create(
        Guid orderId,
        Guid orderItemId,
        string trackingNumber,
        string carrier,
        DateTimeOffset shippedAt,
        Guid? shippingLabelId)
    {
        trackingNumber = trackingNumber?.Trim() ?? string.Empty;
        if (trackingNumber.Length == 0)
        {
            return Error.Failure("Order.Shipment.InvalidTracking", "Tracking number is required.");
        }

        carrier = carrier?.Trim() ?? string.Empty;
        if (carrier.Length == 0)
        {
            return Error.Failure("Order.Shipment.InvalidCarrier", "Carrier is required.");
        }

        var now = DateTimeOffset.UtcNow;
        var shipment = new OrderItemShipment(
            orderId,
            orderItemId,
            trackingNumber,
            carrier,
            shippedAt,
            shippingLabelId,
            now,
            null);

        return Result.Success(shipment);
    }

    public Result Update(
        string trackingNumber,
        string carrier,
        DateTimeOffset shippedAt,
        Guid? shippingLabelId)
    {
        trackingNumber = trackingNumber?.Trim() ?? string.Empty;
        if (trackingNumber.Length == 0)
        {
            return Error.Failure("Order.Shipment.InvalidTracking", "Tracking number is required.");
        }

        carrier = carrier?.Trim() ?? string.Empty;
        if (carrier.Length == 0)
        {
            return Error.Failure("Order.Shipment.InvalidCarrier", "Carrier is required.");
        }

        TrackingNumber = trackingNumber;
        Carrier = carrier;
        ShippedAt = shippedAt;
        ShippingLabelId = shippingLabelId;
        UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success();
    }
}