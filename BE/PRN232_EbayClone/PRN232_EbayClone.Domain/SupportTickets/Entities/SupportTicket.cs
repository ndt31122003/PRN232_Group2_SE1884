using PRN232_EbayClone.Domain.SupportTickets.Enums;
using PRN232_EbayClone.Domain.SupportTickets.Events;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SupportTickets.Entities;

public sealed class SupportTicket : AggregateRoot<Guid>
{
    public const int MaxSubjectLength = 200;
    public const int MaxMessageLength = 5000;

    public string TicketNumber { get; private set; } = string.Empty;
    public Guid SellerId { get; private set; }
    public SupportTicketCategory Category { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public SupportTicketStatus Status { get; private set; }
    public SupportTicketPriority Priority { get; private set; }
    public Guid? AssignedToAdminId { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    private SupportTicket() : base(Guid.Empty)
    {
    }

    private SupportTicket(
        Guid id,
        string ticketNumber,
        Guid sellerId,
        SupportTicketCategory category,
        string subject,
        string message,
        SupportTicketPriority priority,
        DateTimeOffset createdAt,
        string? createdBy) : base(id)
    {
        TicketNumber = ticketNumber;
        SellerId = sellerId;
        Category = category;
        Subject = subject;
        Message = message;
        Status = SupportTicketStatus.Open;
        Priority = priority;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<SupportTicket> Create(
        string ticketNumber,
        Guid sellerId,
        SupportTicketCategory category,
        string subject,
        string message,
        SupportTicketPriority priority,
        DateTimeOffset createdAt,
        string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(ticketNumber))
        {
            return Error.Validation("SupportTicket.TicketNumberRequired", "Ticket number is required");
        }

        if (sellerId == Guid.Empty)
        {
            return Error.Validation("SupportTicket.InvalidSeller", "Seller ID is required");
        }

        var trimmedSubject = subject?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedSubject))
        {
            return Error.Validation("SupportTicket.SubjectRequired", "Subject is required");
        }

        if (trimmedSubject.Length > MaxSubjectLength)
        {
            return Error.Validation(
                "SupportTicket.SubjectTooLong",
                $"Subject cannot exceed {MaxSubjectLength} characters");
        }

        var trimmedMessage = message?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmedMessage))
        {
            return Error.Validation("SupportTicket.MessageRequired", "Message is required");
        }

        if (trimmedMessage.Length > MaxMessageLength)
        {
            return Error.Validation(
                "SupportTicket.MessageTooLong",
                $"Message cannot exceed {MaxMessageLength} characters");
        }

        var ticket = new SupportTicket(
            Guid.NewGuid(),
            ticketNumber,
            sellerId,
            category,
            trimmedSubject,
            trimmedMessage,
            priority,
            createdAt,
            createdBy);

        // Raise domain event for ticket creation
        ticket.RaiseDomainEvent(new SupportTicketCreatedDomainEvent(
            ticket.Id,
            ticket.TicketNumber,
            ticket.SellerId,
            ticket.Category.ToString(),
            ticket.Priority.ToString(),
            ticket.CreatedAt));

        return ticket;
    }

    public Result UpdateStatus(SupportTicketStatus newStatus, string? updatedBy = null)
    {
        if (Status == SupportTicketStatus.Closed)
        {
            return Error.Failure("SupportTicket.Closed", "Cannot update a closed ticket");
        }

        var oldStatus = Status;
        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        if (newStatus == SupportTicketStatus.Resolved)
        {
            ResolvedAt = DateTimeOffset.UtcNow;
        }
        else if (newStatus == SupportTicketStatus.Closed)
        {
            ClosedAt = DateTimeOffset.UtcNow;
        }

        // Raise domain event for status change
        RaiseDomainEvent(new SupportTicketStatusChangedDomainEvent(
            Id,
            oldStatus.ToString(),
            newStatus.ToString(),
            Guid.Empty, // Will be set by the command handler with actual user ID
            DateTimeOffset.UtcNow));

        return Result.Success();
    }

    public Result AssignToAdmin(Guid adminId, string? updatedBy = null)
    {
        if (Status == SupportTicketStatus.Closed)
        {
            return Error.Failure("SupportTicket.Closed", "Cannot assign a closed ticket");
        }

        if (adminId == Guid.Empty)
        {
            return Error.Validation("SupportTicket.InvalidAdmin", "Admin ID is required");
        }

        AssignedToAdminId = adminId;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result Resolve(string? updatedBy = null)
    {
        if (Status == SupportTicketStatus.Closed)
        {
            return Error.Failure("SupportTicket.Closed", "Cannot resolve a closed ticket");
        }

        Status = SupportTicketStatus.Resolved;
        ResolvedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public Result Close(string? updatedBy = null)
    {
        if (Status == SupportTicketStatus.Closed)
        {
            return Error.Failure("SupportTicket.AlreadyClosed", "Ticket is already closed");
        }

        Status = SupportTicketStatus.Closed;
        ClosedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = updatedBy;

        return Result.Success();
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }

    public void RaiseResponseAddedEvent(Guid responseId, Guid responderId, string responderRole, DateTimeOffset respondedAt)
    {
        RaiseDomainEvent(new SupportTicketResponseAddedDomainEvent(
            Id,
            responseId,
            responderId,
            responderRole,
            respondedAt));
    }

    public bool IsClosed => Status == SupportTicketStatus.Closed;
}
