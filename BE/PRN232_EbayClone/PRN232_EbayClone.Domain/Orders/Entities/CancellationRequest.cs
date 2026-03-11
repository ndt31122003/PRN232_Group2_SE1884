using System;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Orders.Events;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Orders.Entities;

public class CancellationRequest : AggregateRoot<Guid>
{
    private CancellationRequest(Guid id) : base(id) { }

    public Guid OrderId { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public CancellationInitiator InitiatedBy { get; private set; }
    public CancellationReason Reason { get; private set; }
    public string? BuyerNote { get; private set; }
    public string? SellerNote { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? SellerResponseDeadlineUtc { get; private set; }
    public DateTime? SellerRespondedAt { get; private set; }
    public DateTime? AutoClosedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public CancellationStatus Status { get; private set; }
    public Money? RefundAmount { get; private set; }
    public Money OrderTotalSnapshot { get; private set; } = null!;
    public Order Order { get; private set; } = null!;

    public bool IsClosed => Status is CancellationStatus.Completed or CancellationStatus.Declined or CancellationStatus.AutoCancelled;
    public bool RequiresSellerAction => Status is CancellationStatus.PendingSellerResponse or CancellationStatus.AwaitingRefund;

    public static Result<CancellationRequest> Create(
        Guid orderId,
        Guid buyerId,
        Guid sellerId,
        CancellationInitiator initiatedBy,
        CancellationReason reason,
        Money orderTotalSnapshot,
        string? buyerNote,
        string? sellerNote,
        DateTime requestedAtUtc,
        DateTime? sellerResponseDeadlineUtc)
    {
        if (orderId == Guid.Empty)
        {
            return Error.Failure("CancellationRequest.InvalidOrderId", "OrderId cannot be empty.");
        }

        if (buyerId == Guid.Empty)
        {
            return Error.Failure("CancellationRequest.InvalidBuyerId", "BuyerId cannot be empty.");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Failure("CancellationRequest.InvalidSellerId", "SellerId cannot be empty.");
        }

        if (requestedAtUtc.Kind != DateTimeKind.Utc)
        {
            requestedAtUtc = DateTime.SpecifyKind(requestedAtUtc, DateTimeKind.Utc);
        }

        if (sellerResponseDeadlineUtc.HasValue && sellerResponseDeadlineUtc.Value.Kind != DateTimeKind.Utc)
        {
            sellerResponseDeadlineUtc = DateTime.SpecifyKind(sellerResponseDeadlineUtc.Value, DateTimeKind.Utc);
        }

        var status = initiatedBy switch
        {
            CancellationInitiator.Buyer => CancellationStatus.PendingSellerResponse,
            CancellationInitiator.Seller => CancellationStatus.PendingBuyerConfirmation,
            CancellationInitiator.System => CancellationStatus.AutoCancelled,
            _ => CancellationStatus.PendingSellerResponse
        };

        if (initiatedBy == CancellationInitiator.System)
        {
            sellerNote = sellerNote ?? "System initiated cancellation";
        }

        var request = new CancellationRequest(Guid.NewGuid())
        {
            OrderId = orderId,
            BuyerId = buyerId,
            SellerId = sellerId,
            InitiatedBy = initiatedBy,
            Reason = reason,
            BuyerNote = buyerNote,
            SellerNote = sellerNote,
            RequestedAt = requestedAtUtc,
            SellerResponseDeadlineUtc = sellerResponseDeadlineUtc,
            Status = status,
            OrderTotalSnapshot = orderTotalSnapshot
        };

        if (status is CancellationStatus.AutoCancelled)
        {
            request.AutoClosedAt = requestedAtUtc;
            request.CompletedAt = requestedAtUtc;
        }

        return Result.Success(request);
    }

    public Result UpdateSellerNote(string? note)
    {
        SellerNote = note?.Trim();
        return Result.Success();
    }

    public Result UpdateBuyerNote(string? note)
    {
        BuyerNote = note?.Trim();
        return Result.Success();
    }

    public Result Approve(Money refundAmount, DateTime respondedAtUtc, string? sellerNote)
    {
        if (Order.Status.Code != OrderStatusCodes.DeliveryFailed
            && Order.Status.Code != OrderStatusCodes.PaidAndShipped
            && Order.Status.Code != OrderStatusCodes.ShippedAwaitingFeedback)
        {
            if (Status != CancellationStatus.PendingBuyerConfirmation && Status != CancellationStatus.PendingSellerResponse)
            {
                return Error.Failure("CancellationRequest.InvalidStatus", "Cancellation cannot be approved in the current status.");
            }
            if (respondedAtUtc.Kind != DateTimeKind.Utc)
            {
                respondedAtUtc = DateTime.SpecifyKind(respondedAtUtc, DateTimeKind.Utc);
            }
            if (Order.Status.Code != OrderStatusCodes.AwaitingPayment)
                RefundAmount = refundAmount;
            else RefundAmount = Money.Zero("USD").Value;
            SellerRespondedAt = respondedAtUtc;
            Status = CancellationStatus.AwaitingRefund;
            SellerNote = sellerNote?.Trim();
            RaiseDomainEvent(new CancellationApprovedEvent(OrderId));
            return Result.Success();
        }
        return Error.Failure("CancellationRequest.InvalidOrderStatus", "Cancellation cannot be approved in the current order's status.");
    }

    public Result Reject(DateTime respondedAtUtc, string? sellerNote)
    {
        if (Order.Status.Code == OrderStatusCodes.DeliveryFailed
            || Order.Status.Code == OrderStatusCodes.PaidAndShipped
            || Order.Status.Code == OrderStatusCodes.ShippedAwaitingFeedback)
            return Error.Failure("CancellationRequest.InvalidOrderStatus", "Cancellation cannot be rejected in the current order's status.");

        if (Status != CancellationStatus.PendingSellerResponse && Status != CancellationStatus.PendingBuyerConfirmation)
        {
            return Error.Failure("CancellationRequest.InvalidStatus", "Cancellation cannot be rejected in the current status.");
        }

        if (respondedAtUtc.Kind != DateTimeKind.Utc)
        {
            respondedAtUtc = DateTime.SpecifyKind(respondedAtUtc, DateTimeKind.Utc);
        }

        Status = CancellationStatus.Declined;
        SellerRespondedAt = respondedAtUtc;
        SellerNote = sellerNote?.Trim() ?? SellerNote;
        CompletedAt = respondedAtUtc;

        return Result.Success();
    }

    public Result RecordRefund(Money refundAmount, DateTime refundCompletedUtc, string? sellerNote)
    {
        if (Status != CancellationStatus.AwaitingRefund)
        {
            return Error.Failure("CancellationRequest.InvalidStatus", "Refund can only be recorded when awaiting refund.");
        }

        if (refundCompletedUtc.Kind != DateTimeKind.Utc)
        {
            refundCompletedUtc = DateTime.SpecifyKind(refundCompletedUtc, DateTimeKind.Utc);
        }

        if (!EnsureSameCurrency(refundAmount))
        {
            return Error.Failure("CancellationRequest.CurrencyMismatch", "Refund amount currency does not match the order currency snapshot.");
        }

        RefundAmount = refundAmount;
        Status = CancellationStatus.Completed;
        CompletedAt = refundCompletedUtc;
        SellerNote = sellerNote?.Trim() ?? SellerNote;

        return Result.Success();
    }

    public Result MarkAutoCancelled(DateTime autoCancelledUtc, string? systemNote = null)
    {
        if (autoCancelledUtc.Kind != DateTimeKind.Utc)
        {
            autoCancelledUtc = DateTime.SpecifyKind(autoCancelledUtc, DateTimeKind.Utc);
        }

        Status = CancellationStatus.AutoCancelled;
        AutoClosedAt = autoCancelledUtc;
        CompletedAt = autoCancelledUtc;
        SellerNote = systemNote?.Trim() ?? SellerNote;

        return Result.Success();
    }

    private bool EnsureSameCurrency(Money money)
        => string.Equals(money.Currency, OrderTotalSnapshot.Currency, StringComparison.OrdinalIgnoreCase);
}
