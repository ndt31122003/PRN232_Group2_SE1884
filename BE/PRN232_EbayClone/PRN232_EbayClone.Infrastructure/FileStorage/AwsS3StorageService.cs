using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PRN232_EbayClone.Application.Abstractions.File;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// File storage service implementation using AWS S3.
/// Supports streaming uploads to handle large files efficiently without loading them entirely into memory.
/// </summary>
public sealed class AwsS3StorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsS3Configuration _configuration;

    public AwsS3StorageService(IAmazonS3 s3Client, IOptions<AwsS3Configuration> options)
    {
        _s3Client = s3Client;
        _configuration = options.Value;
    }

    /// <summary>
    /// Uploads a file to AWS S3 using streaming.
    /// </summary>
    public async Task<string> UploadAsync(IFormFile file, string containerName, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty or null", nameof(file));
        }

        // Validate file
        var validationResult = FileUploadValidator.ValidateFile(file);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }

        try
        {
            // Generate unique file name
            var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(file.FileName);
            var key = $"{containerName}/{uniqueFileName}";

            // Create S3 put request
            var putRequest = new PutObjectRequest
            {
                BucketName = _configuration.BucketName,
                Key = key,
                InputStream = file.OpenReadStream(),
                ContentType = file.ContentType,
                AutoCloseStream = true,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
            };

            // Upload using streaming
            var response = await _s3Client.PutObjectAsync(putRequest, cancellationToken);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"S3 upload failed with status code: {response.HttpStatusCode}");
            }

            // Return the public URL
            var url = GenerateS3Url(key);
            return url;
        }
        catch (Exception ex) when (!(ex is InvalidOperationException))
        {
            throw new InvalidOperationException($"Failed to upload file '{file.FileName}' to AWS S3", ex);
        }
    }

    /// <summary>
    /// Uploads multiple files to AWS S3 using streaming.
    /// </summary>
    public async Task<Dictionary<string, string>> UploadMultipleAsync(IEnumerable<IFormFile> files, string containerName, CancellationToken cancellationToken = default)
    {
        var fileList = files?.ToList() ?? new List<IFormFile>();

        // Validate all files
        var validationResult = FileUploadValidator.ValidateFiles(fileList);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.ErrorMessage);
        }

        var uploadedFiles = new Dictionary<string, string>();

        try
        {
            foreach (var file in fileList)
            {
                var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(file.FileName);
                var key = $"{containerName}/{uniqueFileName}";

                var putRequest = new PutObjectRequest
                {
                    BucketName = _configuration.BucketName,
                    Key = key,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                    AutoCloseStream = true,
                    ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
                };

                var response = await _s3Client.PutObjectAsync(putRequest, cancellationToken);

                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"S3 upload failed for file '{file.FileName}' with status code: {response.HttpStatusCode}");
                }

                var url = GenerateS3Url(key);
                uploadedFiles[file.FileName] = url;
            }

            return uploadedFiles;
        }
        catch (Exception ex) when (!(ex is InvalidOperationException))
        {
            throw new InvalidOperationException("Failed to upload multiple files to AWS S3", ex);
        }
    }

    /// <summary>
    /// Deletes a file from AWS S3.
    /// </summary>
    public async Task<bool> DeleteAsync(string fileUrl, string containerName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            throw new ArgumentException("File URL cannot be empty", nameof(fileUrl));
        }

        try
        {
            // Extract key from URL
            var key = ExtractKeyFromUrl(fileUrl);
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _configuration.BucketName,
                Key = key
            };

            var response = await _s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent ||
                   response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file from AWS S3: {fileUrl}", ex);
        }
    }

    /// <summary>
    /// Gets a public URL for accessing a file in AWS S3.
    /// </summary>
    public string GetUrl(string fileName, string containerName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        }

        var key = $"{containerName}/{fileName}";
        return GenerateS3Url(key);
    }

    /// <summary>
    /// Generates a public S3 URL for a given key.
    /// </summary>
    private string GenerateS3Url(string key)
    {
        if (_configuration.UseCloudFront && !string.IsNullOrEmpty(_configuration.CloudFrontDomain))
        {
            return $"https://{_configuration.CloudFrontDomain}/{key}";
        }

        var region = _configuration.Region ?? "us-east-1";
        return $"https://{_configuration.BucketName}.s3.{region}.amazonaws.com/{key}";
    }

    /// <summary>
    /// Extracts the S3 key from a full S3 URL.
    /// </summary>
    private static string? ExtractKeyFromUrl(string fileUrl)
    {
        try
        {
            var uri = new Uri(fileUrl);
            var path = uri.AbsolutePath.TrimStart('/');

            // Handle CloudFront URLs
            if (uri.Host.Contains("cloudfront"))
            {
                return path;
            }

            // Handle S3 URLs (bucket.s3.region.amazonaws.com/key)
            return path;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Configuration for AWS S3 storage service.
/// </summary>
public sealed class AwsS3Configuration
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool UseCloudFront { get; set; } = false;
    public string? CloudFrontDomain { get; set; }
}
