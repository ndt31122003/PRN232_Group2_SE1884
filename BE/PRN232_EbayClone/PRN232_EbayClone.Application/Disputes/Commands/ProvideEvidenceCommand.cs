using Dapper;
using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record ProvideEvidenceCommand(
    Guid DisputeId,
    string SellerId,
    IFormFileCollection Files
) : ICommand;

internal sealed class ProvideEvidenceCommandHandler(
    IDisputeRepository disputeRepository,
    IUnitOfWork unitOfWork,
    IDisputeStateMachine stateMachine,
    IDbConnectionFactory dbConnectionFactory,
    IFileService fileService)
    : ICommandHandler<ProvideEvidenceCommand>
{
    public async Task<Result> Handle(ProvideEvidenceCommand request, CancellationToken cancellationToken)
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
        var newStatusResult = stateMachine.ProvideEvidence(currentStatus);

        if (newStatusResult.IsFailure)
        {
            return newStatusResult.Error;
        }

        // Upload files and save to file_metadata
        var uploadTasks = request.Files.Select(async file =>
        {
            var uploadResult = await fileService.UploadAsync(file, cancellationToken);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            // Save file metadata with linked_entity_id = dispute.id
            var insertSql = @"
                INSERT INTO file_metadata (id, file_name, content_type, size, url, linked_entity_id, created_at, updated_at)
                VALUES (@Id, @FileName, @ContentType, @Size, @Url, @LinkedEntityId, @CreatedAt, @UpdatedAt)";

            await connection.ExecuteAsync(insertSql, new
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                ContentType = file.ContentType,
                Size = file.Length,
                Url = uploadResult.Value,
                LinkedEntityId = request.DisputeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            return Result.Success();
        });

        var uploadResults = await Task.WhenAll(uploadTasks);
        var failedUpload = uploadResults.FirstOrDefault(r => r.IsFailure);
        if (failedUpload != null)
        {
            return failedUpload.Error;
        }

        // Update dispute status
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