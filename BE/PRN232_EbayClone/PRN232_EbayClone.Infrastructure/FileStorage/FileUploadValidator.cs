using Microsoft.AspNetCore.Http;

namespace PRN232_EbayClone.Infrastructure.FileStorage;

/// <summary>
/// Validates file uploads for size, type, extension constraints, and optional virus scanning.
/// </summary>
public sealed class FileUploadValidator
{
    private readonly IVirusScanService? _virusScanService;

    // Maximum file size: 10MB
    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    /// <summary>
    /// Creates a new instance of FileUploadValidator.
    /// </summary>
    /// <param name="virusScanService">Optional virus scan service for malware detection</param>
    public FileUploadValidator(IVirusScanService? virusScanService = null)
    {
        _virusScanService = virusScanService;
    }

    // Allowed MIME types for different file categories
    private static readonly HashSet<string> AllowedImageMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/gif",
        "image/webp"
    };

    private static readonly HashSet<string> AllowedDocumentMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/pdf",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "text/plain"
    };

    // Allowed file extensions
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp",
        ".pdf",
        ".doc", ".docx",
        ".xls", ".xlsx",
        ".txt"
    };

    /// <summary>
    /// Validates a single file for upload.
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <returns>Validation result with error message if invalid</returns>
    public static ValidationResult ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ValidationResult.Failure("File is empty or null");
        }

        // Check file size
        if (file.Length > MaxFileSizeBytes)
        {
            return ValidationResult.Failure($"File size exceeds maximum allowed size of 10MB. Actual size: {FormatBytes(file.Length)}");
        }

        // Check file extension
        var extension = Path.GetExtension(file.FileName);
        if (!AllowedExtensions.Contains(extension))
        {
            return ValidationResult.Failure($"File extension '{extension}' is not allowed. Allowed extensions: {string.Join(", ", AllowedExtensions)}");
        }

        // Check MIME type
        if (!IsValidMimeType(file.ContentType))
        {
            return ValidationResult.Failure($"File type '{file.ContentType}' is not allowed");
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates multiple files for upload.
    /// </summary>
    /// <param name="files">Collection of files to validate</param>
    /// <param name="maxFileCount">Maximum number of files allowed</param>
    /// <returns>Validation result with error message if invalid</returns>
    public static ValidationResult ValidateFiles(IEnumerable<IFormFile> files, int maxFileCount = 10)
    {
        var fileList = files?.ToList() ?? new List<IFormFile>();

        if (!fileList.Any())
        {
            return ValidationResult.Failure("No files provided");
        }

        if (fileList.Count > maxFileCount)
        {
            return ValidationResult.Failure($"Too many files. Maximum allowed: {maxFileCount}, provided: {fileList.Count}");
        }

        // Validate each file
        foreach (var file in fileList)
        {
            var result = ValidateFile(file);
            if (!result.IsValid)
            {
                return result;
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Checks if a MIME type is allowed for upload.
    /// </summary>
    private static bool IsValidMimeType(string mimeType)
    {
        if (string.IsNullOrWhiteSpace(mimeType))
        {
            return false;
        }

        return AllowedImageMimeTypes.Contains(mimeType) || AllowedDocumentMimeTypes.Contains(mimeType);
    }

    /// <summary>
    /// Formats bytes to human-readable format.
    /// </summary>
    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    /// <summary>
    /// Validates a single file for upload with optional virus scanning.
    /// </summary>
    /// <param name="file">The file to validate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result with error message if invalid</returns>
    public async Task<ValidationResult> ValidateFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        // First perform basic validation
        var basicValidation = ValidateFile(file);
        if (!basicValidation.IsValid)
        {
            return basicValidation;
        }

        // Perform virus scanning if available
        if (_virusScanService?.IsAvailable() == true)
        {
            try
            {
                await using var stream = file.OpenReadStream();
                var isClean = await _virusScanService.ScanAsync(stream, file.FileName, cancellationToken);

                if (!isClean)
                {
                    return ValidationResult.Failure($"File '{file.FileName}' failed virus scan. The file may contain malware.");
                }
            }
            catch (Exception ex)
            {
                return ValidationResult.Failure($"Error scanning file '{file.FileName}' for viruses: {ex.Message}");
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates multiple files for upload with optional virus scanning.
    /// </summary>
    /// <param name="files">Collection of files to validate</param>
    /// <param name="maxFileCount">Maximum number of files allowed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result with error message if invalid</returns>
    public async Task<ValidationResult> ValidateFilesAsync(IEnumerable<IFormFile> files, int maxFileCount = 10, CancellationToken cancellationToken = default)
    {
        var fileList = files?.ToList() ?? new List<IFormFile>();

        if (!fileList.Any())
        {
            return ValidationResult.Failure("No files provided");
        }

        if (fileList.Count > maxFileCount)
        {
            return ValidationResult.Failure($"Too many files. Maximum allowed: {maxFileCount}, provided: {fileList.Count}");
        }

        // Validate each file with virus scanning
        foreach (var file in fileList)
        {
            var result = await ValidateFileAsync(file, cancellationToken);
            if (!result.IsValid)
            {
                return result;
            }
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Generates a unique file name to prevent collisions.
    /// </summary>
    /// <param name="originalFileName">The original file name</param>
    /// <returns>A unique file name with timestamp and GUID</returns>
    public static string GenerateUniqueFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var guid = Guid.NewGuid().ToString("N").Substring(0, 8);

        return $"{nameWithoutExtension}_{timestamp}_{guid}{extension}";
    }
}

/// <summary>
/// Represents the result of file validation.
/// </summary>
public sealed class ValidationResult
{
    public bool IsValid { get; private set; }
    public string? ErrorMessage { get; private set; }

    private ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    public static ValidationResult Success() => new(true);
    public static ValidationResult Failure(string errorMessage) => new(false, errorMessage);
}
