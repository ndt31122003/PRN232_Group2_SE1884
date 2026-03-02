namespace PRN232_EbayClone.Application.Abstractions.Authentication;

public interface IUserContext
{
    string? UserId { get; }
    string? Username { get; }
}
