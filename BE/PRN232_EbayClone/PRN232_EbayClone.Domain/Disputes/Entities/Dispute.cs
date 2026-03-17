using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public sealed class Dispute : AggregateRoot<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid ListingId { get; private set; }
    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }
    public DisputeType DisputeType { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DisputeStatus Status { get; private set; }
    public DisputeResolutionType? ResolutionType { get; private set; }
    public decimal? RefundAmount { get; private set; }
    public string? RefundCurrency { get; private set; }
    public DateTimeOffset OpenedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }
    public DateTimeOffset? EscalatedAt { get; private set; }
    public DateTimeOffset? DeadlineAt { get; private set; }
    public bool IsDeadlineApproaching { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public Order? Order { get; private set; }
    public Listing? Listing { get; private set; }

    private Dispute() : base(Guid.Empty)
    {
    }

    private Dispute(
        Guid id,
        Guid orderId,
        Guid listingId,
        Guid buyerId,
        Guid sellerId,
        DisputeType disputeType,
        string reason,
        DateTimeOffset openedAt,
        string? createdBy) : base(id)
    {
        OrderId = orderId;
        ListingId = listingId;
        BuyerId = buyerId;
        SellerId = sellerId;
        DisputeType = disputeType;
        Reason = reason;
        Status = DisputeStatus.Open;
        OpenedAt = openedAt;
        CreatedAt = openedAt;
        CreatedBy = createdBy;
        IsDeleted = false;
        IsDeadlineApproaching = false;
    }

    public static Result<Dispute> Create(
        Guid orderId,
        Guid listingId,
        Guid buyerId,
        Guid sellerId,
        DisputeType disputeType,
        string reason,
        DateTimeOffset openedAt,
        string? createdBy = null)
    {
        if (orderId == Guid.Empty)
        {
            return Error.Validation("Dispute.InvalidOrder", "Order ID is required");
        }

        if (listingId == Guid.Empty)
        {
            return Error.Validation("Dispute.InvalidListing", "Listing ID is required");
        }

        if (buyerId == Guid.Empty)
        {
            return Error.Validation("Dispute.InvalidBuyer", "Buyer ID is required");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Validation("Dispute.InvalidSeller", "Seller ID is required");
        }

        var trimmedReason = reason?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedReason))
        {
            return DisputeErrors.ReasonRequired;
        }

        var dispute = new Dispute(
            Guid.NewGuid(),
            orderId,
            listingId,
            buyerId,
            sellerId,
            disputeType,
            trimmedReason,
            openedAt,
            createdBy);

        return dispute;
    }

    public Result TransitionToWaitingSeller(DateTimeOffset deadline, string? updatedBy = null)
    {
        if (Status != DisputeStatus.Open)
        {
            return Error.Failure("Dispute.InvalidTransition", 
                $"Cannot transition from {Status} to WaitingSeller");
        }

        Status = DisputeStatus.WaitingSeller;
        DeadlineAt = deadline;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result AcceptRefund(
        decimal refundAmount,
        string currency,
        string? updatedBy = null)
    {
        if (Status != DisputeStatus.WaitingSeller)
        {
            return Error.Failure("Dispute.InvalidTransition", 
                $"Cannot accept refund from status {Status}");
        }

        if (refundAmount <= 0)
        {
            return Error.Validation("Dispute.InvalidRefundAmount", 
                "Refund amount must be positive");
        }

        Status = DisputeStatus.Resolved;
        ResolutionType = DisputeResolutionType.RefundIssued;
        RefundAmount = refundAmount;
        RefundCurrency = currency;
        ResolvedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result ProvideEvidence(string? updatedBy = null)
    {
        if (Status != DisputeStatus.WaitingSeller && Status != DisputeStatus.Open)
        {
            return Error.Failure("Dispute.InvalidTransition", 
                $"Cannot provide evidence from status {Status}");
        }

        Status = DisputeStatus.WaitingBuyer;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result Escalate(string? updatedBy = null)
    {
        if (Status == DisputeStatus.Closed || Status == DisputeStatus.Resolved)
        {
            return Error.Failure("Dispute.InvalidTransition", 
                $"Cannot escalate from status {Status}");
        }

        Status = DisputeStatus.Escalated;
        EscalatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result Resolve(DisputeResolutionType resolutionType, string? updatedBy = null)
    {
        if (Status == DisputeStatus.Closed)
        {
            return Error.Failure("Dispute.InvalidTransition", 
                "Cannot resolve a closed dispute");
        }

        Status = DisputeStatus.Resolved;
        ResolutionType = resolutionType;
        ResolvedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result Close(string? updatedBy = null)
    {
        if (Status == DisputeStatus.Closed)
        {
            return Error.Failure("Dispute.AlreadyClosed", "Dispute is already closed");
        }

        Status = DisputeStatus.Closed;
        ClosedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public void SetDeadlineApproaching(bool isApproaching)
    {
        IsDeadlineApproaching = isApproaching;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }

    public bool IsClosed => Status == DisputeStatus.Closed || Status == DisputeStatus.Resolved;
}
