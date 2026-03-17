using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Disputes.Events;

public sealed record DisputeCreatedDomainEvent(
    Guid DisputeId,
    Guid OrderId,
    Guid BuyerId,
    Guid SellerId,
    string DisputeType,
    DateTimeOffset OpenedAt
) : DomainEventBase;
