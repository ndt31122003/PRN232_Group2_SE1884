using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Enums;
using PRN232_EbayClone.Domain.FileMetadata.Entities;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record ProvideEvidenceCommand(
    Guid DisputeId,
    string SellerId,
    IFormFileCollection Files
) : ICommand;

internal sealed class ProvideEvidenceCommandHandler(
    IDisputeRepository disputeRepository,
    IListingRepository listingRepository,
    IFileMetadataRepository fileMetadataRepository,
    IUnitOfWork unitOfWork,
    IDisputeStateMachine stateMachine,
    IFileManager fileManager,
    IRealtimeNotifier realtimeNotifier)
    : ICommandHandler<ProvideEvidenceCommand>
{
    public async Task<Result> Handle(ProvideEvidenceCommand request, CancellationToken cancellationToken)
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
        var newStatusResult = stateMachine.ProvideEvidence(currentStatus);

        if (newStatusResult.IsFailure)
        {
            return newStatusResult.Error;
        }

        // Upload files and save to file_metadata
        var uploadTasks = request.Files.Select(async file =>
        {
            var uploadResult = await fileManager.UploadFileAsync(file);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            // Create file metadata with linked_entity_id = dispute.id
            var fileMetadataResult = FileMetadata.Create(
                uploadResult.Value,
                file.FileName,
                file.ContentType ?? "application/octet-stream",
                file.Length,
                request.DisputeId
            );

            if (fileMetadataResult.IsFailure)
            {
                return fileMetadataResult.Error;
            }

            fileMetadataRepository.Add(fileMetadataResult.Value);
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

        // Send realtime notification to buyer
        try
        {
            await realtimeNotifier.SendToUserAsync(
                dispute.RaisedById,
                "DisputeEvidenceUploaded",
                new
                {
                    DisputeId = dispute.Id,
                    FileCount = request.Files.Count,
                    Message = $"Seller has uploaded {request.Files.Count} evidence file(s)",
                    Timestamp = DateTime.UtcNow
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            // Log but don't fail the request if notification fails
            Console.WriteLine($"[ProvideEvidence] Failed to send realtime notification: {ex.Message}");
        }

        return Result.Success();
    }
}