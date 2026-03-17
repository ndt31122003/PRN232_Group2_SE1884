namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeStatusHistoryDto
{
    public Guid Id { get; init; }
    public string FromStatus { get; init; } = string.Empty;
    public string ToStatus { get; init; } = string.Empty;
    public Guid ChangedById { get; init; }
    public string ChangedByRole { get; init; } = string.Empty;
    public string? Reason { get; init; }
    public DateTime ChangedAt { get; init; }
}
