using PRN232_EbayClone.Domain.Orders.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ICancellationRequestRepository : IRepository<CancellationRequest, Guid>
{
    Task<IReadOnlyList<CancellationRequest>> GetOpenBuyerInitiatedAsync(
        CancellationToken cancellationToken = default);
}
