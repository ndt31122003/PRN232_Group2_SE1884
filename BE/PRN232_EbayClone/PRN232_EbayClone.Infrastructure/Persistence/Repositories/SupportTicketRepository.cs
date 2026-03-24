using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SupportTickets.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class SupportTicketRepository : Repository<SupportTicket, Guid>, ISupportTicketRepository
{
    public SupportTicketRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public override Task<SupportTicket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<SupportTicket>()
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, cancellationToken);
    }

    public async Task<SupportTicket?> GetByIdAndSellerAsync(Guid id, string sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SupportTicket>()
            .FirstOrDefaultAsync(t => t.Id == id && t.SellerId == sellerId && !t.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SupportTicket>> GetBySellerIdAsync(string sellerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<SupportTicket>()
            .Where(t => t.SellerId == sellerId && !t.IsDeleted)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}