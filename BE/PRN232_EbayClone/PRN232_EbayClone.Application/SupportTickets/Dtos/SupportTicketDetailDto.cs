namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketDetailDto
{
    public Guid Id { get; init; }
    public string TicketNumber { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public Guid? AssignedToAdminId { get; init; }
    public string? AssignedToAdminName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? ResolvedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public List<SupportTicketAttachmentDto> Attachments { get; init; } = [];
    public List<SupportTicketResponseDto> Responses { get; init; } = [];
}
