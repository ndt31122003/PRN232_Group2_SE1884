using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Reviews.Events;

public sealed record ReviewReplyEditedDomainEvent(
    Guid ReviewId,
    Guid SellerId,
    Guid BuyerId,
    string NewReplyText,
    DateTimeOffset EditedAt
) : DomainEventBase;
