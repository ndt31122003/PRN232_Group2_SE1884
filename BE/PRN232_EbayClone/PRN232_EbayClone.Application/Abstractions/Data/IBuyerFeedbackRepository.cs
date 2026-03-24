using PRN232_EbayClone.Domain.BuyerFeedback.Entities;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IBuyerFeedbackRepository : IRepository<BuyerFeedbackEntity, Guid>
{
    Task<BuyerFeedbackEntity?> GetByOrderAsync(string sellerId, string buyerId, Guid orderId, CancellationToken cancellationToken = default);
    Task<int> CountNegativeFeedbacksAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default);
    Task<List<BuyerFeedbackEntity>> GetByBuyerAsync(string buyerId, CancellationToken cancellationToken = default);
    Task<List<BuyerFeedbackEntity>> GetBySellerAsync(string sellerId, FeedbackType? feedbackType = null, CancellationToken cancellationToken = default);
}