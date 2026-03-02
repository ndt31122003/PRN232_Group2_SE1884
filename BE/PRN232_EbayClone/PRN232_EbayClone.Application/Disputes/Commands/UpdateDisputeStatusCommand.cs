using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record UpdateDisputeStatusCommand(
    Guid DisputeId,
    string Status
) : ICommand;

public sealed class UpdateDisputeStatusCommandValidator : AbstractValidator<UpdateDisputeStatusCommand>
{
    public UpdateDisputeStatusCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Trạng thái không được để trống")
            .Must(s => System.Enum.TryParse<Domain.Disputes.Enums.DisputeStatus>(s, ignoreCase: true, out _))
            .WithMessage("Trạng thái không hợp lệ");
    }
}

public sealed class UpdateDisputeStatusCommandHandler : ICommandHandler<UpdateDisputeStatusCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDisputeStatusCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateDisputeStatusCommand request, CancellationToken cancellationToken)
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

        var result = dispute.UpdateStatus(request.Status);
        if (result.IsFailure)
        {
            return result.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
