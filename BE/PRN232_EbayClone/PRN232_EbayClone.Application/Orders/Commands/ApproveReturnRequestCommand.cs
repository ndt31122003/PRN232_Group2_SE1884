using System;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record ApproveReturnRequestCommand(
    Guid ReturnRequestId,
    Guid SellerId,
    DateTime? BuyerReturnDueAtUtc,
    string? SellerNote) : ICommand;

public sealed class ApproveReturnRequestCommandValidator : AbstractValidator<ApproveReturnRequestCommand>
{
    public ApproveReturnRequestCommandValidator()
    {
        RuleFor(x => x.ReturnRequestId).NotEmpty();
        RuleFor(x => x.SellerId).NotEmpty();
    }
}

public sealed class ApproveReturnRequestCommandHandler : ICommandHandler<ApproveReturnRequestCommand>
{
    private readonly IReturnRequestRepository _returnRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveReturnRequestCommandHandler(
        IReturnRequestRepository returnRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _returnRequestRepository = returnRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ApproveReturnRequestCommand request,
        CancellationToken cancellationToken)
    {
        var returnRequest = await _returnRequestRepository.GetByIdAsync(
            request.ReturnRequestId,
            cancellationToken);

        if (returnRequest is null)
        {
            return Error.Failure("ReturnRequest.NotFound", "Return request not found.");
        }

        if (returnRequest.SellerId != request.SellerId)
        {
            return Error.Failure("ReturnRequest.AccessDenied", "You cannot modify this return request.");
        }

        if (returnRequest.Status is not ReturnStatus.PendingSellerResponse)
        {
            return Error.Failure("ReturnRequest.InvalidStatus", "Return request is not awaiting your approval.");
        }

        DateTime? normalizedDueAt = request.BuyerReturnDueAtUtc;
        if (normalizedDueAt.HasValue && normalizedDueAt.Value.Kind != DateTimeKind.Utc)
        {
            normalizedDueAt = DateTime.SpecifyKind(normalizedDueAt.Value, DateTimeKind.Utc);
        }

        var approveResult = returnRequest.Approve(
            DateTime.UtcNow,
            normalizedDueAt,
            request.SellerNote,
            restockingFee: null);

        if (approveResult.IsFailure)
        {
            return approveResult.Error;
        }

        _returnRequestRepository.Update(returnRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
