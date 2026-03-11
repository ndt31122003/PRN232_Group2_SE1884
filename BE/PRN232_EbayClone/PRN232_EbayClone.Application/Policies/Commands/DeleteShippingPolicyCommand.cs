using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Policies.Commands;

public sealed record DeleteShippingPolicyCommand(
    string StoreId,
    Guid PolicyId
) : ICommand;

public sealed class DeleteShippingPolicyCommandValidator : AbstractValidator<DeleteShippingPolicyCommand>
{
    public DeleteShippingPolicyCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");

        RuleFor(x => x.PolicyId)
            .NotEmpty().WithMessage("Policy ID là bắt buộc");
    }
}

public sealed class DeleteShippingPolicyCommandHandler : ICommandHandler<DeleteShippingPolicyCommand>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IShippingPolicyRepository _shippingPolicyRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShippingPolicyCommandHandler(
        IStoreRepository storeRepository,
        IShippingPolicyRepository shippingPolicyRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _storeRepository = storeRepository;
        _shippingPolicyRepository = shippingPolicyRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteShippingPolicyCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return StoreErrors.Unauthorized;

        var storeId = StoreId.From(Guid.Parse(request.StoreId));
        var store = await _storeRepository.GetByIdAsync(storeId, cancellationToken);

        if (store is null)
            return StoreErrors.NotFound;

        // Check ownership
        if (store.UserId.Value.ToString() != userId)
            return StoreErrors.Unauthorized;

        // Get policy
        var policy = await _shippingPolicyRepository.GetByIdAsync(request.PolicyId, cancellationToken);
        if (policy is null)
            return Domain.Policies.Errors.PolicyErrors.NotFound;

        // Verify policy belongs to store
        if (policy.StoreId.Value != storeId.Value)
            return Domain.Policies.Errors.PolicyErrors.Unauthorized;

        // Delete policy
        _shippingPolicyRepository.Remove(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

