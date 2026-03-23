using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using PRN232_EbayClone.Application.Abstractions.Storage;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

internal sealed class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var cloudName = (Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? configuration["Cloudinary:CloudName"])?.Trim();
        var apiKey = (Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? configuration["Cloudinary:ApiKey"])?.Trim();
        var apiSecret = (Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? configuration["Cloudinary:ApiSecret"])?.Trim();

        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
        {
            throw new InvalidOperationException($"Cloudinary credentials missing. CloudName: '{cloudName}', ApiKey exists: {!string.IsNullOrEmpty(apiKey)}, ApiSecret exists: {!string.IsNullOrEmpty(apiSecret)}");
        }

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<Result<string>> UploadImageAsync(string base64Image, CancellationToken cancellationToken = default)
    {
        try
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(base64Image),
                Folder = "prn232_ebayclone_listings"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (uploadResult.Error != null)
            {
                return PRN232_EbayClone.Domain.Shared.Results.Error.Failure("Cloudinary.UploadFailed", uploadResult.Error.Message);
            }

            return uploadResult.SecureUrl.ToString();
        }
        catch (Exception ex)
        {
            return PRN232_EbayClone.Domain.Shared.Results.Error.Failure("Cloudinary.Exception", ex.Message);
        }
    }
}
