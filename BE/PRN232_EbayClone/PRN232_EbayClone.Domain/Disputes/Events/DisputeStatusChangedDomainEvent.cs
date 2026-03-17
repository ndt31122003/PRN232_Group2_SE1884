using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Disputes.Events;

public sealed record DisputeStatusChangedDomainEvent(
    Guid DisputeId,
    string FromStatus,
    string ToStatus,
    Guid ChangedById,
    string ChangedByRole,
    DateTimeOffset ChangedAt
) : DomainEventBase;
