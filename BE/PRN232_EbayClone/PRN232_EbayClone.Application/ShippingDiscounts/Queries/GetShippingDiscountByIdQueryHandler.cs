using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.ShippingDiscounts.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Queries;

internal sealed class GetShippingDiscountByIdQueryHandler : IQueryHandler<GetShippingDiscountByIdQuery, ShippingDiscountDto>
{
    private readonly IShippingDiscountRepository _repository;

    public GetShippingDiscountByIdQueryHandler(IShippingDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ShippingDiscountDto>> Handle(GetShippingDiscountByIdQuery request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);

        if (discount == null)
            return Error.Failure("ShippingDiscount.NotFound", "Shipping discount not found");

        var dto = new ShippingDiscountDto(
            discount.Id,
            discount.Name,
            discount.Description,
            discount.DiscountValue,
            discount.DiscountUnit,
            discount.IsFreeShipping,
            discount.MinimumOrderValue,
            discount.StartDate,
            discount.EndDate,
            discount.IsActive);

        return Result.Success(dto);
    }
}
