namespace PRN232_EbayClone.Infrastructure.Identity;

public sealed class JwtConfiguration
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpiryMinutes { get; set; }
}
