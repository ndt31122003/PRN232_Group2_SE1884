using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IShippingLabelRepository
{
    Task<ShippingLabel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ShippingLabel?> GetLatestForOrderAsync(Guid orderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShippingLabel>> GetByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShippingLabelSummaryRecord>> GetSummariesAsync(
        Guid sellerId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        CancellationToken cancellationToken = default);

    void Add(ShippingLabel label);

    void Update(ShippingLabel label);
}
