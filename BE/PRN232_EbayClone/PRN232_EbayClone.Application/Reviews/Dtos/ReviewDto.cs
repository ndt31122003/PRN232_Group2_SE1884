namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReviewDto(
    Guid Id,
    Guid ListingId,
    string ReviewerId,
    string ReviewerRole,
    string ReviewerUsername,
    string ReviewerFullName,
    string RecipientId,
    string RecipientRole,
    int Rating,
    string RatingType,
    string? Comment,
    string? Reply,
    DateTime? RepliedAt,
    string RevisionStatus,
    DateTime? RevisionRequestedAt,
    DateTime CreatedAt
);

public sealed record ReviewFilterDto(
    Guid? ListingId = null,
    string? ReviewerId = null,
    string? ReviewerRole = null,
    string? RecipientId = null,
    string? RecipientRole = null,
    string? SellerId = null,
    int? Rating = null,
    string? RatingType = null,
    string? RevisionStatus = null,
    bool? HasReply = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 20
);

public sealed record ReviewStatsDto(
    Dictionary<string, Dictionary<string, int>> RatingStats,
    int TotalReviews,
    decimal AverageRating
);
