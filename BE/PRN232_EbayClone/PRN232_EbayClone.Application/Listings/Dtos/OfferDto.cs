using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Application.Listings.Dtos;

public record OfferDto(
    Guid Id,
    Guid ListingId,
    string ListingTitle,
    string? ListingThumbnail,
    string BuyerId,
    string BuyerName,
    decimal Amount,
    decimal CurrentPrice,
    OfferStatus Status,
    DateTime CreatedAt);
