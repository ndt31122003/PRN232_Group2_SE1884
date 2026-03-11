using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Policies.Errors;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Policies.Commands;

public sealed record CreateShippingPolicyCommand(
    string StoreId,
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays,
    bool IsDefault
) : ICommand<CreateShippingPolicyCommandResult>;

public sealed record CreateShippingPolicyCommandResult(
    Guid PolicyId,
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays
);

public sealed class CreateShippingPolicyCommandValidator : AbstractValidator<CreateShippingPolicyCommand>
{
    public CreateShippingPolicyCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");

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

public sealed class CreateShippingPolicyCommandHandler : ICommandHandler<CreateShippingPolicyCommand, CreateShippingPolicyCommandResult>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IShippingPolicyRepository _shippingPolicyRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShippingPolicyCommandHandler(
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

    public async Task<Result<CreateShippingPolicyCommandResult>> Handle(CreateShippingPolicyCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return StoreErrors.Unauthorized;

        var storeId = new StoreId(Guid.Parse(request.StoreId));
        var store = await _storeRepository.GetByIdAsync(storeId, cancellationToken);

        if (store is null)
            return StoreErrors.NotFound;

        // Check ownership
        if (store.UserId.Value.ToString() != userId)
            return StoreErrors.Unauthorized;

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
                if (existingPolicy.IsDefault)
                {
                    existingPolicy.Update(existingPolicy.Carrier, existingPolicy.ServiceName, existingPolicy.Cost, existingPolicy.HandlingTimeDays, false);
                    _shippingPolicyRepository.Update(existingPolicy);
                }
            }
        }

        // Create shipping policy
        var policyResult = ShippingPolicy.Create(
            storeId,
            request.Carrier,
            request.ServiceName,
            costResult.Value,
            request.HandlingTimeDays,
            request.IsDefault);

        if (policyResult.IsFailure)
            return policyResult.Error;

        var newPolicy = policyResult.Value;

        _shippingPolicyRepository.Add(newPolicy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateShippingPolicyCommandResult(
            newPolicy.Id,
            newPolicy.Carrier,
            newPolicy.ServiceName,
            newPolicy.Cost.Amount,
            newPolicy.Cost.Currency,
            newPolicy.HandlingTimeDays);
    }
}

