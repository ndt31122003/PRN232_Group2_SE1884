namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReviewDetailDto
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ListingId { get; init; }
    public string ListingTitle { get; init; } = string.Empty;
    public Guid BuyerId { get; init; }
    public string BuyerUsername { get; init; } = string.Empty;
    public string BuyerEmail { get; init; } = string.Empty;
    public int StarRating { get; init; }
    public string Comment { get; init; } = string.Empty;
    public string? FollowUpComment { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? FollowUpCommentedAt { get; init; }
    public SellerReplyDto? SellerReply { get; init; }
}
