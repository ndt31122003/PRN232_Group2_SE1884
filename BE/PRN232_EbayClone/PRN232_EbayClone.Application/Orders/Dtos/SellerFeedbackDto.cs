using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record SellerFeedbackDto(
    Guid Id,
    Guid OrderId,
    Guid SellerId,
    Guid BuyerId,
    string Comment,
    bool UsesStoredFeedback,
    string? StoredFeedbackKey,
    DateTimeOffset CreatedAt,
    string? FollowUpComment,
    DateTimeOffset? FollowUpCommentedAt
);
