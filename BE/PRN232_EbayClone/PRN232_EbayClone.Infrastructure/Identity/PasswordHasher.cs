using PRN232_EbayClone.Application.Abstractions.Authentication;

namespace PRN232_EbayClone.Infrastructure.Identity;

public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
}
