using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Identity.Events;

public sealed record OtpGeneratedDomainEvent(
    string Email,
    string Code,
    int ExpiresInMinutes,
    OtpType Type
) : DomainEventBase;
