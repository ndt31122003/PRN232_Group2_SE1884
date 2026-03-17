namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record ReplyToReviewRequest
{
    public string Reply { get; init; } = string.Empty;
}
