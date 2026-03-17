namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record DisputeEvidenceDto
{
    public Guid Id { get; init; }
    public Guid UploadedById { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string FileType { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string? Description { get; init; }
    public DateTime UploadedAt { get; init; }
}
