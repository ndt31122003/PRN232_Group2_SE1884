namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeDto(
    Guid Id,
    Guid ListingId,
    string RaisedById,
    string RaisedByUsername,
    string RaisedByFullName,
    string Reason,
    string Status,
    DateTime CreatedAt
);

public sealed record DisputeFilterDto(
    Guid? ListingId = null,
    string? RaisedById = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 20
);
