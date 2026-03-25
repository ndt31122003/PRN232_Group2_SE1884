using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Enums;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class DisputeRepository : Repository<Dispute, Guid>, IDisputeRepository
{
    public DisputeRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<Dispute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Disputes
            .Include(d => d.Listing)
            .Include(d => d.Responses)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Dispute> Disputes, int TotalCount)> GetDisputesAsync(
        DisputeFilterDto filter,
        string currentUserId,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[DisputeRepository] GetDisputesAsync called with currentUserId: {currentUserId}");
        
        var query = DbContext.Disputes
            .AsNoTracking()
            .Include(d => d.Listing)
            .Include(d => d.Responses)
            .AsQueryable();

        // Filter by current user: either disputes raised by user OR disputes on user's listings
        query = query.Where(d => 
            d.RaisedById == currentUserId || 
            (d.Listing != null && d.Listing.CreatedBy == currentUserId));

        Console.WriteLine($"[DisputeRepository] After user filter, query has been built");

        if (filter.ListingId.HasValue)
        {
            query = query.Where(d => d.ListingId == filter.ListingId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.RaisedById))
        {
            query = query.Where(d => d.RaisedById == filter.RaisedById);
        }

        if (!string.IsNullOrWhiteSpace(filter.SellerId))
        {
            // Filter by listings created by this seller
            query = query.Where(d => d.Listing != null && d.Listing.CreatedBy == filter.SellerId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(d => d.Status == filter.Status);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt <= filter.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        Console.WriteLine($"[DisputeRepository] Total count after filters: {totalCount}");

        var disputes = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        Console.WriteLine($"[DisputeRepository] Returning {disputes.Count} disputes");
        
        return (disputes, totalCount);
    }

    public async Task<IReadOnlyList<Dispute>> GetDisputesByListingIdAsync(
        Guid listingId,
        CancellationToken cancellationToken = default)
    {
        var disputes = await DbContext.Disputes
            .AsNoTracking()
            .Where(d =>
                d.ListingId == listingId &&
                d.Status != DisputeStatus.Resolved.ToString() &&
                d.Status != DisputeStatus.Closed.ToString())
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);

        return disputes;
    }

    public async Task<(IReadOnlyList<Dispute> Disputes, int TotalCount)> GetDisputesByBuyerIdAsync(
        string buyerId,
        DisputeFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[DisputeRepository] GetDisputesByBuyerIdAsync called with buyerId: {buyerId}");
        
        var query = DbContext.Disputes
            .AsNoTracking()
            .Include(d => d.Listing)
            .Include(d => d.Responses)
            .Where(d => d.RaisedById == buyerId); // Only disputes raised by this buyer

        Console.WriteLine($"[DisputeRepository] Filtering disputes raised by buyer: {buyerId}");

        if (filter.ListingId.HasValue)
        {
            query = query.Where(d => d.ListingId == filter.ListingId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(d => d.Status == filter.Status);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(d => d.CreatedAt <= filter.ToDate.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        Console.WriteLine($"[DisputeRepository] Total buyer disputes: {totalCount}");

        var disputes = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        Console.WriteLine($"[DisputeRepository] Returning {disputes.Count} buyer disputes");
        
        return (disputes, totalCount);
    }
}
