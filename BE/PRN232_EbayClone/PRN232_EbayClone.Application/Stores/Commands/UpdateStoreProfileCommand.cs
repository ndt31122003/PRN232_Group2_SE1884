using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Stores.Commands;

public sealed record UpdateStoreProfileCommand(
    string StoreId,
    string Name,
    string? Description = null,
    string? LogoUrl = null,
    string? BannerUrl = null
) : ICommand;

public sealed class UpdateStoreProfileCommandValidator : AbstractValidator<UpdateStoreProfileCommand>
{
    public UpdateStoreProfileCommandValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên cửa hàng là bắt buộc")
            .MaximumLength(255).WithMessage("Tên cửa hàng không được vượt quá 255 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Mô tả không được vượt quá 2000 ký tự");

        RuleFor(x => x.LogoUrl)
            .MaximumLength(500).WithMessage("URL logo không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));

        RuleFor(x => x.BannerUrl)
            .MaximumLength(500).WithMessage("URL banner không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.BannerUrl));
    }
}

public sealed class UpdateStoreProfileCommandHandler : ICommandHandler<UpdateStoreProfileCommand>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStoreProfileCommandHandler(
        IStoreRepository storeRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _storeRepository = storeRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateStoreProfileCommand request, CancellationToken cancellationToken)
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

        var updateResult = store.UpdateProfile(
            request.Name,
            request.Description,
            request.LogoUrl,
            request.BannerUrl);

        if (updateResult.IsFailure)
            return updateResult.Error;

        _storeRepository.Update(store);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

