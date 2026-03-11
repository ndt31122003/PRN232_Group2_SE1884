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
        return DbContext.FileMetadatas
            .SingleOrDefaultAsync(fm => fm.Id == id, cancellationToken);
    }

    public Task<List<FileMetadata>> GetFileMetadatasByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken)
    {
        if (ids is null || !ids.Any())
            return Task.FromResult(new List<FileMetadata>());

        var idSet = ids.ToHashSet();

        return DbContext.FileMetadatas
            .AsNoTracking()
            .Where(fm => idSet.Contains(fm.Id))
            .ToListAsync(cancellationToken);
    }

}
