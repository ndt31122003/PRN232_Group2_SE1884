using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

internal sealed class DeleteVolumePricingCommandHandler : ICommandHandler<DeleteVolumePricingCommand>
{
    private readonly IVolumePricingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVolumePricingCommandHandler(
        IVolumePricingRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteVolumePricingCommand request, CancellationToken cancellationToken)
    {
        var pricing = await _repository.GetByIdAsync(request.PricingId, cancellationToken);

        if (pricing == null)
            return Error.Failure("VolumePricing.NotFound", "Volume pricing not found");

        await _repository.DeleteAsync(request.PricingId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
