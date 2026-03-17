using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Disputes.Entities;

public sealed class DisputeEvidence : Entity<Guid>
{
    public const int MaxDescriptionLength = 500;
    public const long MaxFileSize = 10 * 1024 * 1024; // 10MB

    public Guid DisputeId { get; private set; }
    public Guid UploadedById { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;
    public string FileType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public Dispute? Dispute { get; private set; }

    private DisputeEvidence() : base(Guid.Empty)
    {
    }

    private DisputeEvidence(
        Guid id,
        Guid disputeId,
        Guid uploadedById,
        string fileName,
        string fileUrl,
        string fileType,
        long fileSize,
        string? description,
        DateTimeOffset uploadedAt,
        string? createdBy) : base(id)
    {
        DisputeId = disputeId;
        UploadedById = uploadedById;
        FileName = fileName;
        FileUrl = fileUrl;
        FileType = fileType;
        FileSize = fileSize;
        Description = description;
        UploadedAt = uploadedAt;
        CreatedAt = uploadedAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<DisputeEvidence> Create(
        Guid disputeId,
        Guid uploadedById,
        string fileName,
        string fileUrl,
        string fileType,
        long fileSize,
        string? description,
        DateTimeOffset uploadedAt,
        string? createdBy = null)
    {
        if (disputeId == Guid.Empty)
        {
            return Error.Validation("DisputeEvidence.InvalidDispute", "Dispute ID is required");
        }

        if (uploadedById == Guid.Empty)
        {
            return Error.Validation("DisputeEvidence.InvalidUploader", "Uploader ID is required");
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Error.Validation("DisputeEvidence.FileNameRequired", "File name is required");
        }

        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Error.Validation("DisputeEvidence.FileUrlRequired", "File URL is required");
        }

        if (string.IsNullOrWhiteSpace(fileType))
        {
            return Error.Validation("DisputeEvidence.FileTypeRequired", "File type is required");
        }

        if (fileSize <= 0)
        {
            return Error.Validation("DisputeEvidence.InvalidFileSize", "File size must be positive");
        }

        if (fileSize > MaxFileSize)
        {
            return Error.Validation(
                "DisputeEvidence.FileTooLarge",
                $"File size cannot exceed {MaxFileSize / (1024 * 1024)}MB");
        }

        var trimmedDescription = description?.Trim();
        if (!string.IsNullOrEmpty(trimmedDescription) && trimmedDescription.Length > MaxDescriptionLength)
        {
            return Error.Validation(
                "DisputeEvidence.DescriptionTooLong",
                $"Description cannot exceed {MaxDescriptionLength} characters");
        }

        var evidence = new DisputeEvidence(
            Guid.NewGuid(),
            disputeId,
            uploadedById,
            fileName,
            fileUrl,
            fileType,
            fileSize,
            trimmedDescription,
            uploadedAt,
            createdBy);

        return evidence;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }
}
