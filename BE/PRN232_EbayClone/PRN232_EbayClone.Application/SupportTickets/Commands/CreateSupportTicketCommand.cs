using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.FileMetadata.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Application.SupportTickets.Commands;

public sealed record CreateSupportTicketCommand(
    string SellerId,
    string Category,
    string Subject,
    string Message,
    IFormFileCollection? Attachments = null
) : ICommand<Guid>;

internal sealed class CreateSupportTicketCommandHandler(
    ISupportTicketRepository supportTicketRepository,
    IFileMetadataRepository fileMetadataRepository,
    IUnitOfWork unitOfWork,
    IFileManager fileManager)
    : ICommandHandler<CreateSupportTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
    {
        var ticketResult = SupportTicket.Create(
            request.SellerId,
            request.Category,
            request.Subject,
            request.Message);

        if (ticketResult.IsFailure)
        {
            return Result.Failure<Guid>(ticketResult.Error);
        }

        var ticket = ticketResult.Value;
        supportTicketRepository.Add(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Handle file attachments if provided
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            var uploadTasks = request.Attachments.Select(async file =>
            {
                var uploadResult = await fileManager.UploadFileAsync(file);
                if (uploadResult.IsFailure)
                {
                    return uploadResult.Error;
                }

                // Create file metadata with linked_entity_id = ticket.id
                var fileMetadataResult = FileMetadata.Create(
                    uploadResult.Value,
                    file.FileName,
                    file.ContentType ?? "application/octet-stream",
                    file.Length,
                    ticket.Id
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
                return Result.Failure<Guid>(failedUpload.Error);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Success(ticket.Id);
    }
}