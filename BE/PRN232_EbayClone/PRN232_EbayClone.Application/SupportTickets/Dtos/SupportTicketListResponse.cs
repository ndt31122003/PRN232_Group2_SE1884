namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketListResponse
{
    public List<SupportTicketSummaryDto> Tickets { get; init; } = [];
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int OpenCount { get; init; }
    public int PendingCount { get; init; }
}
