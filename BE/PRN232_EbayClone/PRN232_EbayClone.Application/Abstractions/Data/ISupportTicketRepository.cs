using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISupportTicketRepository : IRepository<SupportTicket, Guid>
{
    Task<(List<SupportTicketSummaryDto> Tickets, int TotalCount, int OpenCount, int PendingCount)> GetSupportTicketsAsync(
        SupportTicketFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<SupportTicketDetailDto?> GetTicketDetailByIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken = default);

    Task<string?> GetLastTicketNumberForYearAsync(
        int year,
        CancellationToken cancellationToken = default);
}
