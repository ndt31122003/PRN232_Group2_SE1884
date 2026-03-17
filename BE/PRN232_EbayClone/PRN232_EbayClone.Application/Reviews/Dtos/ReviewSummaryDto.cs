namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReviewSummaryDto
{
    public Guid Id { get; init; }
    public Guid OrderId { get; init; }
    public Guid ListingId { get; init; }
    public Guid BuyerId { get; init; }
    public string BuyerUsername { get; init; } = string.Empty;
    public int StarRating { get; init; }
    public string Comment { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string? SellerReply { get; init; }
    public DateTime? RepliedAt { get; init; }
    public bool HasReply { get; init; }
}
