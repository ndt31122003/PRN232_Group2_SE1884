using System;
using FluentValidation;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record RejectReturnRequestCommand(
    Guid ReturnRequestId,
    Guid SellerId,
    string? SellerNote) : ICommand;

public sealed class RejectReturnRequestCommandValidator : AbstractValidator<RejectReturnRequestCommand>
{
    public RejectReturnRequestCommandValidator()
    {
        RuleFor(x => x.ReturnRequestId).NotEmpty();
        RuleFor(x => x.SellerId).NotEmpty();
    }
}

public sealed class RejectReturnRequestCommandHandler : ICommandHandler<RejectReturnRequestCommand>
{
    private readonly IReturnRequestRepository _returnRequestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectReturnRequestCommandHandler(
        IReturnRequestRepository returnRequestRepository,
        IUnitOfWork unitOfWork)
    {
        _returnRequestRepository = returnRequestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        RejectReturnRequestCommand request,
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
            return Error.Failure("ReturnRequest.InvalidStatus", "Return request is not awaiting your response.");
        }

        var rejectResult = returnRequest.Reject(DateTime.UtcNow, request.SellerNote);
        if (rejectResult.IsFailure)
        {
            return rejectResult.Error;
        }

        _returnRequestRepository.Update(returnRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
