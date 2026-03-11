using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Sms;

namespace PRN232_EbayClone.Infrastructure.Sms;

public sealed class DevSmsSender : ISmsSender
{
    private readonly ILogger<DevSmsSender> _logger;

    public DevSmsSender(ILogger<DevSmsSender> logger)
    {
        _logger = logger;
    }

    public Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning(
            "[DEV SMS] To: {PhoneNumber} | Message: {Message}",
            phoneNumber,
            message);

        return Task.CompletedTask;
    }
}
