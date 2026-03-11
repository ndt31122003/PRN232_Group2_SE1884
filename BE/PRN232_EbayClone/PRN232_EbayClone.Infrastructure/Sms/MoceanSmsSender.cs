using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PRN232_EbayClone.Application.Abstractions.Sms;

namespace PRN232_EbayClone.Infrastructure.Sms;

public sealed class MoceanSmsSender : ISmsSender
{
    private static readonly HttpClient HttpClient = new();
    private const string BaseUrl = "https://rest.moceanapi.com/rest/2/sms";

    private readonly MoceanConfiguration _config;
    private readonly ILogger<MoceanSmsSender> _logger;

    public MoceanSmsSender(IOptions<MoceanConfiguration> config, ILogger<MoceanSmsSender> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _config.ApiToken);

            var payload = new Dictionary<string, string>
            {
                ["mocean-to"] = NormalizePhoneNumber(phoneNumber),
                ["mocean-from"] = _config.From,
                ["mocean-text"] = message
            };

            request.Content = new FormUrlEncodedContent(payload);

            var response = await HttpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "MOCEAN SMS sent to {PhoneNumber}, Response: {Response}",
                    phoneNumber, responseBody);
            }
            else
            {
                _logger.LogError(
                    "MOCEAN SMS failed for {PhoneNumber}, Status: {StatusCode}, Response: {Response}",
                    phoneNumber, response.StatusCode, responseBody);
                throw new HttpRequestException($"MOCEAN API returned {response.StatusCode}: {responseBody}");
            }
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS via MOCEAN to {PhoneNumber}", phoneNumber);
            throw;
        }
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        return phoneNumber.TrimStart('+');
    }
}
