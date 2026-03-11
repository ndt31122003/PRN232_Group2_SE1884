using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Stores.Commands;

public sealed record CreateStoreCommand(
    string Name,
    string? Description,
    StoreType StoreType
) : ICommand<CreateStoreCommandResult>;

public sealed record CreateStoreCommandResult(
    Guid StoreId,
    string Name,
    string Slug,
    StoreType StoreType
);

public sealed class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên cửa hàng là bắt buộc")
            .MaximumLength(255).WithMessage("Tên cửa hàng không được vượt quá 255 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự");

        RuleFor(x => x.StoreType)
            .IsInEnum().WithMessage("Loại cửa hàng không hợp lệ");
    }
}

public sealed class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, CreateStoreCommandResult>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStoreCommandHandler(
        IStoreRepository storeRepository,
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _storeRepository = storeRepository;
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateStoreCommandResult>> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return StoreErrors.Unauthorized;

        var user = await _userRepository.GetByIdAsync(new UserId(Guid.Parse(userId)), cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        if (!user.IsSellerVerified)
            return UserErrors.SellerNotVerified;

        // Check if slug already exists
        var tempSlug = GenerateSlug(request.Name);
        var slugExists = await _storeRepository.SlugExistsAsync(tempSlug, cancellationToken);
        if (slugExists)
        {
            tempSlug = $"{tempSlug}-{Guid.NewGuid().ToString("N")[..8]}";
        }

        // Create store
        var storeResult = Domain.Stores.Entities.Store.Create(
            new UserId(Guid.Parse(userId)),
            request.Name,
            request.StoreType,
            request.Description);

        if (storeResult.IsFailure)
            return storeResult.Error;

        var store = storeResult.Value;

        // Create initial subscription
        var subscriptionFees = GetFeesForStoreType(request.StoreType);
        var subscription = Domain.Stores.Entities.StoreSubscription.Create(
            store.Id,
            request.StoreType,
            subscriptionFees);

        store.AddSubscription(subscription);

        _storeRepository.Add(store);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateStoreCommandResult(
            store.Id.Value,
            store.Name,
            store.Slug,
            store.StoreType);
    }

    private static string GenerateSlug(string name)
    {
        return name.Trim()
            .Replace(" ", "-")
            .Replace("_", "-")
            .ToLowerInvariant();
    }

    private static SubscriptionFees GetFeesForStoreType(StoreType storeType)
    {
        return storeType switch
        {
            StoreType.Basic => SubscriptionFees.ForBasic(),
            StoreType.Premium => SubscriptionFees.ForPremium(),
            StoreType.Anchor => SubscriptionFees.ForAnchor(),
            _ => throw new ArgumentOutOfRangeException(nameof(storeType), storeType, "Invalid store type")
        };
    }
}

