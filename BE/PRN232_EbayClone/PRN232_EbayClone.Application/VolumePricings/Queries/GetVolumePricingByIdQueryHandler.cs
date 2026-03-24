using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.VolumePricings.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.VolumePricings.Queries;

internal sealed class GetVolumePricingByIdQueryHandler : IQueryHandler<GetVolumePricingByIdQuery, VolumePricingDto>
{
    private readonly IVolumePricingRepository _repository;

    public GetVolumePricingByIdQueryHandler(IVolumePricingRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<VolumePricingDto>> Handle(GetVolumePricingByIdQuery request, CancellationToken cancellationToken)
    {
        var pricing = await _repository.GetByIdAsync(request.PricingId, cancellationToken);

        if (pricing == null)
            return Error.Failure("VolumePricing.NotFound", "Volume pricing not found");

        var dto = new VolumePricingDto(
            pricing.Id,
            pricing.Name,
            pricing.Description,
            pricing.ListingId,
            pricing.StartDate,
            pricing.EndDate,
            pricing.IsActive,
            pricing.Tiers.Select(t => new VolumePricingTierDto(t.Id, t.MinQuantity, t.DiscountValue, t.DiscountUnit)).ToList());

        return Result.Success(dto);
    }
}
