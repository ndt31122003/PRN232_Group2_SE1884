namespace PRN232_EbayClone.Application.ListingTemplates.Dtos;

public sealed record ListingTemplateDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    string? FormatLabel,
    string? ThumbnailUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string PayloadJson
);
