using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

internal sealed class CreateVolumePricingCommandHandler : ICommandHandler<CreateVolumePricingCommand, Guid>
{
    private readonly IVolumePricingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateVolumePricingCommandHandler(
        IVolumePricingRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateVolumePricingCommand request, CancellationToken cancellationToken)
    {
        var sellerId = new UserId(request.SellerId);

        var tierDefinitions = request.Tiers.Select(t => new VolumePricingTierDefinition(
            t.MinQuantity,
            t.DiscountValue,
            t.DiscountUnit.ToLowerInvariant() == "percent" ? DiscountUnit.Percent : DiscountUnit.FixedAmount)).ToList();

        var pricingResult = VolumePricing.Create(
            sellerId,
            request.ListingId,
            request.Name,
            request.Description,
            request.StartDate,
            request.EndDate,
            tierDefinitions);

        if (pricingResult.IsFailure)
            return pricingResult.Error;

        await _repository.AddAsync(pricingResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(pricingResult.Value.Id);
    }
}
