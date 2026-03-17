namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketResponseDto
{
    public Guid Id { get; init; }
    public Guid ResponderId { get; init; }
    public string ResponderName { get; init; } = string.Empty;
    public string ResponderRole { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime RespondedAt { get; init; }
}
