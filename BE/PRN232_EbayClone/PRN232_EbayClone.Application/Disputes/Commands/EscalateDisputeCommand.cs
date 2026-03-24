using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record EscalateDisputeCommand(
    Guid DisputeId
) : ICommand;

public sealed class EscalateDisputeCommandValidator : AbstractValidator<EscalateDisputeCommand>
{
    public EscalateDisputeCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
    }
}

public sealed class EscalateDisputeCommandHandler : ICommandHandler<EscalateDisputeCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public EscalateDisputeCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(EscalateDisputeCommand request, CancellationToken cancellationToken)
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

        // Update status to Escalated when escalated to platform
        var statusResult = dispute.UpdateStatus(DisputeStatus.Escalated.ToString());
        if (statusResult.IsFailure)
        {
            return statusResult.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


