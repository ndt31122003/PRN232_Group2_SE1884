using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record BuyerAcceptEvidenceCommand(Guid DisputeId, string BuyerId) : ICommand;

internal sealed class BuyerAcceptEvidenceCommandHandler(
    IDisputeRepository disputeRepository,
    IUnitOfWork unitOfWork,
    IRealtimeNotifier realtimeNotifier) : ICommandHandler<BuyerAcceptEvidenceCommand>
{
    public async Task<Result> Handle(BuyerAcceptEvidenceCommand request, CancellationToken cancellationToken)
    {
        var dispute = await disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute is null)
        {
            return Error.NotFound("Dispute.NotFound", "Dispute không tồn tại");
        }

        // Verify buyer is the one who raised the dispute
        if (dispute.RaisedById != request.BuyerId)
        {
            return Error.Validation("Dispute.NotAuthorized", "Bạn không có quyền thực hiện hành động này");
        }

        // Check if dispute is in WaitingBuyer status
        if (dispute.Status != DisputeStatus.WaitingBuyer.ToString())
        {
            return Error.Validation("Dispute.InvalidStatus", 
                $"Không thể chấp nhận bằng chứng khi dispute đang ở trạng thái {dispute.Status}");
        }

        // Buyer accepts evidence -> Resolve dispute in favor of seller
        var updateResult = dispute.UpdateStatus(DisputeStatus.Resolved.ToString());
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        disputeRepository.Update(dispute);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Send realtime notification to seller
        if (dispute.Listing?.CreatedBy != null)
        {
            await realtimeNotifier.SendToUserAsync(
                dispute.Listing.CreatedBy,
                "DisputeStatusChanged",
                new
                {
                    DisputeId = dispute.Id,
                    Status = dispute.Status,
                    Message = "Buyer has accepted your evidence. Dispute resolved!",
                    Timestamp = DateTime.UtcNow
                },
                cancellationToken);
        }

        // Send notification to buyer
        await realtimeNotifier.SendToUserAsync(
            request.BuyerId,
            "DisputeStatusChanged",
            new
            {
                DisputeId = dispute.Id,
                Status = dispute.Status,
                Message = "You have accepted the seller's evidence. Dispute resolved.",
                Timestamp = DateTime.UtcNow
            },
            cancellationToken);

        return Result.Success();
    }
}
