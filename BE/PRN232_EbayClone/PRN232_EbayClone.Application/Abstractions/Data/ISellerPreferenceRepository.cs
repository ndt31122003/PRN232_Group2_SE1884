using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISellerPreferenceRepository : IRepository<SellerPreference, Guid>
{
    Task<SellerPreference?> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
}
