using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PRN232_EbayClone.Application.Abstractions.File;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// File storage service implementation using Azure Blob Storage.
/// Supports streaming uploads to handle large files efficiently without loading them entirely into memory.
/// </summary>
public sealed class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobStorageConfiguration _configuration;

    public AzureBlobStorageService(IOptions<AzureBlobStorageConfiguration> options)
    {
        _configuration = options.Value;
        _blobServiceClient = new BlobServiceClient(_configuration.ConnectionString);
    }

    /// <summary>
    /// Uploads a file to Azure Blob Storage using streaming.
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
            // Get or create container
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

            // Generate unique file name
            var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(file.FileName);

            // Get blob client
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            // Upload using streaming to avoid memory buffering
            await using var stream = file.OpenReadStream();
            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                }
            };

            await blobClient.UploadAsync(stream, overwrite: true, options: uploadOptions, cancellationToken: cancellationToken);

            // Return the public URL
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to upload file '{file.FileName}' to Azure Blob Storage", ex);
        }
    }

    /// <summary>
    /// Uploads multiple files to Azure Blob Storage using streaming.
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
            // Get or create container
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

            // Upload each file
            foreach (var file in fileList)
            {
                var uniqueFileName = FileUploadValidator.GenerateUniqueFileName(file.FileName);
                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                await using var stream = file.OpenReadStream();
                var uploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    }
                };

                await blobClient.UploadAsync(stream, overwrite: true, options: uploadOptions, cancellationToken: cancellationToken);

                uploadedFiles[file.FileName] = blobClient.Uri.ToString();
            }

            return uploadedFiles;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to upload multiple files to Azure Blob Storage", ex);
        }
    }

    /// <summary>
    /// Deletes a file from Azure Blob Storage.
    /// </summary>
    public async Task<bool> DeleteAsync(string fileUrl, string containerName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            throw new ArgumentException("File URL cannot be empty", nameof(fileUrl));
        }

        try
        {
            // Extract blob name from URL
            var blobName = ExtractBlobNameFromUrl(fileUrl);
            if (string.IsNullOrEmpty(blobName))
            {
                return false;
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to delete file from Azure Blob Storage: {fileUrl}", ex);
        }
    }

    /// <summary>
    /// Gets a public URL for accessing a file in Azure Blob Storage.
    /// </summary>
    public string GetUrl(string fileName, string containerName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(fileName);

        return blobClient.Uri.ToString();
    }

    /// <summary>
    /// Extracts the blob name from a full Azure Blob Storage URL.
    /// </summary>
    private static string? ExtractBlobNameFromUrl(string fileUrl)
    {
        try
        {
            var uri = new Uri(fileUrl);
            var path = uri.AbsolutePath;

            // Remove leading slash and container name
            var parts = path.TrimStart('/').Split('/', 2);
            return parts.Length > 1 ? parts[1] : null;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Configuration for Azure Blob Storage service.
/// </summary>
public sealed class AzureBlobStorageConfiguration
{
    public string ConnectionString { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountKey { get; set; } = string.Empty;
}
