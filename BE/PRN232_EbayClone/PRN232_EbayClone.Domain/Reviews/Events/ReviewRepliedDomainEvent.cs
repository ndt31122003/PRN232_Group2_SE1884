using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Reviews.Events;

public sealed record ReviewRepliedDomainEvent(
    Guid ReviewId,
    Guid SellerId,
    Guid BuyerId,
    string ReplyText,
    DateTimeOffset RepliedAt
) : DomainEventBase;
