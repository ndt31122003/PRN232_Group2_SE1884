using System;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record CompleteReturnRefundCommand(
    Guid ReturnRequestId,
    Guid SellerId,
    decimal? RefundAmount,
    string? SellerNote) : ICommand;

public sealed class CompleteReturnRefundCommandValidator : AbstractValidator<CompleteReturnRefundCommand>
{
    public CompleteReturnRefundCommandValidator()
    {
        RuleFor(x => x.ReturnRequestId).NotEmpty();
        RuleFor(x => x.SellerId).NotEmpty();
        RuleFor(x => x.RefundAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.RefundAmount.HasValue);
    }
}

public sealed class CompleteReturnRefundCommandHandler : ICommandHandler<CompleteReturnRefundCommand>
{
    private readonly IReturnRequestRepository _returnRequestRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteReturnRefundCommandHandler(
        IReturnRequestRepository returnRequestRepository,
        IOrderRepository orderRepository,
        IInventoryRepository inventoryRepository,
        IUnitOfWork unitOfWork)
    {
        _returnRequestRepository = returnRequestRepository;
        _orderRepository = orderRepository;
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CompleteReturnRefundCommand request, CancellationToken cancellationToken)
    {
        var returnRequest = await _returnRequestRepository.GetByIdAsync(request.ReturnRequestId, cancellationToken);
        if (returnRequest is null)
        {
            return Error.Failure("ReturnRequest.NotFound", "Return request not found.");
        }

        if (returnRequest.SellerId != request.SellerId)
        {
            return Error.Failure("ReturnRequest.AccessDenied", "You cannot modify this return request.");
        }

        if (returnRequest.Status is not ReturnStatus.DeliveredToSeller and not ReturnStatus.RefundPending)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return request is not ready for refund completion.");
        }

        var order = await _orderRepository.GetByIdAsync(returnRequest.OrderId, cancellationToken);
        if (order is null)
        {
            return Error.Failure("Order.NotFound", "Order not found.");
        }

        var targetRefundAmount = request.RefundAmount ?? ResolveRefundAmount(returnRequest);
        var moneyResult = Money.Create(targetRefundAmount, returnRequest.OrderTotalSnapshot.Currency);
        if (moneyResult.IsFailure)
        {
            return moneyResult.Error;
        }

        var refundResult = returnRequest.IssueRefund(moneyResult.Value, DateTime.UtcNow, request.SellerNote);
        if (refundResult.IsFailure)
        {
            return refundResult.Error;
        }

        foreach (var item in order.Items)
        {
            var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(item.ListingId), cancellationToken);
            if (inventory is null || inventory.SoldQuantity < item.Quantity)
            {
                continue;
            }

            var restockResult = inventory.RestockFromReturn(item.Quantity);
            if (restockResult.IsFailure)
            {
                return restockResult.Error;
            }

            _inventoryRepository.Update(inventory);
        }

        _returnRequestRepository.Update(returnRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private static decimal ResolveRefundAmount(Domain.Orders.Entities.ReturnRequest returnRequest)
    {
        var restockingFee = returnRequest.RestockingFee?.Amount ?? 0m;
        var refundAmount = returnRequest.OrderTotalSnapshot.Amount - restockingFee;
        return refundAmount < 0m ? 0m : refundAmount;
    }
}