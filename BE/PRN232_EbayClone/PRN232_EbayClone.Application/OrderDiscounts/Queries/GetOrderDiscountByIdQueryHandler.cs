using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.OrderDiscounts.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.OrderDiscounts.Queries;

internal sealed class GetOrderDiscountByIdQueryHandler : IQueryHandler<GetOrderDiscountByIdQuery, OrderDiscountDto>
{
    private readonly IOrderDiscountRepository _repository;

    public GetOrderDiscountByIdQueryHandler(IOrderDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<OrderDiscountDto>> Handle(GetOrderDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);
        
        if (discount == null)
            return Error.Failure("OrderDiscount.NotFound", "Discount not found");

        var dto = new OrderDiscountDto(
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
            discount.CategoryRules.Count(r => r.IsExclusion));

        return Result.Success(dto);
    }
}
