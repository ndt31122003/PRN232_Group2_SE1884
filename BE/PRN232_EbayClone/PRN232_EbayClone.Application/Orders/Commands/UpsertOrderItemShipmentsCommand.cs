using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record UpsertOrderItemShipmentsCommand(
    Guid OrderId,
    Guid SellerId,
    IReadOnlyCollection<OrderItemShipmentDraft> Shipments,
    IReadOnlyCollection<Guid> ClearedOrderItemIds) : ICommand<IReadOnlyCollection<OrderShipmentDto>>;

public sealed class UpsertOrderItemShipmentsCommandValidator : AbstractValidator<UpsertOrderItemShipmentsCommand>
{
    public UpsertOrderItemShipmentsCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller id is required.");

        RuleForEach(x => x.Shipments)
            .ChildRules(shipment =>
            {
                shipment.RuleFor(s => s.OrderItemId)
                    .NotEmpty().WithMessage("Order item id is required.");

                shipment.RuleFor(s => s.TrackingNumber)
                    .NotEmpty().WithMessage("Tracking number is required.");

                shipment.RuleFor(s => s.Carrier)
                    .NotEmpty().WithMessage("Carrier is required.");
            });
    }
}

public sealed class UpsertOrderItemShipmentsCommandHandler : ICommandHandler<UpsertOrderItemShipmentsCommand, IReadOnlyCollection<OrderShipmentDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpsertOrderItemShipmentsCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyCollection<OrderShipmentDto>>> Handle(UpsertOrderItemShipmentsCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(
                Error.Failure("Order.NotFound", "Order not found."));
        }

        if (order.SellerId != request.SellerId)
        {
            return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(
                Error.Failure("Order.AccessDenied", "You do not have permission to modify this order."));
        }

        var clearedIds = request.ClearedOrderItemIds?.Distinct().ToHashSet() ?? new HashSet<Guid>();

        foreach (var orderItemId in clearedIds)
        {
            var clearResult = order.ClearShipmentsForItem(orderItemId);
            if (clearResult.IsFailure)
            {
                return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(clearResult.Error);
            }
        }

        var draftsByItem = (request.Shipments ?? Array.Empty<OrderItemShipmentDraft>())
            .GroupBy(s => s.OrderItemId);

        foreach (var group in draftsByItem)
        {
            if (clearedIds.Contains(group.Key))
            {
                // Already cleared; treat remaining drafts as fresh additions below.
                clearedIds.Remove(group.Key);
            }

            var drafts = group.ToList();
            var existingShipments = order.ItemShipments
                .Where(s => s.OrderItemId == group.Key)
                .ToDictionary(s => s.Id);

            foreach (var draft in drafts)
            {
                var shippedAt = draft.ShippedAt ?? DateTimeOffset.UtcNow;

                if (draft.ShipmentId.HasValue)
                {
                    if (!existingShipments.TryGetValue(draft.ShipmentId.Value, out _))
                    {
                        return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(
                            Error.Failure("Order.Shipment.NotFound", "Shipment to update was not found."));
                    }

                    var updateResult = order.UpdateShipment(
                        draft.ShipmentId.Value,
                        draft.Carrier,
                        draft.TrackingNumber,
                        shippedAt,
                        draft.ShippingLabelId);

                    if (updateResult.IsFailure)
                    {
                        return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(updateResult.Error);
                    }

                    continue;
                }

                var addResult = order.AddShipment(
                    draft.OrderItemId,
                    draft.Carrier,
                    draft.TrackingNumber,
                    shippedAt,
                    draft.ShippingLabelId);

                if (addResult.IsFailure)
                {
                    return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(addResult.Error);
                }
            }
        }

        if (order.HasShipmentsForAllItems())
        {
            var paidAndShippedStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.PaidAndShipped, cancellationToken);
            if (paidAndShippedStatus is not null)
            {
                var statusResult = order.ChangeStatus(paidAndShippedStatus, OrderRoles.Seller);
                if (statusResult.IsFailure)
                {
                    return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(statusResult.Error);
                }
            }
        }
        else
        {
            var awaitingShipmentStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.AwaitingShipment, cancellationToken);
            if (awaitingShipmentStatus is not null)
            {
                var statusResult = order.ChangeStatus(awaitingShipmentStatus, OrderRoles.Seller);
                if (statusResult.IsFailure)
                {
                    return Result.Failure<IReadOnlyCollection<OrderShipmentDto>>(statusResult.Error);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var payload = order.ItemShipments
            .OrderBy(s => s.ShippedAt)
            .Select(s => new OrderShipmentDto(
                s.Id,
                s.OrderItemId,
                s.TrackingNumber,
                s.Carrier,
                s.ShippedAt,
                s.ShippingLabelId,
                s.CreatedAt,
                s.UpdatedAt))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyCollection<OrderShipmentDto>>(payload);
    }
}