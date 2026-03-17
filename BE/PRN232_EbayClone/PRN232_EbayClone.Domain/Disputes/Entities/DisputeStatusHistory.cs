using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public sealed class DisputeStatusHistory : Entity<Guid>
{
    public Guid DisputeId { get; private set; }
    public DisputeStatus FromStatus { get; private set; }
    public DisputeStatus ToStatus { get; private set; }
    public Guid ChangedById { get; private set; }
    public SenderRole ChangedByRole { get; private set; }
    public string? Reason { get; private set; }
    public DateTimeOffset ChangedAt { get; private set; }

    // Navigation properties
    public Dispute? Dispute { get; private set; }

    private DisputeStatusHistory() : base(Guid.Empty)
    {
    }

    private DisputeStatusHistory(
        Guid id,
        Guid disputeId,
        DisputeStatus fromStatus,
        DisputeStatus toStatus,
        Guid changedById,
        SenderRole changedByRole,
        string? reason,
        DateTimeOffset changedAt) : base(id)
    {
        DisputeId = disputeId;
        FromStatus = fromStatus;
        ToStatus = toStatus;
        ChangedById = changedById;
        ChangedByRole = changedByRole;
        Reason = reason;
        ChangedAt = changedAt;
    }

    public static Result<DisputeStatusHistory> Create(
        Guid disputeId,
        DisputeStatus fromStatus,
        DisputeStatus toStatus,
        Guid changedById,
        SenderRole changedByRole,
        string? reason,
        DateTimeOffset changedAt)
    {
        if (disputeId == Guid.Empty)
        {
            return Error.Validation("DisputeStatusHistory.InvalidDispute", "Dispute ID is required");
        }

        if (changedById == Guid.Empty)
        {
            return Error.Validation("DisputeStatusHistory.InvalidChanger", "Changed by ID is required");
        }

        var history = new DisputeStatusHistory(
            Guid.NewGuid(),
            disputeId,
            fromStatus,
            toStatus,
            changedById,
            changedByRole,
            reason?.Trim(),
            changedAt);

        return history;
    }
}
