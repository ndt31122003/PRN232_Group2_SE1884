using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PRN232_EbayClone.Application.Abstractions.Sms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PRN232_EbayClone.Infrastructure.Sms;

public sealed class TwilioSmsSender : ISmsSender
{
    private readonly TwilioConfiguration _config;
    private readonly ILogger<TwilioSmsSender> _logger;

    public TwilioSmsSender(IOptions<TwilioConfiguration> config, ILogger<TwilioSmsSender> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            TwilioClient.Init(_config.AccountSid, _config.AuthToken);

            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_config.FromNumber),
                body: message);

            _logger.LogInformation("SMS sent to {PhoneNumber}, SID: {Sid}", phoneNumber, messageResource.Sid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}
