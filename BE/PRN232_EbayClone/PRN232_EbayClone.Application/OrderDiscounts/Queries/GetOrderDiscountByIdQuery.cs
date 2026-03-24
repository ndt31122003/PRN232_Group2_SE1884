using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.OrderDiscounts.Dtos;

namespace PRN232_EbayClone.Application.OrderDiscounts.Queries;

public sealed record GetOrderDiscountByIdQuery(Guid DiscountId) : IQuery<OrderDiscountDto>;
