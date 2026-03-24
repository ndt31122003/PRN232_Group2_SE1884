using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.SupportTickets.Queries;

public sealed record GetSellerSupportTicketsQuery(
    string SellerId,
    SupportTicketFilterDto Filter
) : IQuery<PagedResult<SupportTicketDto>>;

internal sealed class GetSellerSupportTicketsQueryHandler(
    ISupportTicketRepository supportTicketRepository,
    IFileMetadataRepository fileMetadataRepository)
    : IQueryHandler<GetSellerSupportTicketsQuery, PagedResult<SupportTicketDto>>
{
    public async Task<Result<PagedResult<SupportTicketDto>>> Handle(
        GetSellerSupportTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var tickets = await supportTicketRepository.GetBySellerIdAsync(request.SellerId, cancellationToken);

        // Apply filters
        if (!string.IsNullOrEmpty(request.Filter.Status))
        {
            tickets = tickets.Where(st => st.Status == request.Filter.Status).ToList();
        }

        if (!string.IsNullOrEmpty(request.Filter.Category))
        {
            tickets = tickets.Where(st => st.Category == request.Filter.Category).ToList();
        }

        if (!string.IsNullOrEmpty(request.Filter.Keyword))
        {
            var keyword = request.Filter.Keyword.ToLower();
            tickets = tickets.Where(st => st.Subject.ToLower().Contains(keyword) || 
                                         st.Message.ToLower().Contains(keyword)).ToList();
        }

        var totalCount = tickets.Count;

        var pagedTickets = tickets
            .OrderByDescending(st => st.CreatedAt)
            .Skip((request.Filter.Page - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToList();

        // Get all ticket IDs for batch query
        var ticketIds = pagedTickets.Select(t => t.Id).ToList();
        
        // Get attachment counts for all tickets in one query (if any tickets exist)
        var attachmentCounts = new Dictionary<Guid, int>();
        if (ticketIds.Any())
        {
            foreach (var ticketId in ticketIds)
            {
                try
                {
                    var count = await fileMetadataRepository.CountByLinkedEntityAsync(ticketId, cancellationToken);
                    attachmentCounts[ticketId] = count;
                }
                catch
                {
                    // If count fails, default to 0
                    attachmentCounts[ticketId] = 0;
                }
            }
        }

        var ticketDtos = pagedTickets.Select(ticket => new SupportTicketDto(
            ticket.Id,
            ticket.SellerId,
            ticket.Category,
            ticket.Subject,
            ticket.Message,
            ticket.Status,
            ticket.CreatedAt,
            ticket.UpdatedAt ?? ticket.CreatedAt,
            attachmentCounts.GetValueOrDefault(ticket.Id, 0)
        )).ToList();

        return Result.Success(new PagedResult<SupportTicketDto>(
            ticketDtos,
            request.Filter.Page,
            request.Filter.PageSize,
            totalCount
        ));
    }
}