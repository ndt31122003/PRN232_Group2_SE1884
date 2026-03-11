using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetOrderByCancellationRequestIdQuery(
    Guid SellerId,
    Guid CancellationRequestId) : IQuery<OrderDto>;

public sealed class GetOrderByCancellationRequestIdQueryHandler
    : IQueryHandler<GetOrderByCancellationRequestIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByCancellationRequestIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> Handle(
        GetOrderByCancellationRequestIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetOrderByCancellationRequestIdAsync(
            request.SellerId,
            request.CancellationRequestId,
            cancellationToken);

        if (order is null)
        {
            return Error.Failure("Order.CancellationRequestNotFound", "Order not found for the supplied cancellation request.");
        }

        var dto = order.ToDto();
        return Result.Success(dto);
    }
}
