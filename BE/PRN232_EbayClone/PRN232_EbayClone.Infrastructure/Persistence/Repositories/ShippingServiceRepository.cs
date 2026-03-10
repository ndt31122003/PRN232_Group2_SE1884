using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Orders.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class ShippingServiceRepository
    : Repository<ShippingService, Guid>, IShippingServiceRepository
{
    public ShippingServiceRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
    : base(context, connectionFactory)
    {
    }
    public override Task<ShippingService?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.ShippingServices
            .AsNoTracking()
            .SingleOrDefaultAsync(service => service.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ShippingService>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var services = await DbContext.ShippingServices
            .AsNoTracking()
            .OrderBy(service => service.Carrier)
            .ThenBy(service => service.ServiceName)
            .ToListAsync(cancellationToken);

        return services;
    }
}
