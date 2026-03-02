using PRN232_EbayClone.Application.Orders.Dtos;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetOrderStatusesQuery(): IQuery<List<OrderStatusDto>>;
public sealed class GetOrderStatusesQueryHandler : IQueryHandler<GetOrderStatusesQuery, List<OrderStatusDto>>
{
    private readonly IOrderRepository _orderRepository;
    public GetOrderStatusesQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result<List<OrderStatusDto>>> Handle(GetOrderStatusesQuery request, CancellationToken cancellationToken)
    {
        var statuses = await _orderRepository.GetAllOrderStatusesAsync(cancellationToken);
        if (statuses is null || !statuses.Any())
        {
            return Error.Failure("GetOrderStatuses.NoStatuses", "No order statuses found.");
        }
        var statusDtos = statuses.Select(s => new OrderStatusDto(s.Id, s.Code, s.Name, s.Color)).ToList();
        return Result.Success(statusDtos);
    }
}

