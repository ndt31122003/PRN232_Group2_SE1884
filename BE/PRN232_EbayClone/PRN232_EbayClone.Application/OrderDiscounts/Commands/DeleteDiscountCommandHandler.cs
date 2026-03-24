using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

internal sealed class DeleteDiscountCommandHandler : ICommandHandler<DeleteDiscountCommand>
{
    private readonly IOrderDiscountRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDiscountCommandHandler(
        IOrderDiscountRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var discount = await _repository.GetByIdAsync(request.DiscountId, cancellationToken);
        
        if (discount == null)
            return Error.Failure("OrderDiscount.NotFound", "Discount not found");

        var hasBeenApplied = await _repository.HasBeenAppliedToOrdersAsync(request.DiscountId, cancellationToken);
        
        if (hasBeenApplied)
            return Error.Failure("OrderDiscount.HasBeenApplied", "Cannot delete discount that has been applied to orders");

        await _repository.DeleteAsync(request.DiscountId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
