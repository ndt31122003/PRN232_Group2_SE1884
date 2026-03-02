using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Commands.UpdateBuyerManagement;

public sealed record UpdateBuyerManagementCommand(
    Guid UserId,
    bool BlockUnpaidItemStrikes,
    int UnpaidItemStrikesCount,
    int UnpaidItemStrikesPeriodInMonths,
    bool BlockPrimaryAddressOutsideShippingLocation,
    bool BlockMaxItemsInLastTenDays,
    int? MaxItemsInLastTenDays,
    bool ApplyFeedbackScoreThreshold,
    int? FeedbackScoreThreshold,
    bool UpdateBlockSettingsForActiveListings,
    bool RequirePaymentMethodBeforeBid,
    bool RequirePaymentMethodBeforeOffer,
    bool PreventBlockedBuyersFromContacting) : ICommand<BuyerManagementDto>;

public sealed class UpdateBuyerManagementCommandValidator : AbstractValidator<UpdateBuyerManagementCommand>
{
    public UpdateBuyerManagementCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        When(x => x.BlockUnpaidItemStrikes, () =>
        {
            RuleFor(x => x.UnpaidItemStrikesCount)
                .InclusiveBetween(1, 4);

            RuleFor(x => x.UnpaidItemStrikesPeriodInMonths)
                .InclusiveBetween(1, 12);
        });

        When(x => x.BlockMaxItemsInLastTenDays, () =>
        {
            RuleFor(x => x.MaxItemsInLastTenDays)
                .NotNull()
                .InclusiveBetween(1, 25);
        });

        When(x => x.ApplyFeedbackScoreThreshold, () =>
        {
            RuleFor(x => x.FeedbackScoreThreshold)
                .NotNull()
                .InclusiveBetween(0, 1000);
        });
    }
}

public sealed class UpdateBuyerManagementCommandHandler : ICommandHandler<UpdateBuyerManagementCommand, BuyerManagementDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBuyerManagementCommandHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BuyerManagementDto>> Handle(UpdateBuyerManagementCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<BuyerManagementDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

        var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
        var isNew = false;
        if (preference is null)
        {
            preference = SellerPreference.CreateDefault(userId);
            _sellerPreferenceRepository.Add(preference);
            isNew = true;
        }

        var result = preference.UpdateBuyerManagement(
            request.BlockUnpaidItemStrikes,
            request.UnpaidItemStrikesCount,
            request.UnpaidItemStrikesPeriodInMonths,
            request.BlockPrimaryAddressOutsideShippingLocation,
            request.BlockMaxItemsInLastTenDays,
            request.MaxItemsInLastTenDays,
            request.ApplyFeedbackScoreThreshold,
            request.FeedbackScoreThreshold,
            request.UpdateBlockSettingsForActiveListings,
            request.RequirePaymentMethodBeforeBid,
            request.RequirePaymentMethodBeforeOffer,
            request.PreventBlockedBuyersFromContacting);

        if (result.IsFailure)
        {
            return Result.Failure<BuyerManagementDto>(result.Error);
        }

        if (!isNew)
        {
            _sellerPreferenceRepository.Update(preference);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(preference.BuyerManagement.ToDto());
    }
}
