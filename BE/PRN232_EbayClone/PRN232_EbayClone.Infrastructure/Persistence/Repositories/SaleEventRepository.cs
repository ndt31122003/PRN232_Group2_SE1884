using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

internal sealed class SaleEventRepository : ISaleEventRepository
{
    private readonly ApplicationDbContext _context;

    public SaleEventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaleEvent?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SaleEvents
            .Include(se => se.DiscountTiers)
            .Include(se => se.Listings)
            .FirstOrDefaultAsync(se => se.Id == id, cancellationToken);
    }

    public async Task<List<SaleEvent>> GetBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.SaleEvents
            .Include(se => se.DiscountTiers)
            .Include(se => se.Listings)
            .Where(se => se.SellerId == sellerId)
            .OrderByDescending(se => se.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SaleEvent>> GetActiveSaleEventsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.SaleEvents
            .Include(se => se.DiscountTiers)
            .Include(se => se.Listings)
            .Where(se => se.Status == SaleEventStatus.Active && se.StartDate <= now && se.EndDate > now)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SaleEvent>> GetActiveSaleEventsForListingAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.SaleEvents
            .Include(se => se.DiscountTiers)
            .Include(se => se.Listings)
            .Where(se => se.Status == SaleEventStatus.Active 
                && se.StartDate <= now 
                && se.EndDate > now
                && se.Listings.Any(l => l.ListingId == listingId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasBeenAppliedToOrdersAsync(Guid saleEventId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<AppliedSaleEvent>()
            .AnyAsync(ase => ase.SaleEventId == saleEventId, cancellationToken);
    }

    public async Task<SaleEventPriceSnapshot?> GetPriceSnapshotAsync(Guid saleEventId, Guid listingId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SaleEventPriceSnapshot>()
            .FirstOrDefaultAsync(s => s.SaleEventId == saleEventId && s.ListingId == listingId, cancellationToken);
    }

    public async Task<SaleEventPerformanceMetrics?> GetPerformanceMetricsAsync(
        Guid saleEventId, 
        DateTime? startDate, 
        DateTime? endDate, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<SaleEventPerformanceMetrics>()
            .Where(m => m.SaleEventId == saleEventId);

        // If date range filtering is needed, we would need to aggregate from AppliedSaleEvents
        // For now, return the aggregate metrics
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<TierPerformanceData>> GetTierPerformanceMetricsAsync(
        Guid saleEventId, 
        DateTime? startDate, 
        DateTime? endDate, 
        CancellationToken cancellationToken = default)
    {
        // Query applied sale events grouped by tier
        var query = _context.Set<AppliedSaleEvent>()
            .Where(ase => ase.SaleEventId == saleEventId);

        if (startDate.HasValue)
        {
            query = query.Where(ase => ase.AppliedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(ase => ase.AppliedAt <= endDate.Value);
        }

        var tierMetrics = await query
            .Where(ase => ase.DiscountTierId.HasValue)
            .GroupBy(ase => ase.DiscountTierId!.Value)
            .Select(g => new
            {
                TierId = g.Key,
                OrderCount = g.Select(x => x.OrderId).Distinct().Count(),
                TotalDiscountAmount = g.Sum(x => x.DiscountAmount),
                // Note: TotalSalesRevenue would need to be calculated from Order data
                // For now, we'll set it to 0 as it requires joining with Orders table
                TotalSalesRevenue = 0m
            })
            .ToListAsync(cancellationToken);

        // Join with tier information to get label and priority
        var tierData = new List<TierPerformanceData>();
        foreach (var metric in tierMetrics)
        {
            var tier = await _context.Set<SaleEventDiscountTier>()
                .FirstOrDefaultAsync(t => t.Id == metric.TierId, cancellationToken);

            if (tier != null)
            {
                tierData.Add(new TierPerformanceData(
                    metric.TierId,
                    tier.Label,
                    tier.Priority,
                    metric.OrderCount,
                    metric.TotalDiscountAmount,
                    metric.TotalSalesRevenue
                ));
            }
        }

        return tierData.OrderBy(t => t.Priority).ToList();
    }

    public async Task AddAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default)
    {
        await _context.SaleEvents.AddAsync(saleEvent, cancellationToken);
    }

    public Task UpdateAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default)
    {
        _context.SaleEvents.Update(saleEvent);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SaleEvent saleEvent, CancellationToken cancellationToken = default)
    {
        _context.SaleEvents.Remove(saleEvent);
        return Task.CompletedTask;
    }

    public async Task AddPriceSnapshotAsync(SaleEventPriceSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        await _context.Set<SaleEventPriceSnapshot>().AddAsync(snapshot, cancellationToken);
    }
}
