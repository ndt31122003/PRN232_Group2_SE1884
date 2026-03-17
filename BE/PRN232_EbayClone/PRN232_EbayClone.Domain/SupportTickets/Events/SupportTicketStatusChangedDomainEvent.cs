using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.SupportTickets.Events;

public sealed record SupportTicketStatusChangedDomainEvent(
    Guid TicketId,
    string FromStatus,
    string ToStatus,
    Guid ChangedById,
    DateTimeOffset ChangedAt
) : DomainEventBase;
