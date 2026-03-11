namespace PRN232_EbayClone.Infrastructure.Sms;

public sealed class TwilioConfiguration
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromNumber { get; set; } = string.Empty;
}
