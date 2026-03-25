using System;
using System.IO;
using PRN232_EbayClone.Application.Abstractions.File;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PRN232_EbayClone.Domain.FileMetadata.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.FileMetadata.Entities;
using Error = PRN232_EbayClone.Domain.Shared.Results.Error;
using System.Net;
using Microsoft.Extensions.Logging;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

public sealed class CloudinaryFileManager : IFileManager
{
    private readonly CloudinaryConfiguration _config;
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryFileManager> _logger;

    public CloudinaryFileManager(IOptions<CloudinaryConfiguration> config, ILogger<CloudinaryFileManager> logger)
    {
        _config = config.Value;
        _logger = logger;

        var cloudName = (Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME") ?? _config.CloudName)?.Trim();
        var apiKey = (Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY") ?? _config.ApiKey)?.Trim();
        var apiSecret = (Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET") ?? _config.ApiSecret)?.Trim();

        _logger.LogInformation("Cloudinary init: CloudName={CloudName}, ApiKey={ApiKeyMasked}",
            cloudName,
            string.IsNullOrEmpty(apiKey) ? "(empty)" : apiKey[..Math.Min(6, apiKey.Length)] + "***");

        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
        {
            throw new InvalidOperationException("Cloudinary credentials are missing in .env or appsettings.json.");
        }

        _cloudinary = new Cloudinary(new Account(cloudName, apiKey, apiSecret));
        _cloudinary.Api.Secure = true;
    }

    public async Task<Result<string>> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return FileErrors.Empty;
        }

        await using var stream = file.OpenReadStream();

        var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}";
        var isImage = !string.IsNullOrWhiteSpace(file.ContentType)
            && file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);

        UploadResult uploadResult;

        if (isImage)
        {
            var imageParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _config.Folder,
                PublicId = uniqueFileName
            };

            uploadResult = await _cloudinary.UploadAsync(imageParams);
        }
        else
        {
            var rawParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = _config.Folder,
                PublicId = uniqueFileName
            };

            uploadResult = await _cloudinary.UploadAsync(rawParams);
        }

        if (uploadResult == null)
        {
            return Error.Failure("File.UploadFailed", "File upload failed");
        }

        if (uploadResult.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Cloudinary upload failed: StatusCode={StatusCode}, Error={Error}",
                uploadResult.StatusCode, uploadResult.Error?.Message ?? "(no error message)");
            return Error.Failure(
                "File.UploadFailed",
                $"File upload failed with status code {uploadResult.StatusCode}");
        }

        var downloadUrl = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString();

        if (string.IsNullOrWhiteSpace(downloadUrl))
        {
            return Error.Failure("File.UploadFailed", "File download url is missing after upload.");
        }

        return downloadUrl;
    }

    public async Task<Result<IEnumerable<FileMetadata>>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
            return FileErrors.Empty;

        var results = new List<FileMetadata>();

        foreach (var file in files)
        {
            var uploadResult = await UploadFileAsync(file);
            if (uploadResult.IsFailure)
                return uploadResult.Error;

            var fileMetadataOrError = FileMetadata.Create(
                uploadResult.Value,
                file.FileName,
                file.ContentType,
                file.Length);

            if (fileMetadataOrError.IsFailure)
                return fileMetadataOrError.Error;

            results.Add(fileMetadataOrError.Value);
        }

        return results;
    }
}
