using Microsoft.AspNetCore.Http;

namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record CreateSupportTicketRequest
{
    public string Category { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Priority { get; init; }
    public IFormFileCollection? Attachments { get; init; }
}
