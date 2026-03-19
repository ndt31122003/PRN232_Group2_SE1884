namespace PRN232_EbayClone.Api.Services;

public interface ICurrentUser
{
    Guid Id { get; }
    string? UserId { get; }
    string? Username { get; }
}