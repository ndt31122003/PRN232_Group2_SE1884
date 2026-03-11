using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record BulkLeaveBuyerFeedbackCommand(
    Guid SellerId,
    IReadOnlyList<Guid> OrderIds,
    bool UseStoredFeedback,
    string Comment,
    string? StoredFeedbackKey
) : ICommand<BulkLeaveBuyerFeedbackResult>;

public sealed class BulkLeaveBuyerFeedbackCommandValidator : AbstractValidator<BulkLeaveBuyerFeedbackCommand>
{
    public BulkLeaveBuyerFeedbackCommandValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty();

        RuleFor(x => x.OrderIds)
            .NotNull().WithMessage("Select at least one order.")
            .Must(ids => ids.Any()).WithMessage("Select at least one order.");

        RuleForEach(x => x.OrderIds)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Feedback comment is required.")
            .MaximumLength(BuyerFeedback.MaxCommentLength)
            .WithMessage($"Feedback comment cannot exceed {BuyerFeedback.MaxCommentLength} characters.");

        When(x => x.UseStoredFeedback, () =>
        {
            RuleFor(x => x.StoredFeedbackKey)
                .NotEmpty().WithMessage("Stored feedback key is required when using stored feedback.");
        });
    }
}

public sealed class BulkLeaveBuyerFeedbackCommandHandler : ICommandHandler<BulkLeaveBuyerFeedbackCommand, BulkLeaveBuyerFeedbackResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _sender;

    public BulkLeaveBuyerFeedbackCommandHandler(IOrderRepository orderRepository, ISender sender)
    {
        _orderRepository = orderRepository;
        _sender = sender;
    }

    public async Task<Result<BulkLeaveBuyerFeedbackResult>> Handle(BulkLeaveBuyerFeedbackCommand request, CancellationToken cancellationToken)
    {
        var normalizedOrderIds = request.OrderIds?.Where(id => id != Guid.Empty).ToList() ?? new List<Guid>();
        if (normalizedOrderIds.Count == 0)
        {
            return Result.Failure<BulkLeaveBuyerFeedbackResult>(
                Error.Validation("BulkFeedback.NoOrders", "Select at least one order."));
        }

        var uniqueOrderIds = normalizedOrderIds.Distinct().ToList();
        var orders = await _orderRepository.GetByIdsAsync(uniqueOrderIds, cancellationToken);
        var orderLookup = orders.ToDictionary(order => order.Id);

        var items = new List<BulkLeaveBuyerFeedbackItemResult>(normalizedOrderIds.Count);
        var seenIds = new HashSet<Guid>();
        var successCount = 0;

        foreach (var orderId in normalizedOrderIds)
        {
            if (!seenIds.Add(orderId))
            {
                items.Add(new BulkLeaveBuyerFeedbackItemResult(
                    orderId,
                    false,
                    null,
                    "BulkFeedback.DuplicateOrder",
                    "Duplicate order detected in the request."));
                continue;
            }

            if (!orderLookup.TryGetValue(orderId, out var order))
            {
                items.Add(new BulkLeaveBuyerFeedbackItemResult(
                    orderId,
                    false,
                    null,
                    "Order.NotFound",
                    "Order not found or unavailable."));
                continue;
            }

            if (order.SellerId != request.SellerId)
            {
                items.Add(new BulkLeaveBuyerFeedbackItemResult(
                    orderId,
                    false,
                    null,
                    "Order.AccessDenied",
                    "You do not have permission to update this order."));
                continue;
            }

            var result = await _sender.Send(new LeaveBuyerFeedbackCommand(
                orderId,
                request.SellerId,
                request.UseStoredFeedback,
                request.Comment,
                request.StoredFeedbackKey), cancellationToken);

            if (result.IsFailure)
            {
                items.Add(new BulkLeaveBuyerFeedbackItemResult(
                    orderId,
                    false,
                    null,
                    result.Error.Code,
                    result.Error.Description));
                continue;
            }

            successCount++;
            items.Add(new BulkLeaveBuyerFeedbackItemResult(
                orderId,
                true,
                result.Value,
                null,
                null));
        }

        var failureCount = items.Count - successCount;
        var payload = new BulkLeaveBuyerFeedbackResult(successCount, failureCount, items);
        return Result.Success(payload);
    }
}
