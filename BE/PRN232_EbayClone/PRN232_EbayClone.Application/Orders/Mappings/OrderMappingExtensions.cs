using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Mappings;

internal static class OrderMappingExtensions
{
    public static OrderDto ToDto(this Order order)
    {
        var items = order.Items
            .Select(ToDto)
            .ToList();

        var history = order.StatusHistory
            .OrderByDescending(h => h.ChangedAt)
            .Select(ToDto)
            .ToList();

        var shipments = order.ItemShipments
            .OrderBy(s => s.ShippedAt)
            .Select(ToDto)
            .ToList();

        var feedback = order.SellerFeedback?.ToDto();

        return new OrderDto(
            order.Id,
            order.OrderNumber,
            order.BuyerId,
            order.Buyer?.Username ?? string.Empty,
            order.Buyer?.FullName ?? string.Empty,
            order.SellerId,
            items,
            order.SubTotal,
            order.ShippingCost,
            order.PlatformFee,
            order.TaxAmount,
            order.DiscountAmount,
            order.Total,
            order.Status.Code,
            order.Status.Name,
            order.Status.Color,
            order.ShippingStatus.ToString(),
            order.FulfillmentType.ToString(),
            order.OrderedAt,
            order.PaidAt,
            order.ShippedAt,
            order.CancelledAt,
            order.ArchivedAt,
            history,
            shipments,
            feedback);
    }

    private static OrderItemDto ToDto(this OrderItem item) =>
        new(
            item.Id,
            item.ListingId,
            item.VariationId,
            item.Title,
            item.ImageUrl,
            item.Sku,
            item.Quantity,
            item.UnitPrice,
            item.TotalPrice);

    private static OrderStatusHistoryDto ToDto(this OrderStatusHistory history)
        => new(
            history.FromStatus?.Code ?? string.Empty,
            history.FromStatus?.Name ?? string.Empty,
            history.ToStatus.Code,
            history.ToStatus.Name,
            history.ChangedAt);

    private static MoneyDto ToDto(this Money money) =>
        new(money.Amount, money.Currency);

    private static OrderShipmentDto ToDto(this OrderItemShipment shipment) =>
        new(
            shipment.Id,
            shipment.OrderItemId,
            shipment.TrackingNumber,
            shipment.Carrier,
            shipment.ShippedAt,
            shipment.ShippingLabelId,
            shipment.CreatedAt,
            shipment.UpdatedAt);

    private static SellerFeedbackDto ToDto(this Domain.Orders.Entities.BuyerFeedback feedback) =>
        new(
            feedback.Id,
            feedback.OrderId,
            feedback.SellerId,
            feedback.BuyerId.Value,
            feedback.Comment,
            feedback.UsesStoredComment,
            feedback.StoredCommentKey,
            feedback.CreatedAt,
            feedback.FollowUpComment,
            feedback.FollowUpCommentedAt);
}
