using System;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using Quartz;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

public sealed class AwaitingShipmentStatusUpdateJob : IJob
{
    private const int HandlingTimeDays = 4;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AwaitingShipmentStatusUpdateJob> _logger;

    public AwaitingShipmentStatusUpdateJob(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<AwaitingShipmentStatusUpdateJob> logger)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var utcNow = DateTime.UtcNow;

        var orders = await _orderRepository.GetAwaitingShipmentOrdersAsync(context.CancellationToken);
        if (orders.Count == 0)
        {
            return;
        }

        var awaitingShipmentStatus = await _orderRepository.GetStatusByCodeAsync(
            OrderStatusCodes.AwaitingShipment,
            context.CancellationToken);
        var shipWithin24hStatus = await _orderRepository.GetStatusByCodeAsync(
            OrderStatusCodes.AwaitingShipmentShipWithin24h,
            context.CancellationToken);
        var overdueStatus = await _orderRepository.GetStatusByCodeAsync(
            OrderStatusCodes.AwaitingShipmentOverdue,
            context.CancellationToken);

        if (awaitingShipmentStatus is null || shipWithin24hStatus is null || overdueStatus is null)
        {
            _logger.LogWarning("Awaiting shipment status definitions are missing. Skipping status update job.");
            return;
        }

        var updatedOrders = 0;

        foreach (var order in orders)
        {
            var referenceDate = NormalizeReferenceDate(order);

            var handlingDeadline = referenceDate.AddDays(HandlingTimeDays);
            var shipWithinThreshold = handlingDeadline.AddDays(-1);

            var targetStatus = DetermineTargetStatus(
                order,
                utcNow,
                shipWithinThreshold,
                handlingDeadline,
                awaitingShipmentStatus,
                shipWithin24hStatus,
                overdueStatus);

            if (targetStatus is null || order.Status?.Id == targetStatus.Id)
            {
                continue;
            }

            var changeResult = order.ChangeStatus(targetStatus, OrderRoles.System);
            if (changeResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to update order {OrderId} to status {StatusCode}: {Error}",
                    order.Id,
                    targetStatus.Code,
                    changeResult.Error.Description);
                continue;
            }

            updatedOrders++;
        }

        if (updatedOrders == 0)
        {
            return;
        }

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);
        _logger.LogInformation(
            "Awaiting shipment status update job updated {OrderCount} order(s) at {ExecutedAtUtc:O}.",
            updatedOrders,
            utcNow);
    }

    private static DateTime NormalizeReferenceDate(Order order)
    {
        if (order.PaidAt.HasValue)
        {
            return NormalizeKind(order.PaidAt.Value);
        }

        return NormalizeKind(order.OrderedAt);
    }

    private static OrderStatus? DetermineTargetStatus(
        Order order,
        DateTime utcNow,
        DateTime shipWithinThreshold,
        DateTime handlingDeadline,
        OrderStatus awaitingShipmentStatus,
        OrderStatus shipWithin24hStatus,
        OrderStatus overdueStatus)
    {
        if (utcNow >= handlingDeadline)
        {
            return overdueStatus;
        }

        if (utcNow >= shipWithinThreshold)
        {
            return shipWithin24hStatus;
        }

        return awaitingShipmentStatus;
    }

    private static DateTime NormalizeKind(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => value
        };
    }
}
