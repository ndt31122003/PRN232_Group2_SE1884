using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Policies.Commands;

public sealed record UpdateShippingPolicyCommand(
    string StoreId,
    Guid PolicyId,
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays,
    bool IsDefault
) : ICommand;

public sealed class UpdateShippingPolicyCommandValidator : AbstractValidator<UpdateShippingPolicyCommand>
{
    public UpdateShippingPolicyCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");

        RuleFor(x => x.PolicyId)
            .NotEmpty().WithMessage("Policy ID là bắt buộc");

        RuleFor(x => x.Carrier)
            .NotEmpty().WithMessage("Carrier là bắt buộc")
            .MaximumLength(100).WithMessage("Carrier không được vượt quá 100 ký tự");

        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("Service Name là bắt buộc")
            .MaximumLength(100).WithMessage("Service Name không được vượt quá 100 ký tự");

        RuleFor(x => x.CostAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Chi phí phải lớn hơn hoặc bằng 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Loại tiền tệ là bắt buộc")
            .Length(3).WithMessage("Loại tiền tệ phải có 3 ký tự");

        RuleFor(x => x.HandlingTimeDays)
            .InclusiveBetween(0, 30).WithMessage("Thời gian xử lý phải từ 0 đến 30 ngày");
    }
}

public sealed class UpdateShippingPolicyCommandHandler : ICommandHandler<UpdateShippingPolicyCommand>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IShippingPolicyRepository _shippingPolicyRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateShippingPolicyCommandHandler(
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

    public async Task<Result> Handle(UpdateShippingPolicyCommand request, CancellationToken cancellationToken)
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

        // Create money value object
        var costResult = Money.Create(request.CostAmount, request.Currency);
        if (costResult.IsFailure)
            return costResult.Error;

        // If setting as default, unset other defaults
        if (request.IsDefault)
        {
            var existingPolicies = await _shippingPolicyRepository.GetByStoreIdAsync(storeId, cancellationToken);
            foreach (var existingPolicy in existingPolicies)
            {
                if (existingPolicy.IsDefault && existingPolicy.Id != request.PolicyId)
                {
                    var unsetResult = existingPolicy.Update(
                        existingPolicy.Carrier,
                        existingPolicy.ServiceName,
                        existingPolicy.Cost,
                        existingPolicy.HandlingTimeDays,
                        false);
                    if (unsetResult.IsFailure)
                        return unsetResult.Error;
                    
                    _shippingPolicyRepository.Update(existingPolicy);
                }
            }
        }

        // Update policy
        var updateResult = policy.Update(
            request.Carrier,
            request.ServiceName,
            costResult.Value,
            request.HandlingTimeDays,
            request.IsDefault);

        if (updateResult.IsFailure)
            return updateResult.Error;

        _shippingPolicyRepository.Update(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

