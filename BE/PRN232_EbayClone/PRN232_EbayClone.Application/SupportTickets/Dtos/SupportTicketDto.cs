namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketDto(
    Guid Id,
    string SellerId,
    string Category,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int AttachmentCount
);

public sealed record SupportTicketDetailDto(
    Guid Id,
    string SellerId,
    string Category,
    string Subject,
    string Message,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<TicketAttachmentDto> Attachments
);

public sealed record TicketAttachmentDto(
    Guid Id,
    string FileName,
    string ContentType,
    long Size,
    string Url,
    DateTime UploadedAt
);

public sealed record SupportTicketFilterDto(
    string? Status = null,
    string? Category = null,
    string? Keyword = null,
    int Page = 1,
    int PageSize = 20
);

public sealed record CreateSupportTicketDto(
    string Category,
    string Subject,
    string Message
);