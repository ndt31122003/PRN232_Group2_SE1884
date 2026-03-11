using System;
using System.Linq;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public enum DeliveryStatusUpdate
{
    Delivered,
    Failed
}

public sealed record UpdateOrderDeliveryStatusCommand(
    Guid OrderId,
    Guid SellerId,
    DeliveryStatusUpdate Outcome,
    string? Note
) : ICommand<OrderStatusUpdateResult>;

public sealed class UpdateOrderDeliveryStatusCommandValidator : AbstractValidator<UpdateOrderDeliveryStatusCommand>
{
    public UpdateOrderDeliveryStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller id is required.");

        RuleFor(x => x.Outcome)
            .IsInEnum().WithMessage("Outcome is invalid.");
    }
}

public sealed class UpdateOrderDeliveryStatusCommandHandler : ICommandHandler<UpdateOrderDeliveryStatusCommand, OrderStatusUpdateResult>
{
    private static readonly string[] ShippedStatuses =
    {
        OrderStatusCodes.PaidAndShipped,
        OrderStatusCodes.ShippedAwaitingFeedback,
        OrderStatusCodes.DeliveryFailed
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderDeliveryStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<OrderStatusUpdateResult>> Handle(UpdateOrderDeliveryStatusCommand request, CancellationToken cancellationToken)
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

        if (!ShippedStatuses.Contains(order.Status.Code, StringComparer.OrdinalIgnoreCase))
        {
            return Result.Failure<OrderStatusUpdateResult>(
                Error.Failure("Order.InvalidStatus", "Delivery status can only be updated after shipment."));
        }

        Result outcomeResult = request.Outcome switch
        {
            DeliveryStatusUpdate.Delivered => await MarkDeliveredAsync(order, cancellationToken),
            DeliveryStatusUpdate.Failed => await MarkDeliveryFailedAsync(order, cancellationToken),
            _ => Result.Failure(Error.Failure("Order.InvalidOutcome", "Unknown delivery outcome."))
        };

        if (outcomeResult.IsFailure)
        {
            return Result.Failure<OrderStatusUpdateResult>(outcomeResult.Error);
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

    private async Task<Result> MarkDeliveredAsync(Domain.Orders.Entities.Order order, CancellationToken cancellationToken)
    {
        if (!string.Equals(order.Status.Code, OrderStatusCodes.PaidAndShipped, StringComparison.OrdinalIgnoreCase))
        {
            var deliveredStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.PaidAndShipped, cancellationToken)
                ?? throw new InvalidOperationException("Delivered status was not seeded.");

            var changeResult = order.ChangeStatus(deliveredStatus, OrderRoles.Seller);
            if (changeResult.IsFailure)
            {
                return changeResult;
            }
        }

        return Result.Success();
    }

    private async Task<Result> MarkDeliveryFailedAsync(Domain.Orders.Entities.Order order, CancellationToken cancellationToken)
    {
        var targetStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.DeliveryFailed, cancellationToken)
            ?? throw new InvalidOperationException("DeliveryFailed status was not seeded.");

        var changeResult = order.ChangeStatus(targetStatus, OrderRoles.Seller);
        if (changeResult.IsFailure)
        {
            return changeResult;
        }

        return Result.Success();
    }
}
