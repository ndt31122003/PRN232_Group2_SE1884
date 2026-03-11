using System;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record ApproveCancellationRequestCommand(
    Guid CancellationRequestId,
    Guid SellerId,
    string? SellerNote) : ICommand;

public sealed class ApproveCancellationRequestCommandValidator : AbstractValidator<ApproveCancellationRequestCommand>
{
    public ApproveCancellationRequestCommandValidator()
    {
        RuleFor(x => x.CancellationRequestId).NotEmpty();
        RuleFor(x => x.SellerId).NotEmpty();
    }
}

public sealed class ApproveCancellationRequestCommandHandler : ICommandHandler<ApproveCancellationRequestCommand>
{
    private readonly ICancellationRequestRepository _cancellationRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveCancellationRequestCommandHandler(
        ICancellationRequestRepository cancellationRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _cancellationRequestRepository = cancellationRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ApproveCancellationRequestCommand request,
        CancellationToken cancellationToken)
    {
        var cancellationRequest = await _cancellationRequestRepository.GetByIdAsync(
            request.CancellationRequestId,
            cancellationToken);

        if (cancellationRequest is null)
        {
            return Error.Failure("CancellationRequest.NotFound", "Cancellation request not found.");
        }

        if (cancellationRequest.SellerId != request.SellerId)
        {
            return Error.Failure("CancellationRequest.AccessDenied", "You cannot modify this cancellation request.");
        }

        if (cancellationRequest.Status is CancellationStatus.Completed or CancellationStatus.Declined or CancellationStatus.AutoCancelled)
        {
            return Error.Failure("CancellationRequest.AlreadyClosed", "Cancellation request has already been resolved.");
        }

        var refundAmount = cancellationRequest.OrderTotalSnapshot;
        var approveResult = cancellationRequest.Approve(
            refundAmount,
            DateTime.UtcNow,
            request.SellerNote);

        if (approveResult.IsFailure)
        {
            return approveResult.Error;
        }

        _cancellationRequestRepository.Update(cancellationRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
