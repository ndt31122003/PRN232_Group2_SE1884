# File Storage Service

This module provides a cloud-agnostic file storage service for uploading, deleting, and retrieving files from cloud storage providers.

## Features

- **Streaming Uploads**: Efficiently handles large files without loading them entirely into memory
- **File Validation**: Validates file size, type, and extension before upload
- **Multiple Providers**: Supports Azure Blob Storage and AWS S3
- **Unique File Names**: Automatically generates unique file names to prevent collisions
- **Error Handling**: Comprehensive error handling and validation

## Supported Providers

### Azure Blob Storage
- Connection string-based authentication
- Automatic container creation
- Public blob access for file retrieval

### AWS S3
- IAM credentials-based authentication
- Automatic bucket configuration
- CloudFront CDN support for optimized delivery

## Configuration

### Azure Blob Storage

Add to `appsettings.json`:

```json
{
  "FileStorage": {
    "Provider": "Azure",
    "AzureBlob": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...;AccountKey=...;EndpointSuffix=core.windows.net"
    }
  }
}
```

### AWS S3

Add to `appsettings.json`:

```json
{
  "FileStorage": {
    "Provider": "S3",
    "AwsS3": {
      "BucketName": "my-bucket",
      "Region": "us-east-1",
      "AccessKey": "AKIAIOSFODNN7EXAMPLE",
      "SecretKey": "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
      "UseCloudFront": false,
      "CloudFrontDomain": "d123456.cloudfront.net"
    }
  }
}
```

## Usage Examples

### Single File Upload

```csharp
public class ProvideEvidenceCommandHandler : ICommandHandler<ProvideEvidenceCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public ProvideEvidenceCommandHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<Result> Handle(ProvideEvidenceCommand command, CancellationToken cancellationToken)
    {
        // Upload file to storage
        var fileUrl = await _fileStorageService.UploadAsync(
            file: command.File,
            containerName: "dispute-evidence",
            cancellationToken: cancellationToken
        );

        // Create evidence record with the URL
        var evidence = DisputeEvidence.Create(
            disputeId: command.DisputeId,
            uploadedById: command.UserId,
            fileName: command.File.FileName,
            fileUrl: fileUrl,
            fileType: command.File.ContentType,
            fileSize: command.File.Length
        );

        return Result.Success();
    }
}
```

### Multiple File Upload

```csharp
public class CreateSupportTicketCommandHandler : ICommandHandler<CreateSupportTicketCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public async Task<Result> Handle(CreateSupportTicketCommand command, CancellationToken cancellationToken)
    {
        var uploadedFiles = new Dictionary<string, string>();

        if (command.Attachments?.Any() == true)
        {
            // Upload all attachments
            uploadedFiles = await _fileStorageService.UploadMultipleAsync(
                files: command.Attachments,
                containerName: "support-tickets",
                cancellationToken: cancellationToken
            );
        }

        // Create ticket with attachment URLs
        var ticket = SupportTicket.Create(
            sellerId: command.SellerId,
            category: command.Category,
            subject: command.Subject,
            message: command.Message,
            priority: command.Priority
        );

        // Add attachments
        foreach (var (fileName, fileUrl) in uploadedFiles)
        {
            ticket.AddAttachment(fileName, fileUrl);
        }

        return Result.Success();
    }
}
```

### File Deletion

```csharp
public class DeleteDisputeEvidenceCommandHandler : ICommandHandler<DeleteDisputeEvidenceCommand>
{
    private readonly IFileStorageService _fileStorageService;

    public async Task<Result> Handle(DeleteDisputeEvidenceCommand command, CancellationToken cancellationToken)
    {
        var success = await _fileStorageService.DeleteAsync(
            fileUrl: command.FileUrl,
            containerName: "dispute-evidence",
            cancellationToken: cancellationToken
        );

        if (!success)
        {
            return Result.Failure("Failed to delete file from storage");
        }

        return Result.Success();
    }
}
```

### Get File URL

```csharp
public class GetFileUrlQueryHandler : IQueryHandler<GetFileUrlQuery, string>
{
    private readonly IFileStorageService _fileStorageService;

    public Task<Result<string>> Handle(GetFileUrlQuery query, CancellationToken cancellationToken)
    {
        var url = _fileStorageService.GetUrl(
            fileName: query.FileName,
            containerName: "dispute-evidence"
        );

        return Task.FromResult(Result.Success(url));
    }
}
```

## File Validation

The `FileUploadValidator` class provides validation for:

- **File Size**: Maximum 10MB per file
- **File Types**: Images (JPG, PNG, GIF, WebP), PDF, Documents (DOC, DOCX, XLS, XLSX, TXT)
- **File Count**: Maximum 10 files per upload batch
- **MIME Type**: Validates against allowed MIME types

### Validation Example

```csharp
var validationResult = FileUploadValidator.ValidateFile(file);
if (!validationResult.IsValid)
{
    return Result.Failure(validationResult.ErrorMessage);
}

// Validate multiple files
var multiValidationResult = FileUploadValidator.ValidateFiles(files, maxFileCount: 5);
if (!multiValidationResult.IsValid)
{
    return Result.Failure(multiValidationResult.ErrorMessage);
}
```

## Container Names

Use consistent container names for organizing files:

- `dispute-evidence`: Evidence files for disputes
- `support-tickets`: Attachments for support tickets
- `review-images`: Images for reviews (future enhancement)

## Error Handling

The service throws `InvalidOperationException` with descriptive messages for:

- Invalid file validation
- Upload failures
- Deletion failures
- Configuration issues

Always wrap service calls in try-catch blocks or use Result pattern for error handling.

## Performance Considerations

1. **Streaming**: Files are uploaded using streaming to avoid memory buffering
2. **Unique Names**: Automatically generated unique file names prevent collisions
3. **Async Operations**: All operations are async for non-blocking I/O
4. **Cancellation**: Support for cancellation tokens for graceful shutdown

## Security Considerations

1. **File Validation**: All files are validated before upload
2. **Unique Names**: Original file names are not preserved to prevent directory traversal
3. **MIME Type Checking**: Validates MIME types to prevent malicious uploads
4. **Size Limits**: Enforces maximum file size to prevent storage abuse
5. **Encryption**: AWS S3 uploads use server-side encryption (AES256)

## Migration from Cloudinary

If migrating from Cloudinary's `IFileManager`:

1. The new `IFileStorageService` provides similar functionality with cloud-agnostic design
2. Update dependency injection to use `IFileStorageService` instead of `IFileManager`
3. Update container names to match your organization strategy
4. Test file uploads and retrievals in staging environment

## Testing

When testing file uploads:

```csharp
// Mock the service
var mockFileStorageService = new Mock<IFileStorageService>();
mockFileStorageService
    .Setup(x => x.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync("https://example.com/file.jpg");

// Use in tests
var handler = new ProvideEvidenceCommandHandler(mockFileStorageService.Object);
```
