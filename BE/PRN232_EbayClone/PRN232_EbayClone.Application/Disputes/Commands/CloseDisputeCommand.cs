using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record CloseDisputeCommand(
    Guid DisputeId
) : ICommand;

public sealed class CloseDisputeCommandValidator : AbstractValidator<CloseDisputeCommand>
{
    public CloseDisputeCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
    }
}

public sealed class CloseDisputeCommandHandler : ICommandHandler<CloseDisputeCommand>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRealtimeNotifier _realtimeNotifier;

    public CloseDisputeCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        IRealtimeNotifier realtimeNotifier)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _realtimeNotifier = realtimeNotifier;
    }

    public async Task<Result> Handle(CloseDisputeCommand request, CancellationToken cancellationToken)
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

        var result = dispute.UpdateStatus(DisputeStatus.Closed.ToString());
        if (result.IsFailure)
        {
            return result.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Send realtime notification to both parties
        var listing = dispute.Listing;
        var sellerId = listing?.CreatedBy;
        var buyerId = dispute.RaisedById;
        
        var userIds = new List<string> { buyerId };
        if (!string.IsNullOrEmpty(sellerId))
        {
            userIds.Add(sellerId);
        }

        await _realtimeNotifier.SendToUsersAsync(
            userIds,
            "DisputeStatusChanged",
            new
            {
                DisputeId = dispute.Id,
                Status = dispute.Status,
                Message = "Dispute has been closed.",
                Timestamp = DateTime.UtcNow
            },
            cancellationToken);

        return Result.Success();
    }
}
