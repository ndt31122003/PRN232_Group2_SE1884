namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeDto(
    Guid Id,
    Guid ListingId,
    string ListingCreatedBy,
    string RaisedById,
    string RaisedByUsername,
    string RaisedByFullName,
    string Reason,
    string Status,
    DateTime CreatedAt,
    List<DisputeResponseDto> Responses
);

public sealed record DisputeResponseDto(
    Guid Id,
    Guid ResponderId,
    string ResponderUsername,
    string Message,
    DateTime CreatedAt
);

public sealed record DisputeFilterDto(
    Guid? ListingId = null,
    string? RaisedById = null,
    string? SellerId = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 20
);
