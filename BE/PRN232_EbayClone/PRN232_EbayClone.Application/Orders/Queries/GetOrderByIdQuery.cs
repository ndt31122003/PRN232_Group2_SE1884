using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.Queries
{
    public sealed record GetOrderByIdQuery(Guid id) : IQuery<OrderDto>;
    public sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery,  OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.id, cancellationToken);
            if (order is null)
                return Error.Failure("GetOrder.OrderNotFound", "Order not found.");
            var orderDto = order.ToDto();
            return Result.Success(orderDto);
        }
    }

}
