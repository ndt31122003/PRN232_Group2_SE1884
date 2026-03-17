using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record AcceptRefundCommand(
    Guid DisputeId,
    decimal RefundAmount,
    string? Message
) : ICommand;

public sealed class AcceptRefundCommandValidator : AbstractValidator<AcceptRefundCommand>
{
    public AcceptRefundCommandValidator()
    {
        RuleFor(x => x.DisputeId)
            .NotEmpty()
            .WithMessage("Dispute ID là bắt buộc");
        
        RuleFor(x => x.RefundAmount)
            .GreaterThan(0)
            .WithMessage("Số tiền hoàn lại phải lớn hơn 0");
        
        RuleFor(x => x.Message)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.Message))
            .WithMessage("Tin nhắn không được vượt quá 1000 ký tự");
    }
}

public sealed class AcceptRefundCommandHandler : ICommandHandler<AcceptRefundCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptRefundCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AcceptRefundCommand request, CancellationToken cancellationToken)
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

        // Verify seller ownership
        if (dispute.SellerId.ToString() != _userContext.UserId)
        {
            return DisputeErrors.Unauthorized;
        }

        // Accept refund and update status to Resolved
        var result = dispute.AcceptRefund(
            request.RefundAmount,
            "USD", // Default currency, should be from order
            _userContext.UserId);

        if (result.IsFailure)
        {
            return result.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Trigger refund processing (integration with payment system)
        // TODO: Raise DisputeStatusChangedDomainEvent for notifications

        return Result.Success();
    }
}
