using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record LeaveBuyerFeedbackCommand(
    Guid OrderId,
    Guid SellerId,
    bool UseStoredFeedback,
    string Comment,
    string? StoredFeedbackKey
) : ICommand<LeaveBuyerFeedbackResult>;

public sealed class LeaveBuyerFeedbackCommandValidator : AbstractValidator<LeaveBuyerFeedbackCommand>
{
    public LeaveBuyerFeedbackCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is required.");

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller id is required.");

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Feedback comment is required.")
            .MaximumLength(Domain.Orders.Entities.BuyerFeedback.MaxCommentLength)
            .WithMessage($"Feedback comment cannot exceed {Domain.Orders.Entities.BuyerFeedback.MaxCommentLength} characters.");

        When(x => x.UseStoredFeedback, () =>
        {
            RuleFor(x => x.StoredFeedbackKey)
                .NotEmpty().WithMessage("A stored feedback selection is required when using stored feedback.");
        });
    }
}

public sealed class LeaveBuyerFeedbackCommandHandler : ICommandHandler<LeaveBuyerFeedbackCommand, LeaveBuyerFeedbackResult>
{
    private static readonly string[] EligibleStatuses =
    {
        OrderStatusCodes.PaidAndShipped,
        OrderStatusCodes.PaidAwaitingFeedback,
        OrderStatusCodes.ShippedAwaitingFeedback
    };

    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LeaveBuyerFeedbackCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LeaveBuyerFeedbackResult>> Handle(LeaveBuyerFeedbackCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(
                Error.Failure("Order.NotFound", "Order not found."));
        }

        if (order.SellerId != request.SellerId)
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(
                Error.Failure("Order.AccessDenied", "You do not have permission to update this order."));
        }

        if (!EligibleStatuses.Contains(order.Status.Code, StringComparer.OrdinalIgnoreCase))
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(
                Error.Failure("Order.InvalidStatus", "Feedback can only be left after the order has been shipped."));
        }

        if (!order.HasShipmentsForAllItems())
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(
                Error.Failure("Order.ShipmentIncomplete", "Add tracking for all items before leaving feedback."));
        }

        var ninetyDaysAgo = DateTime.UtcNow.AddDays(-90);
        if (order.OrderedAt < ninetyDaysAgo)
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(
                Error.Failure("Order.FeedbackExpired", "Feedback can only be left within 90 days of the transaction."));
        }

        var leaveResult = order.RecordSellerFeedback(
            request.Comment,
            request.UseStoredFeedback,
            request.StoredFeedbackKey,
            DateTimeOffset.UtcNow);

        if (leaveResult.IsFailure)
        {
            return Result.Failure<LeaveBuyerFeedbackResult>(leaveResult.Error);
        }

        if (!string.Equals(order.Status.Code, OrderStatusCodes.ShippedAwaitingFeedback, StringComparison.OrdinalIgnoreCase))
        {
            var awaitingFeedbackStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.ShippedAwaitingFeedback, cancellationToken);
            if (awaitingFeedbackStatus is not null)
            {
                var statusResult = order.ChangeStatus(awaitingFeedbackStatus, OrderRoles.System);
                if (statusResult.IsFailure)
                {
                    return Result.Failure<LeaveBuyerFeedbackResult>(statusResult.Error);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var feedbackEntity = leaveResult.Value;
        var feedback = new SellerFeedbackDto(
            feedbackEntity.Id,
            feedbackEntity.OrderId,
            feedbackEntity.SellerId,
            feedbackEntity.BuyerId.Value,
            feedbackEntity.Comment,
            feedbackEntity.UsesStoredComment,
            feedbackEntity.StoredCommentKey,
            feedbackEntity.CreatedAt,
            feedbackEntity.FollowUpComment,
            feedbackEntity.FollowUpCommentedAt);
        var statusPayload = new OrderStatusUpdateResult(
            order.Id,
            order.Status.Code,
            order.Status.Name,
            order.Status.Color,
            order.ShippingStatus,
            order.PaidAt,
            order.ShippedAt,
            order.DeliveredAt);

        return Result.Success(new LeaveBuyerFeedbackResult(feedback, statusPayload));
    }
}
