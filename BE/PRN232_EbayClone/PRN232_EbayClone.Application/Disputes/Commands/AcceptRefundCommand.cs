using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record AcceptRefundCommand(
    Guid DisputeId,
    string SellerId
) : ICommand;

internal sealed class AcceptRefundCommandHandler(
    IDisputeRepository disputeRepository,
    IListingRepository listingRepository,
    IUnitOfWork unitOfWork,
    IDisputeStateMachine stateMachine,
    IRealtimeNotifier realtimeNotifier)
    : ICommandHandler<AcceptRefundCommand>
{
    public async Task<Result> Handle(AcceptRefundCommand request, CancellationToken cancellationToken)
    {
        var dispute = await disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute == null)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute");
        }

        // Verify seller owns the listing
        var listing = await listingRepository.GetByIdAsync(dispute.ListingId, cancellationToken);
        if (listing == null || listing.CreatedBy != request.SellerId)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute hoặc bạn không có quyền truy cập");
        }

        var currentStatus = Enum.Parse<DisputeStatus>(dispute.Status, ignoreCase: true);
        var newStatusResult = stateMachine.AcceptRefund(currentStatus);

        if (newStatusResult.IsFailure)
        {
            return newStatusResult.Error;
        }

        var updateResult = dispute.UpdateStatus(newStatusResult.Value.ToString());
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        disputeRepository.Update(dispute);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Send realtime notification to both seller and buyer
        var userIds = new[] { request.SellerId, dispute.RaisedById };
        await realtimeNotifier.SendToUsersAsync(
            userIds,
            "DisputeStatusChanged",
            new
            {
                DisputeId = dispute.Id,
                Status = dispute.Status,
                Message = "Seller has accepted the refund request. Dispute is now resolved.",
                Timestamp = DateTime.UtcNow
            },
            cancellationToken);

        return Result.Success();
    }
}