using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.ShippingDiscounts.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Queries;

internal sealed class GetShippingDiscountsBySellerQueryHandler : IQueryHandler<GetShippingDiscountsBySellerQuery, List<ShippingDiscountDto>>
{
    private readonly IShippingDiscountRepository _repository;

    public GetShippingDiscountsBySellerQueryHandler(IShippingDiscountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ShippingDiscountDto>>> Handle(GetShippingDiscountsBySellerQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var discounts = await _repository.GetBySellerIdAsync(sellerId, cancellationToken);

        var dtos = discounts.Select(d => new ShippingDiscountDto(
            d.Id,
            d.Name,
            d.Description,
            d.DiscountValue,
            d.DiscountUnit,
            d.IsFreeShipping,
            d.MinimumOrderValue,
            d.StartDate,
            d.EndDate,
            d.IsActive)).ToList();

        return Result.Success(dtos);
    }
}
