using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Events;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Identity.Entities;

public sealed class Otp(Guid id) : AggregateRoot<Guid>(id)
{
    private const int ExpirationMinutes = 5;
    public Email Email { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public DateTime ExpiresOnUtc { get; private set; }
    public bool IsUsed { get; private set; }
    public OtpType Type { get; private set; }

    public static Otp Create(Email email, string code, OtpType type)
    {
        var otp = new Otp(Guid.NewGuid())
        {
            Email = email,
            Code = code,
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(ExpirationMinutes),
            IsUsed = false,
            Type = type
        };

        otp.RaiseDomainEvent(new OtpGeneratedDomainEvent(email, code, ExpirationMinutes, type));

        return otp;
    }

    public void MarkAsUsed() => IsUsed = true;
    public bool IsValid() => !IsUsed && DateTime.UtcNow < ExpiresOnUtc;

    public void ResendOtp(string newCode, OtpType type)
    {
        Code = newCode;
        ExpiresOnUtc = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        IsUsed = false;

        RaiseDomainEvent(new OtpGeneratedDomainEvent(Email, newCode, ExpirationMinutes, type));
    }
}
