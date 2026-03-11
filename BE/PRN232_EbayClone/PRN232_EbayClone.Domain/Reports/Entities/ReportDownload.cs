using PRN232_EbayClone.Domain.Reports.Enums;
using PRN232_EbayClone.Domain.Reports.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Reports.Entities;

public class ReportDownload : AggregateRoot<Guid>
{
    private const int SourceMaxLength = 64;
    private const int TypeMaxLength = 128;
    private const int ReferenceCodeMaxLength = 32;
    private const int FileUrlMaxLength = 512;

    public UserId UserId { get; private set; }
    public string ReferenceCode { get; private set; } = null!;
    public string Source { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public DateTime RequestedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public ReportDownloadStatus Status { get; private set; }
    public string? FileUrl { get; private set; }
    public ReportDateRange? DateRange { get; private set; }

    private ReportDownload(Guid id) : base(id)
    {
    }

    public static Result<ReportDownload> Create(
        UserId userId,
        string source,
        string type,
        ReportDownloadStatus initialStatus,
        DateTime requestedAtUtc,
        string? referenceCode = null,
        ReportDateRange? dateRange = null,
        string? fileUrl = null)
    {
        if (userId.Value == Guid.Empty)
        {
            return Error.Failure("ReportDownload.InvalidUser","UserId cannot be empty.");
        }

        var sanitizedSource = SanitizeAndValidateLabel(source, SourceMaxLength, "ReportDownload.InvalidSource", "Source is required.");
        if (sanitizedSource.IsFailure)
        {
            return sanitizedSource.Error;
        }

        var sanitizedType = SanitizeAndValidateLabel(type, TypeMaxLength, "ReportDownload.InvalidType", "Type is required.");
        if (sanitizedType.IsFailure)
        {
            return sanitizedType.Error;
        }

        var normalizedReference = string.IsNullOrWhiteSpace(referenceCode)
            ? GenerateReferenceCode()
            : referenceCode.Trim();

        if (normalizedReference.Length > ReferenceCodeMaxLength)
        {
            return Error.Failure("ReportDownload.ReferenceTooLong",$"Reference code must be {ReferenceCodeMaxLength} characters or fewer.");
        }

        string? sanitizedFileUrl = null;
        if (!string.IsNullOrWhiteSpace(fileUrl))
        {
            var trimmed = fileUrl.Trim();
            if (trimmed.Length > FileUrlMaxLength)
            {
                return Error.Failure("ReportDownload.FileUrlTooLong",$"File url must be {FileUrlMaxLength} characters or fewer.");
            }

            sanitizedFileUrl = trimmed;
        }

        var download = new ReportDownload(Guid.NewGuid())
        {
            UserId = userId,
            Source = sanitizedSource.Value,
            Type = sanitizedType.Value,
            Status = initialStatus,
            RequestedAtUtc = requestedAtUtc,
            ReferenceCode = normalizedReference,
            CompletedAtUtc = null,
            FileUrl = sanitizedFileUrl,
            DateRange = dateRange
        };

        return Result.Success(download);
    }

    public Result MarkProcessing()
    {
        if (Status == ReportDownloadStatus.Processing)
        {
            return Result.Success();
        }

        if (Status is ReportDownloadStatus.Completed or ReportDownloadStatus.Failed)
        {
            return Error.Failure("ReportDownload.InvalidState","Cannot mark download as processing once it has finished.");
        }

        Status = ReportDownloadStatus.Processing;
        return Result.Success();
    }

    public Result MarkCompleted(string fileUrl, DateTime completedAtUtc)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Error.Failure("ReportDownload.InvalidFileUrl","A download url is required to mark as completed.");
        }

        var trimmed = fileUrl.Trim();
        if (trimmed.Length > FileUrlMaxLength)
        {
            return Error.Failure("ReportDownload.FileUrlTooLong",$"File url must be {FileUrlMaxLength} characters or fewer.");
        }

        if (Status == ReportDownloadStatus.Failed)
        {
            return Error.Failure("ReportDownload.InvalidState","Cannot mark a failed download as completed.");
        }

        FileUrl = trimmed;
        CompletedAtUtc = completedAtUtc;
        Status = ReportDownloadStatus.Completed;
        return Result.Success();
    }

    public Result MarkFailed()
    {
        if (Status == ReportDownloadStatus.Completed)
        {
            return Error.Failure("ReportDownload.InvalidState","Completed downloads cannot be marked as failed.");
        }

        Status = ReportDownloadStatus.Failed;
        CompletedAtUtc = DateTime.UtcNow;
        return Result.Success();
    }

    public void UpdateDateRange(ReportDateRange? dateRange)
    {
        DateRange = dateRange;
    }

    private static Result<string> SanitizeAndValidateLabel(string value, int maxLength, string code, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Failure(code, message);
        }

        var sanitized = value.Trim();
        if (sanitized.Length > maxLength)
        {
            return Error.Failure(code,$"Value must be {maxLength} characters or fewer.");
        }

        return Result.Success(sanitized);
    }

    private static string GenerateReferenceCode()
    {
        Span<char> buffer = stackalloc char[10];
        var random = Guid.NewGuid().ToString("N");
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = random[i];
        }

        return new string(buffer).ToUpperInvariant();
    }
}
