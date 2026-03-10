using System;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record CreateCancellationRequestCommand(
    Guid OrderId,
    Guid SellerId,
    CancellationReason Reason,
    string? SellerNote) : ICommand<Guid>;

public sealed class CreateCancellationRequestCommandValidator : AbstractValidator<CreateCancellationRequestCommand>
{
    public CreateCancellationRequestCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.SellerId).NotEmpty();
        RuleFor(x => x.Reason).IsInEnum();
    }
}

public sealed class CreateCancellationRequestCommandHandler
    : ICommandHandler<CreateCancellationRequestCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICancellationRequestRepository _cancellationRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCancellationRequestCommandHandler(
        IOrderRepository orderRepository,
        ICancellationRequestRepository cancellationRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _cancellationRequestRepository = cancellationRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateCancellationRequestCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order is null)
        {
            return Result.Failure<Guid>(Error.Failure("Order.NotFound", "Order not found."));
        }

        if (order.SellerId != request.SellerId)
        {
            return Result.Failure<Guid>(Error.Failure("Order.AccessDenied", "You do not have permission to modify this order."));
        }

        if (!CanSellerInitiateCancellation(order))
        {
            return Result.Failure<Guid>(Error.Failure(
                "CancellationRequest.NotAllowed",
                "Order can no longer be cancelled."));
        }

        if (order.ActiveCancellationRequest is not null)
        {
            return Result.Failure<Guid>(Error.Failure(
                "CancellationRequest.AlreadyExists",
                "This order already has an open cancellation request."));
        }

        var snapshot = order.Total;
        var createResult = CancellationRequest.Create(
            order.Id,
            order.BuyerId.Value,
            order.SellerId,
            CancellationInitiator.Seller,
            request.Reason,
            snapshot,
            buyerNote: null,
            sellerNote: request.SellerNote,
            requestedAtUtc: DateTime.UtcNow,
            sellerResponseDeadlineUtc: null);

        if (createResult.IsFailure)
        {
            return Result.Failure<Guid>(createResult.Error);
        }

        var cancellationRequest = createResult.Value;

        var attachResult = order.AddCancellationRequest(cancellationRequest);
        if (attachResult.IsFailure)
        {
            return Result.Failure<Guid>(attachResult.Error);
        }

        _cancellationRequestRepository.Add(cancellationRequest);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(cancellationRequest.Id);
    }

    private static bool CanSellerInitiateCancellation(Order order)
    {
        if (string.Equals(order.Status.Code, OrderStatusCodes.Cancelled, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return order.ShippingStatus == ShippingStatus.Pending
            || order.ShippingStatus == ShippingStatus.LabelCreated;
    }
}
