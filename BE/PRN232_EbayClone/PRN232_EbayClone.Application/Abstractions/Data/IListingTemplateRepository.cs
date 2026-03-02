using PRN232_EbayClone.Domain.ListingTemplates.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IListingTemplateRepository : IRepository<ListingTemplate, Guid>
{
    Task<(IReadOnlyList<ListingTemplate> Items, int TotalCount)> GetPagedAsync(string? ownerId, string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<ListingTemplate?> GetByIdForOwnerAsync(Guid id, string ownerId, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string? ownerId, string name, Guid? excludeId, CancellationToken cancellationToken = default);
    Task<int> CountByOwnerAsync(string ownerId, CancellationToken cancellationToken = default);
}
