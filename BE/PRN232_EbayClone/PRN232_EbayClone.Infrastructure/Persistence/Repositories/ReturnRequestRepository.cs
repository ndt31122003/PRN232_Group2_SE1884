using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Orders.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ReturnRequestRepository
    : Repository<ReturnRequest, Guid>, IReturnRequestRepository
{
    public ReturnRequestRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
    : base(context, connectionFactory)
    {
    }

    public override Task<ReturnRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.ReturnRequests
            .Include(request => request.Order)
                .ThenInclude(order => order.Status)
                .ThenInclude(status => status.AllowedTransitions)
                .ThenInclude(transition => transition.ToStatus)
            .Include(request => request.Order)
                .ThenInclude(order => order.Buyer)
            .Include(request => request.Order)
                .ThenInclude(order => order.Items)
            .SingleOrDefaultAsync(request => request.Id == id, cancellationToken);
    }
}
