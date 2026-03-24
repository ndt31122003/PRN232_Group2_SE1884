using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Disputes.Entities;
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
    private readonly IRealtimeNotifier _realtimeNotifier;

    public RespondToDisputeCommandHandler(
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

    public async Task<Result> Handle(RespondToDisputeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return DisputeErrors.Unauthorized;
        }

        if (!Guid.TryParse(_userContext.UserId, out var responderId))
        {
            return Error.Validation("RespondToDispute.InvalidUserId", "User ID không hợp lệ");
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

        // Create and add response
        var response = DisputeResponse.Create(
            request.DisputeId,
            responderId,
            request.Message);

        dispute.AddResponse(response);

        // Update status to WaitingSeller when someone responds
        var statusResult = dispute.UpdateStatus(DisputeStatus.WaitingSeller.ToString());
        if (statusResult.IsFailure)
        {
            return statusResult.Error;
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
            "DisputeNewResponse",
            new
            {
                DisputeId = dispute.Id,
                ResponseId = response.Id,
                ResponderId = responderId,
                Message = request.Message,
                Timestamp = response.CreatedAt
            },
            cancellationToken);

        return Result.Success();
    }
}


