using System;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderStatusUpdateResult(
    Guid OrderId,
    string StatusCode,
    string StatusName,
    string StatusColor,
    ShippingStatus ShippingStatus,
    DateTime? PaidAt,
    DateTime? ShippedAt,
    DateTime? DeliveredAt
);
