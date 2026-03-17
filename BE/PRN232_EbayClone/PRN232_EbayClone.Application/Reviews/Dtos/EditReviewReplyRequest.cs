namespace PRN232_EbayClone.Application.Reviews.Dtos;

public sealed record EditReviewReplyRequest
{
    public string NewReply { get; init; } = string.Empty;
}
