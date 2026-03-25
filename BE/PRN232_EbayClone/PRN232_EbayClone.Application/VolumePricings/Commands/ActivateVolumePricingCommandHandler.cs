using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

internal sealed class ActivateVolumePricingCommandHandler : ICommandHandler<ActivateVolumePricingCommand>
{
    private readonly IVolumePricingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateVolumePricingCommandHandler(
        IVolumePricingRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateVolumePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = await _repository.GetByIdAsync(request.PricingId, cancellationToken);

        if (pricing == null)
            return Error.Failure("VolumePricing.NotFound", "Volume pricing not found");

        if (pricing.IsActive)
            return Error.Failure("VolumePricing.AlreadyActive", "Volume pricing is already active");

        pricing.Activate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
