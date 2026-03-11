using PRN232_EbayClone.Application.Orders.Commands;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record UpdateOrderDeliveryStatusRequest(
    DeliveryStatusUpdate Outcome,
    string? Note
);
