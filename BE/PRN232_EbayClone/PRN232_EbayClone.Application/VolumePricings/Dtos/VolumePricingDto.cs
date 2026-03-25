namespace PRN232_EbayClone.Application.VolumePricings.Dtos;

public sealed record VolumePricingDto(
    Guid Id,
    string Name,
    string? Description,
    Guid? ListingId,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    IReadOnlyList<VolumePricingTierDto> Tiers);
