using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ListingTemplateRepository :
    Repository<ListingTemplate, Guid>,
    IListingTemplateRepository
{
    public ListingTemplateRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public override Task<ListingTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<ListingTemplate>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    private IQueryable<ListingTemplate> OwnedTemplates(string ownerId, bool asNoTracking = true)
    {
        var set = DbContext.Set<ListingTemplate>();
        var query = asNoTracking ? set.AsNoTracking() : set.AsQueryable();

        return query.Where(t => t.CreatedBy == ownerId);
    }

    public async Task<(IReadOnlyList<ListingTemplate> Items, int TotalCount)> GetPagedAsync(string? ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<ListingTemplate>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(ownerId))
        {
            query = query.Where(t => t.CreatedBy == ownerId);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = $"%{searchTerm.Trim()}%";
            query = query.Where(t => EF.Functions.ILike(t.Name, term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Array.Empty<ListingTemplate>(), 0);
        }

        var items = await query
            .OrderByDescending(t => t.UpdatedAt ?? t.CreatedAt)
            .ThenBy(t => t.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<ListingTemplate?> GetByIdForOwnerAsync(Guid id, string ownerId, CancellationToken cancellationToken = default)
    {
        return OwnedTemplates(ownerId)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<bool> NameExistsAsync(string? ownerId, string name, Guid? excludeId, CancellationToken cancellationToken = default)
    {
        var trimmedName = name.Trim();
        var query = DbContext.Set<ListingTemplate>().AsNoTracking()
            .Where(t => t.Name == trimmedName);

        if (!string.IsNullOrWhiteSpace(ownerId))
        {
            query = query.Where(t => t.CreatedBy == ownerId);
        }

        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }

        return query.AnyAsync(cancellationToken);
    }

    public Task<int> CountByOwnerAsync(string ownerId, CancellationToken cancellationToken = default)
    {
        return OwnedTemplates(ownerId).CountAsync(cancellationToken);
    }
}
