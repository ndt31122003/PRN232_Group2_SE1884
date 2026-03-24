using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Dtos;

public sealed record ShippingDiscountDto(
    Guid Id,
    string Name,
    string? Description,
    decimal DiscountValue,
    DiscountUnit DiscountUnit,
    bool IsFreeShipping,
    decimal? MinimumOrderValue,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive);
