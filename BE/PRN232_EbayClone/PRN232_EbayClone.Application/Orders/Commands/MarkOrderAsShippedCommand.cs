using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record MarkOrderAsShippedCommand(Guid OrderId, Guid SellerId) : ICommand<OrderStatusUpdateResult>;

public sealed class MarkOrderAsShippedCommandHandler : ICommandHandler<MarkOrderAsShippedCommand, OrderStatusUpdateResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsShippedCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderStatusUpdateResult>> Handle(MarkOrderAsShippedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<OrderStatusUpdateResult>(
                Error.Failure("Order.NotFound", "Order not found."));
        }

        if (order.SellerId != request.SellerId)
        {
            return Result.Failure<OrderStatusUpdateResult>(
                Error.Failure("Order.AccessDenied", "You do not have permission to modify this order."));
        }

        // Already in a shipped state — idempotent
        if (string.Equals(order.Status.Code, OrderStatusCodes.PaidAndShipped, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Success(new OrderStatusUpdateResult(
                order.Id, order.Status.Code, order.Status.Name, order.Status.Color,
                order.ShippingStatus, order.PaidAt, order.ShippedAt, order.DeliveredAt));
        }

        var shippedStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.PaidAndShipped, cancellationToken)
            ?? throw new InvalidOperationException("PaidAndShipped status was not seeded.");

        var changeResult = order.ChangeStatus(shippedStatus, OrderRoles.Seller);
        if (changeResult.IsFailure)
        {
            return Result.Failure<OrderStatusUpdateResult>(changeResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new OrderStatusUpdateResult(
            order.Id, order.Status.Code, order.Status.Name, order.Status.Color,
            order.ShippingStatus, order.PaidAt, order.ShippedAt, order.DeliveredAt));
    }
}
