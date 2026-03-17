using Microsoft.AspNetCore.Http;

namespace PRN232_EbayClone.Application.Abstractions.File;

/// <summary>
/// Interface for file storage service supporting streaming uploads to cloud storage.
/// Supports Azure Blob Storage, AWS S3, or similar cloud storage providers.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to cloud storage using streaming to avoid memory buffering.
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="containerName">The container/bucket name for organizing files</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The URL of the uploaded file</returns>
    Task<string> UploadAsync(IFormFile file, string containerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads multiple files to cloud storage using streaming.
    /// </summary>
    /// <param name="files">Collection of files to upload</param>
    /// <param name="containerName">The container/bucket name for organizing files</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Dictionary mapping original file names to their URLs</returns>
    Task<Dictionary<string, string>> UploadMultipleAsync(IEnumerable<IFormFile> files, string containerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from cloud storage.
    /// </summary>
    /// <param name="fileUrl">The URL or path of the file to delete</param>
    /// <param name="containerName">The container/bucket name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    Task<bool> DeleteAsync(string fileUrl, string containerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a public URL for accessing a file in cloud storage.
    /// </summary>
    /// <param name="fileName">The file name or path</param>
    /// <param name="containerName">The container/bucket name</param>
    /// <returns>The public URL for accessing the file</returns>
    string GetUrl(string fileName, string containerName);
}
