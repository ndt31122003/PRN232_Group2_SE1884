using Microsoft.Extensions.Configuration;
using PRN232_EbayClone.Application.Abstractions.Security;
using System.Net.Http;
using System.Text.Json;

namespace PRN232_EbayClone.Infrastructure.Services
{
    public sealed class RecaptchaService : IRecaptchaVerificationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private const string BaseUrl = "https://www.google.com/recaptcha/api/siteverify";

        public RecaptchaService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _secretKey = config["ReCaptcha:SecretKey"] ?? config["Recaptcha:SecretKey"] ?? string.Empty;
        }

        public async Task<RecaptchaVerificationResult> VerifyAsync(string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(_secretKey))
            {
                return new RecaptchaVerificationResult(false, null);
            }

            var payload = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("response", token)
            ]);

            var response = await _httpClient.PostAsync(BaseUrl, payload, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new RecaptchaVerificationResult(false, null);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            var result = JsonSerializer.Deserialize<RecaptchaResponse>(content);

            return new RecaptchaVerificationResult(result?.Success == true, result?.Action);
        }

        private sealed record RecaptchaResponse(
            bool Success,
            string? Action
        );
    }
}
