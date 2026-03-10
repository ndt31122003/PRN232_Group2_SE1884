using PRN232_EbayClone.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IShippingServiceRepository : IRepository<ShippingService, Guid>
{
    Task<IReadOnlyList<ShippingService>> GetAllAsync(CancellationToken cancellationToken = default);
}
