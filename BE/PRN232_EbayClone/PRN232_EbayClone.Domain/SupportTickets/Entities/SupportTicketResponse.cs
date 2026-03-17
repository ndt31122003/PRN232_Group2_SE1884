using PRN232_EbayClone.Domain.SupportTickets.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SupportTickets.Entities;

public sealed class SupportTicketResponse : Entity<Guid>
{
    public const int MaxMessageLength = 5000;

    public Guid TicketId { get; private set; }
    public Guid ResponderId { get; private set; }
    public ResponderRole ResponderRole { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsInternalNote { get; private set; }
    public DateTimeOffset RespondedAt { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public SupportTicket? Ticket { get; private set; }

    private SupportTicketResponse() : base(Guid.Empty)
    {
    }

    private SupportTicketResponse(
        Guid id,
        Guid ticketId,
        Guid responderId,
        ResponderRole responderRole,
        string message,
        bool isInternalNote,
        DateTimeOffset respondedAt,
        string? createdBy) : base(id)
    {
        TicketId = ticketId;
        ResponderId = responderId;
        ResponderRole = responderRole;
        Message = message;
        IsInternalNote = isInternalNote;
        RespondedAt = respondedAt;
        CreatedAt = respondedAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<SupportTicketResponse> Create(
        Guid ticketId,
        Guid responderId,
        ResponderRole responderRole,
        string message,
        bool isInternalNote,
        DateTimeOffset respondedAt,
        string? createdBy = null)
    {
        if (ticketId == Guid.Empty)
        {
            return Error.Validation("SupportTicketResponse.InvalidTicket", "Ticket ID is required");
        }

        if (responderId == Guid.Empty)
        {
            return Error.Validation("SupportTicketResponse.InvalidResponder", "Responder ID is required");
        }

        var trimmedMessage = message?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedMessage))
        {
            return Error.Validation("SupportTicketResponse.MessageRequired", "Message is required");
        }

        if (trimmedMessage.Length > MaxMessageLength)
        {
            return Error.Validation(
                "SupportTicketResponse.MessageTooLong",
                $"Message cannot exceed {MaxMessageLength} characters");
        }

        var response = new SupportTicketResponse(
            Guid.NewGuid(),
            ticketId,
            responderId,
            responderRole,
            trimmedMessage,
            isInternalNote,
            respondedAt,
            createdBy);

        return response;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }
}
