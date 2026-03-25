using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.ShippingDiscounts.Dtos;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Queries;

public sealed record GetShippingDiscountByIdQuery(Guid DiscountId) : IQuery<ShippingDiscountDto>;
