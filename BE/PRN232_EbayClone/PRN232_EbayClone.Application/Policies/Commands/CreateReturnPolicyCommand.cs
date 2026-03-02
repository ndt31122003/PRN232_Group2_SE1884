using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Policies.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Policies.Commands;

public sealed record CreateReturnPolicyCommand(
    string StoreId,
    bool AcceptReturns,
    ReturnPeriod? ReturnPeriodDays,
    RefundMethod? RefundMethod,
    ReturnShippingPaidBy? ReturnShippingPaidBy
) : ICommand;

public sealed class CreateReturnPolicyCommandValidator : AbstractValidator<CreateReturnPolicyCommand>
{
    public CreateReturnPolicyCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");
    }
}

public sealed class CreateReturnPolicyCommandHandler : ICommandHandler<CreateReturnPolicyCommand>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IReturnPolicyRepository _returnPolicyRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReturnPolicyCommandHandler(
        IStoreRepository storeRepository,
        IReturnPolicyRepository returnPolicyRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _storeRepository = storeRepository;
        _returnPolicyRepository = returnPolicyRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateReturnPolicyCommand request, CancellationToken cancellationToken)
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

        // Check if return policy already exists
        var existingPolicy = await _returnPolicyRepository.GetByStoreIdAsync(storeId, cancellationToken);
        if (existingPolicy is not null)
            return Domain.Policies.Errors.PolicyErrors.AlreadyExists;

        // Create return policy
        var policyResult = ReturnPolicy.Create(
            storeId,
            request.AcceptReturns,
            request.ReturnPeriodDays,
            request.RefundMethod,
            request.ReturnShippingPaidBy);

        if (policyResult.IsFailure)
            return policyResult.Error;

        var newPolicy = policyResult.Value;

        _returnPolicyRepository.Add(newPolicy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

