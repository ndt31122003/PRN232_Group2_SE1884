using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Disputes.Events;

public sealed record DisputeEvidenceUploadedDomainEvent(
    Guid DisputeId,
    Guid UploadedById,
    int FileCount,
    DateTimeOffset UploadedAt
) : DomainEventBase;
