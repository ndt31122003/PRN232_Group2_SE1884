using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IShippingDiscountRepository
{
    Task<ShippingDiscount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShippingDiscount>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShippingDiscount>> GetActiveDiscountsAsync(DateTime currentDate, CancellationToken cancellationToken = default);
    Task AddAsync(ShippingDiscount discount, CancellationToken cancellationToken = default);
    Task UpdateAsync(ShippingDiscount discount, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
