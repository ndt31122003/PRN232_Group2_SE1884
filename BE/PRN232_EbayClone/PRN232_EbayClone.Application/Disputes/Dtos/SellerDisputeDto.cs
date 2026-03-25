namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record SellerDisputeDto(
    Guid Id,
    Guid ListingId,
    string ListingTitle,
    string RaisedById,
    string BuyerName,
    string Reason,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? Deadline,
    bool IsDeadlineSoon,
    int EvidenceCount,
    List<SellerDisputeResponseDto> Responses
);

public sealed record SellerDisputeDetailDto(
    Guid Id,
    Guid ListingId,
    string ListingTitle,
    string RaisedById,
    string BuyerName,
    string Reason,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? Deadline,
    bool IsDeadlineSoon,
    List<DisputeEvidenceDto> Evidence
);

public sealed record DisputeEvidenceDto(
    Guid Id,
    string FileName,
    string ContentType,
    long Size,
    string Url,
    DateTime UploadedAt
);

public sealed record SellerDisputeResponseDto(
    Guid Id,
    string ResponderId,
    string Message,
    DateTime CreatedAt
);

public sealed record SellerDisputeFilterDto(
    string? Status = null,
    bool? DeadlineSoon = null,
    int Page = 1,
    int PageSize = 20
);