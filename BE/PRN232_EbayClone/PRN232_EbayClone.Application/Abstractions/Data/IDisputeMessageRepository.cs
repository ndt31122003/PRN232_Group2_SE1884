using PRN232_EbayClone.Domain.Disputes.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IDisputeMessageRepository : IRepository<DisputeMessage, Guid>
{
    Task<IReadOnlyList<DisputeMessage>> GetMessagesByDisputeIdAsync(
        Guid disputeId,
        CancellationToken cancellationToken = default);
}
