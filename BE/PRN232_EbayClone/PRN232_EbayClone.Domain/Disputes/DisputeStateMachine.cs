using PRN232_EbayClone.Domain.Disputes.Enums;

namespace PRN232_EbayClone.Domain.Disputes;

public sealed class DisputeStateMachine
{
    private static readonly Dictionary<DisputeStatus, List<DisputeStatus>> AllowedTransitions = new()
    {
        [DisputeStatus.Open] = new() { DisputeStatus.WaitingSeller, DisputeStatus.Escalated, DisputeStatus.Closed },
        [DisputeStatus.WaitingSeller] = new() { DisputeStatus.Resolved, DisputeStatus.WaitingBuyer, DisputeStatus.Escalated, DisputeStatus.Closed },
        [DisputeStatus.WaitingBuyer] = new() { DisputeStatus.Resolved, DisputeStatus.WaitingSeller, DisputeStatus.Escalated, DisputeStatus.Closed },
        [DisputeStatus.Escalated] = new() { DisputeStatus.Resolved, DisputeStatus.Closed },
        [DisputeStatus.Resolved] = new() { DisputeStatus.Closed },
        [DisputeStatus.Closed] = new() { }
    };

    public static bool IsTransitionAllowed(DisputeStatus fromStatus, DisputeStatus toStatus)
    {
        return AllowedTransitions.ContainsKey(fromStatus) 
            && AllowedTransitions[fromStatus].Contains(toStatus);
    }

    public static List<DisputeStatus> GetAllowedTransitions(DisputeStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus) 
            ? AllowedTransitions[currentStatus] 
            : new List<DisputeStatus>();
    }
}
