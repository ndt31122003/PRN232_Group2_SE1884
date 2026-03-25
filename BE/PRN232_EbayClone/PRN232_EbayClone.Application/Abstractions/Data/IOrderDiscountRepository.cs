using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IOrderDiscountRepository
{
    Task<OrderDiscount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDiscount>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDiscount>> GetActiveDiscountsAsync(DateTime currentDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDiscount>> GetActiveDiscountsForListingAsync(Guid listingId, CancellationToken cancellationToken = default);
    Task<bool> HasBeenAppliedToOrdersAsync(Guid discountId, CancellationToken cancellationToken = default);
    Task AddAsync(OrderDiscount discount, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrderDiscount discount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
