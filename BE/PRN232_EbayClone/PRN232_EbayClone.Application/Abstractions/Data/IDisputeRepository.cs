using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IDisputeRepository : IRepository<Dispute, Guid>
{
    Task<(IReadOnlyList<Dispute> Disputes, int TotalCount)> GetDisputesAsync(
        DisputeFilterDto filter,
        string currentUserId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Dispute>> GetDisputesByListingIdAsync(
        Guid listingId,
        CancellationToken cancellationToken = default);
}

