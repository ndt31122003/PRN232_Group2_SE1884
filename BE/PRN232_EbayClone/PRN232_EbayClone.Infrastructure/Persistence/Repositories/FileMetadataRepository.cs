using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.FileMetadata.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class FileMetadataRepository :
    Repository<FileMetadata, Guid>,
    IFileMetadataRepository
{
    public FileMetadataRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public override Task<FileMetadata?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return DbContext.Set<FileMetadata>()
            .SingleOrDefaultAsync(fm => fm.Id == id, cancellationToken);
    }

    public Task<List<FileMetadata>> GetFileMetadatasByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids is null || !ids.Any())
            return Task.FromResult(new List<FileMetadata>());

        var idSet = ids.ToHashSet();

        return DbContext.Set<FileMetadata>()
            .AsNoTracking()
            .Where(fm => idSet.Contains(fm.Id))
            .ToListAsync(cancellationToken);
    }

    public Task<List<FileMetadata>> GetByLinkedEntityAsync(
        Guid linkedEntityId,
        CancellationToken cancellationToken)
    {
        return DbContext.Set<FileMetadata>()
            .AsNoTracking()
            .Where(fm => fm.LinkedEntityId == linkedEntityId)
            .OrderByDescending(fm => fm.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountByLinkedEntityAsync(
        Guid linkedEntityId,
        CancellationToken cancellationToken)
    {
        return DbContext.Set<FileMetadata>()
            .CountAsync(fm => fm.LinkedEntityId == linkedEntityId, cancellationToken);
    }
}
