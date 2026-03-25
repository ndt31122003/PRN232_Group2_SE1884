using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Commands;

public sealed record CreateShippingDiscountCommand(
    Guid SellerId,
    string Name,
    string? Description,
    decimal DiscountValue,
    DiscountUnit DiscountUnit,
    bool IsFreeShipping,
    decimal? MinimumOrderValue,
    DateTime StartDate,
    DateTime EndDate) : ICommand<Guid>;
