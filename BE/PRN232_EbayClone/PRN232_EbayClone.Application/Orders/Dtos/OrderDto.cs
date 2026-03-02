using PRN232_EbayClone.Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record OrderDto(
    Guid Id,
    string OrderNumber,
    UserId BuyerId,
    string BuyerUsername,
    string BuyerFullName,
    Guid SellerId,
    IReadOnlyCollection<OrderItemDto> Items,
    decimal? SubTotal,
    decimal? ShippingCost,
    decimal? PlatformFee,
    decimal? TaxAmount,
    decimal? DiscountAmount,
    decimal? Total,
    string StatusCode,
    string StatusName,
    string StatusColor,
    string ShippingStatus,
    string FulfillmentType,
    DateTime OrderedAt,
    DateTime? PaidAt,
    DateTime? ShippedAt,
    DateTime? CancelledAt,
    DateTime? ArchivedAt,
    IReadOnlyCollection<OrderStatusHistoryDto> StatusHistory,
    IReadOnlyCollection<OrderShipmentDto> Shipments,
    SellerFeedbackDto? SellerFeedback
);
