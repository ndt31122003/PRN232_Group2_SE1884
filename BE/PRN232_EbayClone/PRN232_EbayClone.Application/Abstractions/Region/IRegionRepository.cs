namespace PRN232_EbayClone.Application.Abstractions.Region;
using PRN232_EbayClone.Domain.Regions;
public interface IRegionRepository
{
    Task<Region?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
