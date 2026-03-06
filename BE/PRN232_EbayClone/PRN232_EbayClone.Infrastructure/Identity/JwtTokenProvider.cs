using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Shared.Constants;
using PRN232_EbayClone.Domain.Users.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PRN232_EbayClone.Infrastructure.Identity;

public sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtConfiguration _jwtConfiguration;
    public JwtTokenProvider(IOptions<JwtConfiguration> jwtConfiguration)
        => _jwtConfiguration = jwtConfiguration.Value;

    public string GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new("is_email_verified", user.IsEmailVerified.ToString().ToLowerInvariant()),
            new("is_phone_verified", user.IsPhoneVerified.ToString().ToLowerInvariant()),
            new("is_business_verified", user.IsBusinessVerified.ToString().ToLowerInvariant())
        };

        if (user.Roles is not null && user.Roles.Any())
        {
            var distinctPermissions = user.Roles
                .Where(r => r.Permissions is not null && r.Permissions.Any())
                .SelectMany(role => role.Permissions)
                .Select(p => p.Permission.ToString())
                .Distinct();

            claims.AddRange(distinctPermissions.Select(p => new Claim(CustomClaimTypes.Permission, p)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpiryMinutes),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
