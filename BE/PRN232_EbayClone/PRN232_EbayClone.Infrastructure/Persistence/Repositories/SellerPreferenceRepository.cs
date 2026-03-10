using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class SellerPreferenceRepository : Repository<SellerPreference, Guid>, ISellerPreferenceRepository
{
    public SellerPreferenceRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<SellerPreference?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.SellerPreferences
            .Include(x => x.BlockedBuyers)
            .Include(x => x.ExemptBuyers)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<SellerPreference?> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
    {
        var userId = new UserId(sellerId);
        return DbContext.SellerPreferences
            .Include(x => x.BlockedBuyers)
            .Include(x => x.ExemptBuyers)
            .FirstOrDefaultAsync(x => x.SellerId == userId, cancellationToken);
    }
}
