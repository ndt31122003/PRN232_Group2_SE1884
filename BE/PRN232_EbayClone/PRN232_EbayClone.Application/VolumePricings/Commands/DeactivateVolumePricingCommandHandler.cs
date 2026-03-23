using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

internal sealed class DeactivateVolumePricingCommandHandler : ICommandHandler<DeactivateVolumePricingCommand>
{
    private readonly IVolumePricingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateVolumePricingCommandHandler(
        IVolumePricingRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeactivateVolumePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = await _repository.GetByIdAsync(request.PricingId, cancellationToken);

        if (pricing == null)
            return Error.Failure("VolumePricing.NotFound", "Volume pricing not found");

        if (!pricing.IsActive)
            return Error.Failure("VolumePricing.AlreadyInactive", "Volume pricing is already inactive");

        pricing.Deactivate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
