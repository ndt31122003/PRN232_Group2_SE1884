using System;
using System.Linq;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetOrdersQuery(
    Guid UserId,
    OrderFilterDto Filter
) : IQuery<PagingResult<OrderDto>>;

public sealed class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty.");
        RuleFor(x => x.Filter).NotNull().WithMessage("Filter cannot be null.");
        When(x => x.Filter.Period == OrderPeriod.Custom, () =>
        {
            RuleFor(x => x.Filter.FromDate)
                .NotNull().WithMessage("StartDate is required for custom period.")
                .LessThanOrEqualTo(x => x.Filter.ToDate).WithMessage("StartDate must be before or equal to EndDate.");
            RuleFor(x => x.Filter.ToDate)
                .NotNull().WithMessage("EndDate is required for custom period.")
                .GreaterThanOrEqualTo(x => x.Filter.FromDate).WithMessage("EndDate must be after or equal to StartDate.");
        });
    }
}


public sealed class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagingResult<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    public GetOrdersQueryHandler(IOrderRepository orderRepository, IUserRepository userRepository)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
    }
    public async Task<Result<PagingResult<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
        if (user is null)
            return Error.Failure("GetOrders.UserNotFound", "User not found.");
        var (orders, totalCount) = await _orderRepository.GetOrdersByUserIdAsync(request.UserId, request.Filter, cancellationToken);

        if (totalCount == 0)
        {
            return Result.Success(new PagingResult<OrderDto>(Array.Empty<OrderDto>(), 0, request.Filter.PageNumber, request.Filter.PageSize));
        }

        var orderDtos = orders
            .Select(order => order.ToDto())
            .ToList();

        var pagingResult = new PagingResult<OrderDto>(orderDtos, totalCount, request.Filter.PageNumber, request.Filter.PageSize);

        return Result.Success(pagingResult);
    }
}




