using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Disputes.Events;

public sealed record DisputeEscalatedDomainEvent(
    Guid DisputeId,
    Guid SellerId,
    Guid BuyerId,
    string Reason,
    DateTimeOffset EscalatedAt
) : DomainEventBase;
