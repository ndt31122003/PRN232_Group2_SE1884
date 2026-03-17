namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeMessageDto
{
    public Guid Id { get; init; }
    public Guid SenderId { get; init; }
    public string SenderUsername { get; init; } = string.Empty;
    public string SenderRole { get; init; } = string.Empty;
    public string MessageText { get; init; } = string.Empty;
    public DateTime SentAt { get; init; }
}
