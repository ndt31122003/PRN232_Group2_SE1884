using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISupportTicketRepository : IRepository<SupportTicket, Guid>
{
    Task<SupportTicket?> GetByIdAndSellerAsync(Guid id, string sellerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SupportTicket>> GetBySellerIdAsync(string sellerId, CancellationToken cancellationToken = default);
}