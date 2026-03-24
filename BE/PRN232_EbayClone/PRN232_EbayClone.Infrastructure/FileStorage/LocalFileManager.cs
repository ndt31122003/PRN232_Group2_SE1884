using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Domain.FileMetadata.Entities;
using PRN232_EbayClone.Domain.FileMetadata.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

public sealed class LocalFileManager : IFileManager
{
    private readonly string _storageRoot;
    private readonly string _baseUrl;

    public LocalFileManager()
    {
        _storageRoot = Path.Combine(AppContext.BaseDirectory, "storage", "uploads");
        _baseUrl = "/storage/uploads";
        
        // Ensure directory exists
        if (!Directory.Exists(_storageRoot))
        {
            Directory.CreateDirectory(_storageRoot);
        }
    }

    public async Task<Result<string>> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return FileErrors.Empty;
        }

        try
        {
            // Generate unique filename
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_storageRoot, uniqueFileName);

            // Save file to disk
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return URL path
            var fileUrl = $"{_baseUrl}/{uniqueFileName}";
            return fileUrl;
        }
        catch (Exception ex)
        {
            return Error.Failure("File.UploadFailed", $"File upload failed: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<FileMetadata>>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            return FileErrors.Empty;
        }

        var results = new List<FileMetadata>();

        foreach (var file in files)
        {
            var uploadResult = await UploadFileAsync(file);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            var fileMetadataOrError = FileMetadata.Create(
                uploadResult.Value,
                file.FileName,
                file.ContentType ?? "application/octet-stream",
                file.Length);

            if (fileMetadataOrError.IsFailure)
            {
                return fileMetadataOrError.Error;
            }

            results.Add(fileMetadataOrError.Value);
        }

        return results;
    }
}
