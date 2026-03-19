using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Services;

public interface IDisputeStateMachine
{
    Result<DisputeStatus> AcceptRefund(DisputeStatus currentStatus);
    Result<DisputeStatus> ProvideEvidence(DisputeStatus currentStatus);
    Result<DisputeStatus> Escalate(DisputeStatus currentStatus);
    DateTime? GetDeadline(DisputeStatus status, DateTime lastUpdated);
    bool IsDeadlineSoon(DisputeStatus status, DateTime lastUpdated, TimeSpan threshold);
}

public class DisputeStateMachine : IDisputeStateMachine
{
    private static readonly TimeSpan SellerSla = TimeSpan.FromHours(48);
    private static readonly TimeSpan BuyerSla = TimeSpan.FromHours(72);

    public Result<DisputeStatus> AcceptRefund(DisputeStatus currentStatus)
    {
        return currentStatus switch
        {
            DisputeStatus.Open => Result.Success(DisputeStatus.Resolved),
            DisputeStatus.WaitingSeller => Result.Success(DisputeStatus.Resolved),
            DisputeStatus.WaitingBuyer => Result.Success(DisputeStatus.Resolved),
            DisputeStatus.Escalated => Result.Success(DisputeStatus.Resolved),
            DisputeStatus.Resolved => Error.Validation("Dispute.AlreadyResolved", "Dispute đã được giải quyết"),
            DisputeStatus.Closed => Error.Validation("Dispute.AlreadyClosed", "Dispute đã được đóng"),
            _ => Error.Validation("Dispute.InvalidTransition", $"Không thể chấp nhận hoàn tiền từ trạng thái {currentStatus}")
        };
    }

    public Result<DisputeStatus> ProvideEvidence(DisputeStatus currentStatus)
    {
        return currentStatus switch
        {
            DisputeStatus.Open => Result.Success(DisputeStatus.WaitingBuyer),
            DisputeStatus.WaitingSeller => Result.Success(DisputeStatus.WaitingBuyer),
            DisputeStatus.WaitingBuyer => Result.Success(DisputeStatus.WaitingBuyer), // Can update evidence
            DisputeStatus.Escalated => Error.Validation("Dispute.CannotProvideEvidence", "Không thể cung cấp bằng chứng khi dispute đã được escalate"),
            DisputeStatus.Resolved => Error.Validation("Dispute.AlreadyResolved", "Dispute đã được giải quyết"),
            DisputeStatus.Closed => Error.Validation("Dispute.AlreadyClosed", "Dispute đã được đóng"),
            _ => Error.Validation("Dispute.InvalidTransition", $"Không thể cung cấp bằng chứng từ trạng thái {currentStatus}")
        };
    }

    public Result<DisputeStatus> Escalate(DisputeStatus currentStatus)
    {
        return currentStatus switch
        {
            DisputeStatus.Open => Result.Success(DisputeStatus.Escalated),
            DisputeStatus.WaitingSeller => Result.Success(DisputeStatus.Escalated),
            DisputeStatus.WaitingBuyer => Result.Success(DisputeStatus.Escalated),
            DisputeStatus.Escalated => Error.Validation("Dispute.AlreadyEscalated", "Dispute đã được escalate"),
            DisputeStatus.Resolved => Error.Validation("Dispute.AlreadyResolved", "Dispute đã được giải quyết"),
            DisputeStatus.Closed => Error.Validation("Dispute.AlreadyClosed", "Dispute đã được đóng"),
            _ => Error.Validation("Dispute.InvalidTransition", $"Không thể escalate từ trạng thái {currentStatus}")
        };
    }

    public DateTime? GetDeadline(DisputeStatus status, DateTime lastUpdated)
    {
        return status switch
        {
            DisputeStatus.WaitingSeller => lastUpdated.Add(SellerSla),
            DisputeStatus.WaitingBuyer => lastUpdated.Add(BuyerSla),
            _ => null
        };
    }

    public bool IsDeadlineSoon(DisputeStatus status, DateTime lastUpdated, TimeSpan threshold)
    {
        var deadline = GetDeadline(status, lastUpdated);
        if (deadline == null) return false;
        
        return DateTime.UtcNow.Add(threshold) >= deadline.Value;
    }
}