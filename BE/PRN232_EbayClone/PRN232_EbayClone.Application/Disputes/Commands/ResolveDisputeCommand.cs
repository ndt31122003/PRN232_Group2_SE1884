using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record ResolveDisputeCommand(
    Guid DisputeId
) : ICommand;

public sealed class ResolveDisputeCommandValidator : AbstractValidator<ResolveDisputeCommand>
{
    public ResolveDisputeCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
    }
}

public sealed class ResolveDisputeCommandHandler : ICommandHandler<ResolveDisputeCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ResolveDisputeCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ResolveDisputeCommand request, CancellationToken cancellationToken)
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

        var result = dispute.UpdateStatus(DisputeStatus.Resolved.ToString());
        if (result.IsFailure)
        {
            return result.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
