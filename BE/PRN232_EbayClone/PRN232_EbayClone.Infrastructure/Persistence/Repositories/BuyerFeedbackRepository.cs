using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.BuyerFeedback.Entities;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

internal sealed class BuyerFeedbackRepository : Repository<BuyerFeedbackEntity, Guid>, IBuyerFeedbackRepository
{
    public BuyerFeedbackRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<BuyerFeedbackEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Set<BuyerFeedbackEntity>()
            .FirstOrDefaultAsync(bf => bf.Id == id, cancellationToken);
    }

    public async Task<BuyerFeedbackEntity?> GetByOrderAsync(string sellerId, string buyerId, Guid orderId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<BuyerFeedbackEntity>()
            .FirstOrDefaultAsync(bf => bf.SellerId == sellerId && bf.BuyerId == buyerId && bf.OrderId == orderId, cancellationToken);
    }

    public async Task<int> CountNegativeFeedbacksAsync(string sellerId, string buyerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<BuyerFeedbackEntity>()
            .CountAsync(bf => bf.SellerId == sellerId && bf.BuyerId == buyerId && bf.FeedbackType == FeedbackType.Negative, cancellationToken);
    }

    public async Task<List<BuyerFeedbackEntity>> GetByBuyerAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<BuyerFeedbackEntity>()
            .Where(bf => bf.BuyerId == buyerId)
            .OrderByDescending(bf => bf.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BuyerFeedbackEntity>> GetBySellerAsync(string sellerId, FeedbackType? feedbackType = null, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<BuyerFeedbackEntity>()
            .Where(bf => bf.SellerId == sellerId);

        if (feedbackType.HasValue)
        {
            query = query.Where(bf => bf.FeedbackType == feedbackType.Value);
        }

        return await query
            .OrderByDescending(bf => bf.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}