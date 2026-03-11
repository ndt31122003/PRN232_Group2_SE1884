using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Orders.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class CancellationRequestRepository
    : Repository<CancellationRequest, Guid>, ICancellationRequestRepository
{
    public CancellationRequestRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
    : base(context, connectionFactory)
    {
    }

    public override Task<CancellationRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.CancellationRequests
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

    public async Task<IReadOnlyList<CancellationRequest>> GetOpenBuyerInitiatedAsync(
        CancellationToken cancellationToken = default)
    {
        var requests = await DbContext.CancellationRequests
            .AsSplitQuery()
            .Include(request => request.Order)
                .ThenInclude(order => order.Status)
                .ThenInclude(status => status.AllowedTransitions)
                .ThenInclude(transition => transition.ToStatus)
            .Include(request => request.Order)
                .ThenInclude(order => order.StatusHistory)
            .Where(request => request.InitiatedBy == CancellationInitiator.Buyer)
            .Where(request => request.Status == CancellationStatus.PendingSellerResponse || request.Status == CancellationStatus.PendingBuyerConfirmation)
            .ToListAsync(cancellationToken);

        return requests;
    }
}
