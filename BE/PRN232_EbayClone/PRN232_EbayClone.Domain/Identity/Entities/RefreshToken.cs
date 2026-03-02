using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Identity.Entities;

public sealed class RefreshToken : AggregateRoot<Guid>
{
    private const int ExpiresInDays = 7;
    private RefreshToken(Guid id) : base(id)
    {
    }

    public UserId UserId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresOnUtc { get; private set; }
    public User? User { get; private set; }

    public bool IsExpired() => DateTime.UtcNow >= ExpiresOnUtc;

    public static RefreshToken Create(UserId userId, string token)
    {
        return new RefreshToken(Guid.NewGuid())
        {
            UserId = userId,
            Token = token,
            ExpiresOnUtc = DateTime.UtcNow.AddDays(ExpiresInDays)
        };
    }

}