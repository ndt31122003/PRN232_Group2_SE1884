using System;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Orders.Enums;
using Quartz;

namespace PRN232_EbayClone.Infrastructure.BackgroundJobs;

public sealed class BuyerCancellationTimeoutJob : IJob
{
    private static readonly TimeSpan SellerResponseWindow = TimeSpan.FromDays(3);

    private readonly ICancellationRequestRepository _cancellationRequestRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BuyerCancellationTimeoutJob> _logger;

    public BuyerCancellationTimeoutJob(
        ICancellationRequestRepository cancellationRequestRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        ILogger<BuyerCancellationTimeoutJob> logger)
    {
        _cancellationRequestRepository = cancellationRequestRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var utcNow = DateTime.UtcNow;

        var openRequests = await _cancellationRequestRepository.GetOpenBuyerInitiatedAsync(context.CancellationToken);
        if (openRequests.Count == 0)
        {
            return;
        }

        OrderStatus? cancelledStatus = null;
        var autoCancelledCount = 0;
        var autoDeclinedCount = 0;

        foreach (var request in openRequests)
        {
            if (request.Order is null)
            {
                continue;
            }

            var deadlineUtc = ResolveDeadline(request);
            if (deadlineUtc > utcNow)
            {
                continue;
            }

            if (request.Order.PaidAt.HasValue)
            {
                var rejectResult = request.Reject(
                    utcNow,
                    "Seller did not respond within the 3 day window. Order must be fulfilled.");

                if (rejectResult.IsFailure)
                {
                    _logger.LogWarning(
                        "Failed to auto-decline cancellation request {CancellationRequestId}: {Error}",
                        request.Id,
                        rejectResult.Error.Description);
                    continue;
                }

                autoDeclinedCount++;
                continue;
            }

            var autoCancelResult = request.MarkAutoCancelled(
                utcNow,
                "Buyer cancellation auto-approved after seller did not respond for 3 days.");

            if (autoCancelResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to auto-cancel request {CancellationRequestId}: {Error}",
                    request.Id,
                    autoCancelResult.Error.Description);
                continue;
            }

            cancelledStatus ??= await _orderRepository.GetStatusByCodeAsync(
                OrderStatusCodes.Cancelled,
                context.CancellationToken);

            if (cancelledStatus is null)
            {
                _logger.LogWarning(
                    "Cancelled status not found while auto-cancelling request {CancellationRequestId}",
                    request.Id);
                continue;
            }

            var changeStatusResult = request.Order.ChangeStatus(cancelledStatus, OrderRoles.System);
            if (changeStatusResult.IsFailure)
            {
                _logger.LogWarning(
                    "Failed to move order {OrderId} to Cancelled for timed out cancellation request {CancellationRequestId}: {Error}",
                    request.Order.Id,
                    request.Id,
                    changeStatusResult.Error.Description);
                continue;
            }

            autoCancelledCount++;
        }

        if (autoCancelledCount == 0 && autoDeclinedCount == 0)
        {
            return;
        }

        await _unitOfWork.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation(
            "Processed buyer cancellation timeout job at {ExecutedAtUtc:O}. Auto-cancelled: {AutoCancelledCount}, Auto-declined: {AutoDeclinedCount}.",
            utcNow,
            autoCancelledCount,
            autoDeclinedCount);
    }

    private static DateTime ResolveDeadline(CancellationRequest request)
    {
        if (request.SellerResponseDeadlineUtc.HasValue)
        {
            return NormalizeUtc(request.SellerResponseDeadlineUtc.Value);
        }

        var requestedAt = NormalizeUtc(request.RequestedAt);
        return requestedAt.Add(SellerResponseWindow);
    }

    private static DateTime NormalizeUtc(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(value, DateTimeKind.Utc),
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => value
        };
    }
}
