using Microsoft.AspNetCore.Http;

namespace PRN232_EbayClone.Application.Disputes.Dtos;

public sealed record ProvideEvidenceRequest
{
    public IFormFileCollection Files { get; init; } = default!;
    public string? Description { get; init; }
}
