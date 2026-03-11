using PRN232_EbayClone.Domain.FileMetadata.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IFileMetadataRepository : IRepository<FileMetadata, Guid>
{
    Task<List<FileMetadata>> GetFileMetadatasByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
