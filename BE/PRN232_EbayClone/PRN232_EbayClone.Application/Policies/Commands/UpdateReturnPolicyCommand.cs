using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Policies.Commands;

public sealed record UpdateReturnPolicyCommand(
    string StoreId,
    bool AcceptReturns,
    ReturnPeriod? ReturnPeriodDays,
    RefundMethod? RefundMethod,
    ReturnShippingPaidBy? ReturnShippingPaidBy
) : ICommand;

public sealed class UpdateReturnPolicyCommandValidator : AbstractValidator<UpdateReturnPolicyCommand>
{
    public UpdateReturnPolicyCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");
    }
}

public sealed class UpdateReturnPolicyCommandHandler : ICommandHandler<UpdateReturnPolicyCommand>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IReturnPolicyRepository _returnPolicyRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReturnPolicyCommandHandler(
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

    public async Task<Result> Handle(UpdateReturnPolicyCommand request, CancellationToken cancellationToken)
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
        var policy = await _returnPolicyRepository.GetByStoreIdAsync(storeId, cancellationToken);
        if (policy is null)
            return Domain.Policies.Errors.PolicyErrors.NotFound;

        // Update policy
        var updateResult = policy.Update(
            request.AcceptReturns,
            request.ReturnPeriodDays,
            request.RefundMethod,
            request.ReturnShippingPaidBy);

        if (updateResult.IsFailure)
            return updateResult.Error;

        _returnPolicyRepository.Update(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

