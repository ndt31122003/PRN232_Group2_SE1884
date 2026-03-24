using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.SupportTickets.Queries;

public sealed record GetSupportTicketByIdQuery(
    Guid TicketId,
    string SellerId
) : IQuery<SupportTicketDetailDto>;

internal sealed class GetSupportTicketByIdQueryHandler(
    ISupportTicketRepository supportTicketRepository,
    IFileMetadataRepository fileMetadataRepository)
    : IQueryHandler<GetSupportTicketByIdQuery, SupportTicketDetailDto>
{
    public async Task<Result<SupportTicketDetailDto>> Handle(
        GetSupportTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await supportTicketRepository.GetByIdAndSellerAsync(request.TicketId, request.SellerId, cancellationToken);

        if (ticket == null)
        {
            return Result.Failure<SupportTicketDetailDto>(Error.NotFound("SupportTicket.NotFound", "Không tìm thấy support ticket hoặc bạn không có quyền truy cập"));
        }

        var attachments = await fileMetadataRepository.GetByLinkedEntityAsync(request.TicketId, cancellationToken);

        var attachmentDtos = attachments.Select(a => new TicketAttachmentDto(
            a.Id,
            a.FileName,
            a.ContentType,
            a.Size,
            a.Url,
            a.CreatedAt
        )).ToList();

        var result = new SupportTicketDetailDto(
            ticket.Id,
            ticket.SellerId,
            ticket.Category,
            ticket.Subject,
            ticket.Message,
            ticket.Status,
            ticket.CreatedAt,
            ticket.UpdatedAt ?? ticket.CreatedAt,
            attachmentDtos
        );

        return Result.Success(result);
    }
}