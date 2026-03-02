using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Users.Events;

public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    string FullName,
    Email Email
) : DomainEventBase;
