using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ShippingLabelRepository : IShippingLabelRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ShippingLabelRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<ShippingLabel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.ShippingLabels
            .AsTracking()
            .SingleOrDefaultAsync(label => label.Id == id, cancellationToken);
    }

    public Task<ShippingLabel?> GetLatestForOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return _dbContext.ShippingLabels
            .AsNoTracking()
            .Where(label => label.OrderId == orderId)
            .OrderByDescending(label => label.PurchasedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ShippingLabel>> GetByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default)
    {
        if (orderIds.Count == 0)
        {
            return Array.Empty<ShippingLabel>();
        }

        return await _dbContext.ShippingLabels
            .AsNoTracking()
            .Where(label => orderIds.Contains(label.OrderId))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ShippingLabelSummaryRecord>> GetSummariesAsync(
        Guid sellerId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        CancellationToken cancellationToken = default)
    {
        limit = Math.Max(1, limit);

        var labelQuery = _dbContext.ShippingLabels
            .AsNoTracking()
            .Join(
                _dbContext.Orders.AsNoTracking().Where(order => order.SellerId == sellerId),
                label => label.OrderId,
                order => order.Id,
                (label, order) => new { label, order });

        if (fromUtc.HasValue)
        {
            var from = fromUtc.Value;
            labelQuery = labelQuery.Where(pair => pair.label.PurchasedAt >= from);
        }

        if (toUtc.HasValue)
        {
            var to = toUtc.Value;
            labelQuery = labelQuery.Where(pair => pair.label.PurchasedAt <= to);
        }

        var records = await labelQuery
            .OrderByDescending(pair => pair.label.PurchasedAt)
            .Take(limit)
            .Select(pair => new ShippingLabelSummaryRecord(
                pair.label.Id,
                pair.order.Id,
                pair.order.OrderNumber,
                pair.label.Carrier,
                pair.label.ServiceName,
                pair.label.TrackingNumber,
                pair.label.PurchasedAt,
                pair.label.Cost.Amount,
                pair.label.Cost.Currency,
                pair.label.LabelUrl,
                pair.label.LabelFileName,
                pair.label.IsVoided,
                pair.label.VoidedAt,
                pair.label.VoidReason))
            .ToListAsync(cancellationToken);

        return records;
    }

    public void Add(ShippingLabel label)
    {
        _dbContext.ShippingLabels.Add(label);
    }

    public void Update(ShippingLabel label)
    {
        _dbContext.ShippingLabels.Update(label);
    }
}
