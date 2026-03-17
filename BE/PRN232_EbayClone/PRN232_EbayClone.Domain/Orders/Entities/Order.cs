using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public class Order : AggregateRoot<Guid>
{
    private const int MaxTrackingNumbersPerOrder = 5;

    public string OrderNumber { get; private set; } = null!;
    public UserId BuyerId { get; private set; }
    public Guid SellerId { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItemShipment> _itemShipments = new();
    public IReadOnlyCollection<OrderItemShipment> ItemShipments => _itemShipments.AsReadOnly();
    public BuyerFeedback? SellerFeedback { get; private set; }

    public Money SubTotal { get; private set; } = null!;
    public Money ShippingCost { get; private set; } = null!;
    public Money PlatformFee { get; private set; } = null!;
    public Money TaxAmount { get; private set; } = null!;
    public Money DiscountAmount { get; private set; } = null!;
    public Money Total { get; private set; } = null!;
    // Status
    public OrderStatus Status { get; private set; } = null!;
    private readonly List<OrderStatusHistory> _statusHistory = new();
    public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory.AsReadOnly();
    public ShippingStatus ShippingStatus { get; private set; }
    public FulfillmentType FulfillmentType { get; private set; }
    private readonly List<CancellationRequest> _cancellationRequests = new();
    public IReadOnlyCollection<CancellationRequest> CancellationRequests => _cancellationRequests.AsReadOnly();
    public CancellationRequest? ActiveCancellationRequest => _cancellationRequests
        .Where(request => !request.IsClosed)
        .OrderByDescending(request => request.RequestedAt)
        .FirstOrDefault();

    private readonly List<ReturnRequest> _returnRequests = new();
    public IReadOnlyCollection<ReturnRequest> ReturnRequests => _returnRequests.AsReadOnly();
    public ReturnRequest? ActiveReturnRequest => _returnRequests
        .Where(request => !request.IsClosed)
        .OrderByDescending(request => request.RequestedAt)
        .FirstOrDefault();

    public DateTime OrderedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? ArchivedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public string? CouponCode { get; private set; }
    public Guid? PromotionId { get; private set; }

    public User Buyer { get; private set; } = null!;

    private Order(Guid id) : base(id) { }

    public static Result<Order> CreateDraft(UserId buyerId, Guid sellerId, FulfillmentType fulfillmentType, string currency, OrderStatus draftStatus)
    {
        if (sellerId == Guid.Empty) return Error.Failure("Order.InvalidSellerId", "SellerId cannot be empty.");

        var zeroMoneyResult = Money.Zero(currency);
        if (zeroMoneyResult.IsFailure) return zeroMoneyResult.Error;
        var orderNumber = GenerateOrderNumber();
        var order = new Order(Guid.NewGuid())
        {
            OrderNumber = orderNumber,
            BuyerId = buyerId,
            SellerId = sellerId,
            Status = draftStatus,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = fulfillmentType,
            OrderedAt = DateTime.UtcNow,
            SubTotal = zeroMoneyResult.Value,
            ShippingCost = zeroMoneyResult.Value,
            PlatformFee = zeroMoneyResult.Value,
            TaxAmount = zeroMoneyResult.Value,
            DiscountAmount = zeroMoneyResult.Value,
            Total = zeroMoneyResult.Value,
            DeliveredAt = null
        };

        return order;
    }
    public static string GenerateOrderNumber()
    {
        var prefix = "ORD";
        var uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
        return $"{prefix}-{uniquePart}";
    }
    public Result AddItem(OrderItem item)
    {
        if (item.Quantity <= 0) return Error.Failure("OrderItem.InvalidQuantity", "Quantity must be greater than zero.");
        if (_items.Any(x => x.ListingId == item.ListingId))
            return Error.Failure("OrderItem.DuplicateItem", "Item already exists in order.");

        _items.Add(item);

        var recalcResult = RecalculateTotals();
        if (recalcResult.IsFailure) return recalcResult.Error;

        UpdateShippingStatusFromShipments();
        return Result.Success();
    }

    public Result RemoveItem(Guid listingId)
    {
        var item = _items.FirstOrDefault(x => x.ListingId == listingId);
        if (item == null) return Error.Failure("OrderItem.NotFound", "Item not found in order.");

        _items.Remove(item);

        var recalcResult = RecalculateTotals();
        if (recalcResult.IsFailure) return recalcResult.Error;

        UpdateShippingStatusFromShipments();
        return Result.Success();
    }

    public Result<OrderItemShipment> AddShipment(
        Guid orderItemId,
        string carrier,
        string trackingNumber,
        DateTimeOffset shippedAt,
        Guid? shippingLabelId)
    {
        var item = _items.FirstOrDefault(i => i.Id == orderItemId);
        if (item is null)
        {
            return Result.Failure<OrderItemShipment>(Error.Failure("Order.Shipment.ItemNotFound", "Cannot record shipment for an item that does not belong to the order."));
        }

        var normalizedTracking = trackingNumber?.Trim() ?? string.Empty;
        if (normalizedTracking.Length == 0)
        {
            return Result.Failure<OrderItemShipment>(Error.Failure("Order.Shipment.InvalidTracking", "Tracking number is required."));
        }

        shippedAt = NormalizeShipDate(shippedAt);

        var shipmentsForItem = _itemShipments
            .Where(s => s.OrderItemId == orderItemId)
            .OrderByDescending(s => s.CreatedAt)
            .ToList();

        var existingShipmentForItem = shipmentsForItem.FirstOrDefault(s =>
            string.Equals(s.TrackingNumber, normalizedTracking, StringComparison.OrdinalIgnoreCase));

        if (existingShipmentForItem is not null)
        {
            var updateExisting = existingShipmentForItem.Update(normalizedTracking, carrier, shippedAt, shippingLabelId);
            if (updateExisting.IsFailure)
            {
                return Result.Failure<OrderItemShipment>(updateExisting.Error);
            }

            UpdateShippingStatusFromShipments();
            return Result.Success(existingShipmentForItem);
        }

        var trackingAlreadyOnOrder = _itemShipments.Any(s =>
            string.Equals(s.TrackingNumber, normalizedTracking, StringComparison.OrdinalIgnoreCase));

        if (!trackingAlreadyOnOrder)
        {
            var distinctTrackingCount = _itemShipments
                .Select(s => s.TrackingNumber)
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            if (distinctTrackingCount >= MaxTrackingNumbersPerOrder)
            {
                return Result.Failure<OrderItemShipment>(Error.Failure(
                    "Order.Shipment.LimitExceeded",
                    $"An order can only have up to {MaxTrackingNumbersPerOrder} tracking numbers."));
            }
        }

        var createResult = OrderItemShipment.Create(Id, orderItemId, normalizedTracking, carrier, shippedAt, shippingLabelId);
        if (createResult.IsFailure)
        {
            return createResult;
        }

        _itemShipments.Add(createResult.Value);
        UpdateShippingStatusFromShipments();
        return createResult;
    }

    public Result UpdateShipment(
        Guid shipmentId,
        string carrier,
        string trackingNumber,
        DateTimeOffset shippedAt,
        Guid? shippingLabelId)
    {
        var shipment = _itemShipments.FirstOrDefault(s => s.Id == shipmentId);
        if (shipment is null)
        {
            return Error.Failure("Order.Shipment.NotFound", "Shipment was not found on this order.");
        }

        var item = _items.FirstOrDefault(i => i.Id == shipment.OrderItemId);
        if (item is null)
        {
            return Error.Failure("Order.Shipment.ItemNotFound", "Shipment is associated with an unknown order item.");
        }

        shippedAt = NormalizeShipDate(shippedAt);

        var updateResult = shipment.Update(trackingNumber, carrier, shippedAt, shippingLabelId);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        UpdateShippingStatusFromShipments();
        return Result.Success();
    }

    public Result RemoveShipment(Guid shipmentId)
    {
        var shipment = _itemShipments.FirstOrDefault(s => s.Id == shipmentId);
        if (shipment is null)
        {
            return Error.Failure("Order.Shipment.NotFound", "Shipment was not found on this order.");
        }

        _itemShipments.Remove(shipment);
        UpdateShippingStatusFromShipments();
        return Result.Success();
    }

    public Result ClearShipmentsForItem(Guid orderItemId)
    {
        if (_items.All(i => i.Id != orderItemId))
        {
            return Error.Failure("Order.Shipment.ItemNotFound", "Order item not found.");
        }

        _itemShipments.RemoveAll(s => s.OrderItemId == orderItemId);
        UpdateShippingStatusFromShipments();
        return Result.Success();
    }

    public Result<BuyerFeedback> RecordSellerFeedback(
        string comment,
        bool usesStoredComment,
        string? storedCommentKey,
        DateTimeOffset createdAt)
    {
        if (SellerFeedback is not null)
        {
            return Result.Failure<BuyerFeedback>(
                Error.Failure("Order.FeedbackExists", "Feedback has already been left for this order."));
        }

        var createResult = BuyerFeedback.Create(
            Id,
            SellerId,
            BuyerId,
            comment,
            usesStoredComment,
            storedCommentKey,
            null, // starRating
            createdAt);

        if (createResult.IsFailure)
        {
            return Result.Failure<BuyerFeedback>(createResult.Error);
        }

        SellerFeedback = createResult.Value;
        return Result.Success(SellerFeedback);
    }

    public bool HasShipmentsForAllItems()
    {
        if (_items.Count == 0)
        {
            return false;
        }

        var shippedItemCount = _itemShipments
            .Select(s => s.OrderItemId)
            .Distinct()
            .Count();

        return shippedItemCount >= _items.Count;
    }

    public Result ChangeStatus(OrderStatus targetStatus, string userRole)
    {
        userRole = userRole?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userRole))
        {
            return Error.Failure("Order.InvalidRole", "A role is required to change order status.");
        }

        if (Status is not null && Status.Id == targetStatus.Id)
        {
            return Result.Success();
        }

        if (Status is not null && !Status.CanTransitionTo(targetStatus, userRole))
            return Error.Failure("Order.InvalidStatus", $"Role '{userRole}' cannot change from '{Status.Name}' to '{targetStatus.Name}'");

        var previousStatus = Status;
        Status = targetStatus;

        if (previousStatus is not null)
        {
            _statusHistory.Add(new OrderStatusHistory(this.Id, previousStatus, targetStatus));
        }

        UpdateTimeline(previousStatus, targetStatus);

        return Result.Success();
    }

    // --- Money ---
    private Result RecalculateTotals()
    {
        var zeroResult = Money.Zero(SubTotal.Currency);
        if (zeroResult.IsFailure) return zeroResult.Error;

        var subTotal = zeroResult.Value;

        foreach (var item in _items)
        {
            var updated = subTotal + item.TotalPrice;
            if (updated.IsFailure) return updated.Error;
            subTotal = updated.Value;
        }

        SubTotal = subTotal;

        var totalResult = subTotal + ShippingCost;
        if (totalResult.IsFailure) return totalResult.Error;

        totalResult = totalResult.Value + PlatformFee;
        if (totalResult.IsFailure) return totalResult.Error;

        totalResult = totalResult.Value + TaxAmount;
        if (totalResult.IsFailure) return totalResult.Error;

        var discountResult = totalResult.Value - DiscountAmount;
        if (discountResult.IsFailure) return discountResult.Error;

        if (discountResult.Value.Amount < 0)
        {
            return Error.Failure("Order.NegativeTotal", "Discount amount cannot exceed the order total.");
        }

        Total = discountResult.Value;

        return Result.Success();
    }

    public Result SetShippingCost(Money cost)
    {
        ShippingCost = cost;
        return RecalculateTotals();
    }

    public Result SetPlatformFee(Money fee)
    {
        PlatformFee = fee;
        return RecalculateTotals();
    }

    public Result SetTaxAmount(Money tax)
    {
        TaxAmount = tax;
        return RecalculateTotals();
    }

    public Result AddCancellationRequest(CancellationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.OrderId != Id)
        {
            return Error.Failure("Order.CancellationRequestMismatch", "Cancellation request does not belong to this order.");
        }

        if (_cancellationRequests.Any(existing => !existing.IsClosed))
        {
            return Error.Failure("Order.CancellationRequestExists", "There is already an open cancellation request for this order.");
        }

        _cancellationRequests.Add(request);

        return Result.Success();
    }

    public Result AddReturnRequest(ReturnRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.OrderId != Id)
        {
            return Error.Failure("Order.ReturnRequestMismatch", "Return request does not belong to this order.");
        }

        if (_returnRequests.Any(existing => !existing.IsClosed))
        {
            return Error.Failure("Order.ReturnRequestExists", "There is already an open return request for this order.");
        }

        _returnRequests.Add(request);

        return Result.Success();
    }

    public Result ApplyDiscount(Money discount)
    {
        var currencyCheck = EnsureCurrency(discount);
        if (currencyCheck.IsFailure) return currencyCheck.Error;

        if (discount.Amount < 0)
        {
            return Error.Failure("Order.InvalidDiscount", "Discount amount cannot be negative.");
        }

        DiscountAmount = discount;
        return RecalculateTotals();
    }

    private Result EnsureCurrency(Money money)
    {
        if (!string.Equals(money.Currency, SubTotal.Currency, StringComparison.OrdinalIgnoreCase))
        {
            return Error.Failure("Order.CurrencyMismatch", $"Expected currency '{SubTotal.Currency}' but received '{money.Currency}'.");
        }

        return Result.Success();
    }

    private void UpdateShippingStatusFromShipments()
    {
        var statusCode = Status?.Code ?? string.Empty;

        if (string.Equals(statusCode, OrderStatusCodes.Cancelled, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.DeliveryFailed, StringComparison.OrdinalIgnoreCase))
        {
            ShippingStatus = ShippingStatus.Returned;
            return;
        }

        if (_items.Count == 0 || _itemShipments.Count == 0)
        {
            ShippingStatus = ShippingStatus.Pending;
            ShippedAt = null;
            return;
        }

        var firstShipmentDate = _itemShipments.Min(s => s.ShippedAt).UtcDateTime;
        var shippedItemCount = _itemShipments
            .Select(s => s.OrderItemId)
            .Distinct()
            .Count();

        if (shippedItemCount >= _items.Count)
        {
            ShippingStatus = ShippingStatus.Shipped;
            ShippedAt = firstShipmentDate;
            return;
        }

        ShippingStatus = ShippingStatus.InTransit;
        ShippedAt = firstShipmentDate;
    }

    private void UpdateTimeline(OrderStatus? previousStatus, OrderStatus targetStatus)
    {
        var statusCode = targetStatus.Code;

        if (string.Equals(statusCode, OrderStatusCodes.AwaitingPayment, StringComparison.OrdinalIgnoreCase))
        {
            DeliveredAt = null;
        }

        if (string.Equals(statusCode, OrderStatusCodes.AwaitingShipment, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.AwaitingShipmentShipWithin24h, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.AwaitingShipmentOverdue, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.AwaitingExpeditedShipment, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.PaidAwaitingFeedback, StringComparison.OrdinalIgnoreCase))
        {
            PaidAt ??= DateTime.UtcNow;
            DeliveredAt = null;
        }

        if (string.Equals(statusCode, OrderStatusCodes.PaidAndShipped, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(statusCode, OrderStatusCodes.ShippedAwaitingFeedback, StringComparison.OrdinalIgnoreCase))
        {
            PaidAt ??= DateTime.UtcNow;
            ShippedAt ??= DateTime.UtcNow;
        }

        if (string.Equals(statusCode, OrderStatusCodes.PaidAndShipped, StringComparison.OrdinalIgnoreCase))
        {
            PaidAt ??= DateTime.UtcNow;
            ShippedAt ??= DateTime.UtcNow;
            DeliveredAt ??= DateTime.UtcNow;
        }

        if (string.Equals(statusCode, OrderStatusCodes.DeliveryFailed, StringComparison.OrdinalIgnoreCase))
        {
            DeliveredAt = null;
        }

        if (string.Equals(statusCode, OrderStatusCodes.Archived, StringComparison.OrdinalIgnoreCase))
        {
            ArchivedAt ??= DateTime.UtcNow;
        }

        if (string.Equals(statusCode, OrderStatusCodes.Cancelled, StringComparison.OrdinalIgnoreCase))
        {
            CancelledAt ??= DateTime.UtcNow;
            DeliveredAt = null;
        }

        if (string.Equals(statusCode, OrderStatusCodes.Draft, StringComparison.OrdinalIgnoreCase) && previousStatus is not null)
        {
            PaidAt = null;
            ShippedAt = null;
            CancelledAt = null;
            ArchivedAt = null;
            DeliveredAt = null;
        }

        UpdateShippingStatusFromShipments();
    }

    private static DateTimeOffset NormalizeShipDate(DateTimeOffset shipDate) =>
        shipDate == default ? DateTimeOffset.UtcNow : shipDate;
}
