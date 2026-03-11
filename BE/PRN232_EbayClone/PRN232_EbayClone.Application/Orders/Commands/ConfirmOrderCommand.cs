using System;
using System.Linq;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record ConfirmOrderCommand(Guid OrderId, Guid SellerId)
    : ICommand<OrderStatusUpdateResult>;

public sealed class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
{
    public ConfirmOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller id is required.");
    }
}

public sealed class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand, OrderStatusUpdateResult>
{
    private static readonly string[] AllowedStatuses =
    {
        OrderStatusCodes.AwaitingShipment,
        OrderStatusCodes.AwaitingShipmentOverdue,
        OrderStatusCodes.AwaitingShipmentShipWithin24h,
        OrderStatusCodes.AwaitingExpeditedShipment
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderStatusUpdateResult>> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
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

        var currentStatusCode = order.Status.Code;
        if (!AllowedStatuses.Contains(currentStatusCode, StringComparer.OrdinalIgnoreCase))
        {
            return Result.Failure<OrderStatusUpdateResult>(
                Error.Failure("Order.InvalidStatus", "Order cannot be confirmed in its current status."));
        }

        var targetStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.PaidAndShipped, cancellationToken);
        if (targetStatus is null)
        {
            return Result.Failure<OrderStatusUpdateResult>(
                Error.Failure("OrderStatus.NotFound", "Unable to resolve the target status."));
        }

        var changeResult = order.ChangeStatus(targetStatus, OrderRoles.Seller);
        if (changeResult.IsFailure)
        {
            return Result.Failure<OrderStatusUpdateResult>(changeResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var payload = new OrderStatusUpdateResult(
            order.Id,
            order.Status.Code,
            order.Status.Name,
            order.Status.Color,
            order.ShippingStatus,
            order.PaidAt,
            order.ShippedAt,
            order.DeliveredAt);

        return Result.Success(payload);
    }
}
