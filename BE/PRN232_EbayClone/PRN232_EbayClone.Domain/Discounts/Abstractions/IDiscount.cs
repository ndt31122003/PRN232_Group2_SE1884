using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Discounts.Abstractions;

public interface IDiscount
{
    Guid Id { get; }
    DiscountType Type { get; }
    string Name { get; }
    bool IsActive { get; }
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    
    Result<Money> CalculateDiscount(Money orderTotal, int itemCount);
    bool IsApplicable(DateTime currentDate);
}
