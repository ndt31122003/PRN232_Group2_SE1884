using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.SupportTickets.Events;

public sealed record SupportTicketResponseAddedDomainEvent(
    Guid TicketId,
    Guid ResponseId,
    Guid ResponderId,
    string ResponderRole,
    DateTimeOffset RespondedAt
) : DomainEventBase;
