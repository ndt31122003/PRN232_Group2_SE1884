using System;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public class ReturnRequest : AggregateRoot<Guid>
{
    private ReturnRequest(Guid id) : base(id) { }

    public Guid OrderId { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public ReturnReason Reason { get; private set; }
    public ReturnResolution PreferredResolution { get; private set; }
    public string? BuyerNote { get; private set; }
    public string? SellerNote { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? SellerRespondedAt { get; private set; }
    public DateTime? BuyerReturnDueAt { get; private set; }
    public DateTime? BuyerShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? RefundIssuedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public string? ReturnCarrier { get; private set; }
    public string? TrackingNumber { get; private set; }
    public ReturnStatus Status { get; private set; }
    public Money? RefundAmount { get; private set; }
    public Money? RestockingFee { get; private set; }
    public Money OrderTotalSnapshot { get; private set; } = null!;
    public Order Order { get; private set; } = null!;

    public bool IsClosed => Status is ReturnStatus.Closed or ReturnStatus.RefundCompleted or ReturnStatus.SellerDeclined;
    public bool RequiresSellerAction => Status is ReturnStatus.PendingSellerResponse or ReturnStatus.DeliveredToSeller or ReturnStatus.RefundPending;

    public static Result<ReturnRequest> Create(
        Guid orderId,
        Guid buyerId,
        Guid sellerId,
        ReturnReason reason,
        ReturnResolution preferredResolution,
        Money orderTotalSnapshot,
        string? buyerNote,
        DateTime requestedAtUtc)
    {
        if (orderId == Guid.Empty)
        {
            return Error.Failure("ReturnRequest.InvalidOrderId", "OrderId cannot be empty.");
        }

        if (buyerId == Guid.Empty)
        {
            return Error.Failure("ReturnRequest.InvalidBuyerId", "BuyerId cannot be empty.");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Failure("ReturnRequest.InvalidSellerId", "SellerId cannot be empty.");
        }

        if (requestedAtUtc.Kind != DateTimeKind.Utc)
        {
            requestedAtUtc = DateTime.SpecifyKind(requestedAtUtc, DateTimeKind.Utc);
        }

        var request = new ReturnRequest(Guid.NewGuid())
        {
            OrderId = orderId,
            BuyerId = buyerId,
            SellerId = sellerId,
            Reason = reason,
            PreferredResolution = preferredResolution,
            BuyerNote = buyerNote?.Trim(),
            RequestedAt = requestedAtUtc,
            Status = ReturnStatus.PendingSellerResponse,
            OrderTotalSnapshot = orderTotalSnapshot
        };

        return Result.Success(request);
    }

    public Result UpdateSellerNote(string? note)
    {
        SellerNote = note?.Trim();
        return Result.Success();
    }

    public Result Approve(
        DateTime respondedAtUtc,
        DateTime? buyerReturnDueAtUtc,
        string? sellerNote,
        Money? restockingFee)
    {
        if (Status != ReturnStatus.PendingSellerResponse)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return request cannot be approved in the current status.");
        }

        if (respondedAtUtc.Kind != DateTimeKind.Utc)
        {
            respondedAtUtc = DateTime.SpecifyKind(respondedAtUtc, DateTimeKind.Utc);
        }

        if (buyerReturnDueAtUtc.HasValue && buyerReturnDueAtUtc.Value.Kind != DateTimeKind.Utc)
        {
            buyerReturnDueAtUtc = DateTime.SpecifyKind(buyerReturnDueAtUtc.Value, DateTimeKind.Utc);
        }

        if (restockingFee is not null && !EnsureSameCurrency(restockingFee))
        {
            return Error.Failure("ReturnRequest.CurrencyMismatch", "Restocking fee currency does not match order currency.");
        }

        Status = ReturnStatus.AwaitingBuyerReturn;
        SellerRespondedAt = respondedAtUtc;
        BuyerReturnDueAt = buyerReturnDueAtUtc;
        SellerNote = sellerNote?.Trim() ?? SellerNote;
        RestockingFee = restockingFee;

        return Result.Success();
    }

    public Result Reject(DateTime respondedAtUtc, string? sellerNote)
    {
        if (Status != ReturnStatus.PendingSellerResponse)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return request cannot be rejected in the current status.");
        }

        if (respondedAtUtc.Kind != DateTimeKind.Utc)
        {
            respondedAtUtc = DateTime.SpecifyKind(respondedAtUtc, DateTimeKind.Utc);
        }

        Status = ReturnStatus.SellerDeclined;
        SellerRespondedAt = respondedAtUtc;
        SellerNote = sellerNote?.Trim() ?? SellerNote;
        ClosedAt = respondedAtUtc;

        return Result.Success();
    }

    public Result MarkBuyerShipped(string carrier, string trackingNumber, DateTime shippedAtUtc)
    {
        if (Status != ReturnStatus.AwaitingBuyerReturn)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return must be awaiting buyer shipment to record tracking.");
        }

        if (string.IsNullOrWhiteSpace(trackingNumber))
        {
            return Error.Failure("ReturnRequest.InvalidTracking", "Tracking number is required.");
        }

        if (shippedAtUtc.Kind != DateTimeKind.Utc)
        {
            shippedAtUtc = DateTime.SpecifyKind(shippedAtUtc, DateTimeKind.Utc);
        }

        ReturnCarrier = carrier?.Trim();
        TrackingNumber = trackingNumber.Trim();
        BuyerShippedAt = shippedAtUtc;
        Status = ReturnStatus.InTransitBackToSeller;

        return Result.Success();
    }

    public Result MarkDelivered(DateTime deliveredAtUtc)
    {
        if (Status != ReturnStatus.InTransitBackToSeller)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return must be in transit to mark as delivered.");
        }

        if (deliveredAtUtc.Kind != DateTimeKind.Utc)
        {
            deliveredAtUtc = DateTime.SpecifyKind(deliveredAtUtc, DateTimeKind.Utc);
        }

        DeliveredAt = deliveredAtUtc;
        Status = ReturnStatus.DeliveredToSeller;

        return Result.Success();
    }

    public Result StartRefundProcessing(string? sellerNote)
    {
        if (Status != ReturnStatus.DeliveredToSeller)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return must be delivered before starting refund processing.");
        }

        Status = ReturnStatus.RefundPending;
        SellerNote = sellerNote?.Trim() ?? SellerNote;

        return Result.Success();
    }

    public Result IssueRefund(Money refundAmount, DateTime refundIssuedAtUtc, string? sellerNote)
    {
        if (Status != ReturnStatus.RefundPending && Status != ReturnStatus.DeliveredToSeller)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Refund can only be issued when refund is pending or item is delivered.");
        }

        if (!EnsureSameCurrency(refundAmount))
        {
            return Error.Failure("ReturnRequest.CurrencyMismatch", "Refund amount currency does not match order currency.");
        }

        if (refundIssuedAtUtc.Kind != DateTimeKind.Utc)
        {
            refundIssuedAtUtc = DateTime.SpecifyKind(refundIssuedAtUtc, DateTimeKind.Utc);
        }

        RefundAmount = refundAmount;
        RefundIssuedAt = refundIssuedAtUtc;
        Status = ReturnStatus.RefundCompleted;
        SellerNote = sellerNote?.Trim() ?? SellerNote;
        ClosedAt = refundIssuedAtUtc;

        return Result.Success();
    }

    public Result MarkReplacementSent(DateTime shippedAtUtc, string? carrier, string? trackingNumber, string? sellerNote)
    {
        if (Status != ReturnStatus.AwaitingBuyerReturn && Status != ReturnStatus.InTransitBackToSeller && Status != ReturnStatus.DeliveredToSeller)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Replacement can only be sent for active return requests.");
        }

        if (shippedAtUtc.Kind != DateTimeKind.Utc)
        {
            shippedAtUtc = DateTime.SpecifyKind(shippedAtUtc, DateTimeKind.Utc);
        }

        Status = ReturnStatus.ReplacementSent;
        SellerNote = sellerNote?.Trim() ?? SellerNote;
        ReturnCarrier = carrier?.Trim() ?? ReturnCarrier;
        TrackingNumber = trackingNumber?.Trim() ?? TrackingNumber;

        return Result.Success();
    }

    public Result Close(DateTime closedAtUtc, string? sellerNote)
    {
        if (closedAtUtc.Kind != DateTimeKind.Utc)
        {
            closedAtUtc = DateTime.SpecifyKind(closedAtUtc, DateTimeKind.Utc);
        }

        Status = ReturnStatus.Closed;
        ClosedAt = closedAtUtc;
        SellerNote = sellerNote?.Trim() ?? SellerNote;

        return Result.Success();
    }

    private bool EnsureSameCurrency(Money money)
        => string.Equals(money.Currency, OrderTotalSnapshot.Currency, StringComparison.OrdinalIgnoreCase);
}
