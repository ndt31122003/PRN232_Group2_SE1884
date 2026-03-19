using Dapper;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record EscalateSellerDisputeCommand(
    Guid DisputeId,
    string SellerId
) : ICommand;

internal sealed class EscalateSellerDisputeCommandHandler(
    IDisputeRepository disputeRepository,
    IUnitOfWork unitOfWork,
    IDisputeStateMachine stateMachine,
    IDbConnectionFactory dbConnectionFactory)
    : ICommandHandler<EscalateSellerDisputeCommand>
{
    public async Task<Result> Handle(EscalateSellerDisputeCommand request, CancellationToken cancellationToken)
    {
        // Verify seller owns the listing
        using var connection = await dbConnectionFactory.OpenConnectionAsync();
        var ownershipSql = @"
            SELECT 1 
            FROM disputes d
            INNER JOIN listings l ON d.listing_id = l.id
            WHERE d.id = @DisputeId AND l.seller_id = @SellerId";

        var isOwner = await connection.QuerySingleOrDefaultAsync<int?>(ownershipSql, new
        {
            DisputeId = request.DisputeId,
            SellerId = request.SellerId
        });

        if (isOwner == null)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute hoặc bạn không có quyền truy cập");
        }

        var dispute = await disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute == null)
        {
            return Error.NotFound("Dispute.NotFound", "Không tìm thấy dispute");
        }

        var currentStatus = Enum.Parse<DisputeStatus>(dispute.Status, ignoreCase: true);
        var newStatusResult = stateMachine.Escalate(currentStatus);

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

        return Result.Success();
    }
}