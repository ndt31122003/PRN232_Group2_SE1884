using PRN232_EbayClone.Domain.BuyerFeedback.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface ISellerBlacklistRepository : IRepository<SellerBlacklist, Guid>
{
    Task<SellerBlacklist?> GetBySellerAndBuyerAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default);
    Task<List<SellerBlacklist>> GetBySellerAsync(string sellerId, bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<bool> IsBlacklistedAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default);
}