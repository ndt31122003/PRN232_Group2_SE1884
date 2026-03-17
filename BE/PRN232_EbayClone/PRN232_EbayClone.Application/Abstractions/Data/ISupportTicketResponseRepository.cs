using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISupportTicketResponseRepository : IRepository<SupportTicketResponse, Guid>
{
    Task<IReadOnlyList<SupportTicketResponse>> GetResponsesByTicketIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken = default);
}
