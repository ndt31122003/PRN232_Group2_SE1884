using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.SupportTickets.Events;

public sealed record SupportTicketCreatedDomainEvent(
    Guid TicketId,
    string TicketNumber,
    Guid SellerId,
    string Category,
    string Priority,
    DateTimeOffset CreatedAt
) : DomainEventBase;
