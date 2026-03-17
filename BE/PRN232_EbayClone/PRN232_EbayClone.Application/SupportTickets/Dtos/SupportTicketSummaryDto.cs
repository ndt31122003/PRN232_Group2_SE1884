namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketSummaryDto
{
    public Guid Id { get; init; }
    public string TicketNumber { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public int ResponseCount { get; init; }
    public bool HasUnreadResponses { get; init; }
}
