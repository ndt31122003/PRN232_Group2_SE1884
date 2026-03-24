using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.VolumePricings.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.VolumePricings.Queries;

internal sealed class GetVolumePricingsBySellerQueryHandler : IQueryHandler<GetVolumePricingsBySellerQuery, List<VolumePricingDto>>
{
    private readonly IVolumePricingRepository _repository;

    public GetVolumePricingsBySellerQueryHandler(IVolumePricingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<VolumePricingDto>>> Handle(GetVolumePricingsBySellerQuery request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);
        var pricings = await _repository.GetBySellerIdAsync(sellerId, cancellationToken);

        var dtos = pricings.Select(v => new VolumePricingDto(
            v.Id,
            v.Name,
            v.Description,
            v.ListingId,
            v.StartDate,
            v.EndDate,
            v.IsActive,
            v.Tiers.Select(t => new VolumePricingTierDto(t.Id, t.MinQuantity, t.DiscountValue, t.DiscountUnit)).ToList()
        )).ToList();

        return Result.Success(dtos);
    }
}
