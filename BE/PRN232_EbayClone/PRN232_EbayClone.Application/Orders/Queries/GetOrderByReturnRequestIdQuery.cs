using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetOrderByReturnRequestIdQuery(
    Guid SellerId,
    Guid ReturnRequestId) : IQuery<OrderDto>;

public sealed class GetOrderByReturnRequestIdQueryHandler
    : IQueryHandler<GetOrderByReturnRequestIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByReturnRequestIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> Handle(
        GetOrderByReturnRequestIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByReturnRequestIdAsync(
            request.SellerId,
            request.ReturnRequestId,
            cancellationToken);

        if (order is null)
        {
            return Error.Failure("Order.ReturnRequestNotFound", "Order not found for the supplied return request.");
        }

        var dto = order.ToDto();
        return Result.Success(dto);
    }
}
