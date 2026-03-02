using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventEligibleListingDto(
    Guid ListingId,
    string Title,
    string? Sku,
    string? ThumbnailUrl,
    ListingFormat Format,
    Guid CategoryId,
    decimal BasePrice,
    DateTime CreatedAt,
    DateTime? StartDate,
    DateTime? EndDate,
    bool IsAlreadyAssigned);
