namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record SellerReplyDto
{
    public Guid Id { get; init; }
    public string ReplyText { get; init; } = string.Empty;
    public DateTime RepliedAt { get; init; }
    public DateTime? EditedAt { get; init; }
    public bool IsEdited { get; init; }
}
