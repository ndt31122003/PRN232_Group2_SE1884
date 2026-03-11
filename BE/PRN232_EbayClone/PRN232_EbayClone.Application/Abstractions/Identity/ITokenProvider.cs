using PRN232_EbayClone.Domain.Users.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}



