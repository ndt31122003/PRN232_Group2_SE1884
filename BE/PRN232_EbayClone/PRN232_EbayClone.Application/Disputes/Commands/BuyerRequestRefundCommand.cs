using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record BuyerRequestRefundCommand(Guid DisputeId, string BuyerId, string? Message) : ICommand;

internal sealed class BuyerRequestRefundCommandHandler(
    IDisputeRepository disputeRepository,
    IUnitOfWork unitOfWork,
    IRealtimeNotifier realtimeNotifier) : ICommandHandler<BuyerRequestRefundCommand>
{
    public async Task<Result> Handle(BuyerRequestRefundCommand request, CancellationToken cancellationToken)
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

        // Can only request refund from Open status
        if (dispute.Status != DisputeStatus.Open.ToString())
        {
            return Error.Validation("Dispute.InvalidStatus", 
                $"Không thể yêu cầu hoàn tiền khi dispute đang ở trạng thái {dispute.Status}");
        }

        // Change status to WaitingSeller
        var updateResult = dispute.UpdateStatus(DisputeStatus.WaitingSeller.ToString());
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        // Add response if message provided
        if (!string.IsNullOrWhiteSpace(request.Message))
        {
            var response = Domain.Disputes.Entities.DisputeResponse.Create(
                dispute.Id,
                Guid.Parse(request.BuyerId),
                request.Message
            );
            dispute.AddResponse(response);
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
                    Message = "Buyer has requested a refund. Please review and respond.",
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
                Message = "Refund request submitted. Waiting for seller response.",
                Timestamp = DateTime.UtcNow
            },
            cancellationToken);

        return Result.Success();
    }
}
