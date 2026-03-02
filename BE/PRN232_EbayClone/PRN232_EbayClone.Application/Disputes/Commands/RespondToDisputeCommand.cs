using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record RespondToDisputeCommand(
    Guid DisputeId,
    string Message
) : ICommand;

public sealed class RespondToDisputeCommandValidator : AbstractValidator<RespondToDisputeCommand>
{
    public RespondToDisputeCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Nội dung phản hồi là bắt buộc")
            .MaximumLength(2000).WithMessage("Nội dung phản hồi không được vượt quá 2000 ký tự");
    }
}

public sealed class RespondToDisputeCommandHandler : ICommandHandler<RespondToDisputeCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public RespondToDisputeCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RespondToDisputeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return DisputeErrors.Unauthorized;
        }

        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute is null)
        {
            return DisputeErrors.NotFound;
        }

        // Check if dispute is closed
        if (dispute.IsClosed)
        {
            return DisputeErrors.CannotUpdate;
        }

        // Update status to UnderReview when someone responds
        var statusResult = dispute.UpdateStatus(DisputeStatus.UnderReview.ToString());
        if (statusResult.IsFailure)
        {
            return statusResult.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


