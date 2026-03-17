using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SupportTickets.Entities;

public sealed class SupportTicketAttachment : Entity<Guid>
{
    public const long MaxFileSize = 10 * 1024 * 1024; // 10MB

    public Guid TicketId { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;
    public string FileType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; }

    // Audit fields
    public DateTimeOffset CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public string? UpdatedBy { get; private set; }
    public bool IsDeleted { get; private set; }

    // Navigation properties
    public SupportTicket? Ticket { get; private set; }

    private SupportTicketAttachment() : base(Guid.Empty)
    {
    }

    private SupportTicketAttachment(
        Guid id,
        Guid ticketId,
        string fileName,
        string fileUrl,
        string fileType,
        long fileSize,
        DateTimeOffset uploadedAt,
        string? createdBy) : base(id)
    {
        TicketId = ticketId;
        FileName = fileName;
        FileUrl = fileUrl;
        FileType = fileType;
        FileSize = fileSize;
        UploadedAt = uploadedAt;
        CreatedAt = uploadedAt;
        CreatedBy = createdBy;
        IsDeleted = false;
    }

    public static Result<SupportTicketAttachment> Create(
        Guid ticketId,
        string fileName,
        string fileUrl,
        string fileType,
        long fileSize,
        DateTimeOffset uploadedAt,
        string? createdBy = null)
    {
        if (ticketId == Guid.Empty)
        {
            return Error.Validation("SupportTicketAttachment.InvalidTicket", "Ticket ID is required");
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Error.Validation("SupportTicketAttachment.FileNameRequired", "File name is required");
        }

        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return Error.Validation("SupportTicketAttachment.FileUrlRequired", "File URL is required");
        }

        if (string.IsNullOrWhiteSpace(fileType))
        {
            return Error.Validation("SupportTicketAttachment.FileTypeRequired", "File type is required");
        }

        if (fileSize <= 0)
        {
            return Error.Validation("SupportTicketAttachment.InvalidFileSize", "File size must be positive");
        }

        if (fileSize > MaxFileSize)
        {
            return Error.Validation(
                "SupportTicketAttachment.FileTooLarge",
                $"File size cannot exceed {MaxFileSize / (1024 * 1024)}MB");
        }

        var attachment = new SupportTicketAttachment(
            Guid.NewGuid(),
            ticketId,
            fileName,
            fileUrl,
            fileType,
            fileSize,
            uploadedAt,
            createdBy);

        return attachment;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = deletedBy;
    }
}
