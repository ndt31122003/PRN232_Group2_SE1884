namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record CreateSupportTicketResponse
{
    public Guid Id { get; init; }
    public string TicketNumber { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
