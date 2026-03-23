using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.OrderDiscounts.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.OrderDiscounts.Queries;

internal sealed class GetOrderDiscountsBySellerQueryHandler : IQueryHandler<GetOrderDiscountsBySellerQuery, List<OrderDiscountDto>>
{
    private readonly IOrderDiscountRepository _repository;

    public GetOrderDiscountsBySellerQueryHandler(IOrderDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<OrderDiscountDto>>> Handle(GetOrderDiscountsBySellerQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var discounts = await _repository.GetBySellerIdAsync(sellerId, cancellationToken);

        var dtos = discounts.Select(discount => new OrderDiscountDto(
            discount.Id,
            discount.Name,
            discount.Description,
            discount.ThresholdType,
            discount.ThresholdAmount,
            discount.ThresholdQuantity,
            discount.DiscountValue,
            discount.DiscountUnit,
            discount.MaxDiscount,
            discount.ApplyToAllItems,
            discount.StartDate,
            discount.EndDate,
            discount.IsActive,
            discount.Tiers.Select(t => new OrderDiscountTierDto(t.Id, t.ThresholdValue, t.DiscountValue, t.TierOrder)).ToList(),
            discount.ItemRules.Count(r => !r.IsExclusion),
            discount.ItemRules.Count(r => r.IsExclusion),
            discount.CategoryRules.Count(r => !r.IsExclusion),
            discount.CategoryRules.Count(r => r.IsExclusion))).ToList();

        return Result.Success(dtos);
    }
}
