using PRN232_EbayClone.Domain.SupportTickets.Enums;

namespace PRN232_EbayClone.Domain.SupportTickets;

public sealed class SupportTicketStateMachine
{
    private static readonly Dictionary<SupportTicketStatus, List<SupportTicketStatus>> AllowedTransitions = new()
    {
        [SupportTicketStatus.Open] = new() { SupportTicketStatus.Pending, SupportTicketStatus.InProgress, SupportTicketStatus.Closed },
        [SupportTicketStatus.Pending] = new() { SupportTicketStatus.InProgress, SupportTicketStatus.Closed },
        [SupportTicketStatus.InProgress] = new() { SupportTicketStatus.WaitingSeller, SupportTicketStatus.Resolved, SupportTicketStatus.Closed },
        [SupportTicketStatus.WaitingSeller] = new() { SupportTicketStatus.InProgress, SupportTicketStatus.Closed },
        [SupportTicketStatus.Resolved] = new() { SupportTicketStatus.InProgress, SupportTicketStatus.Closed },
        [SupportTicketStatus.Closed] = new() { }
    };

    public static bool IsTransitionAllowed(SupportTicketStatus fromStatus, SupportTicketStatus toStatus)
    {
        return AllowedTransitions.ContainsKey(fromStatus) 
            && AllowedTransitions[fromStatus].Contains(toStatus);
    }

    public static List<SupportTicketStatus> GetAllowedTransitions(SupportTicketStatus currentStatus)
    {
        return AllowedTransitions.ContainsKey(currentStatus) 
            ? AllowedTransitions[currentStatus] 
            : new List<SupportTicketStatus>();
    }
}
