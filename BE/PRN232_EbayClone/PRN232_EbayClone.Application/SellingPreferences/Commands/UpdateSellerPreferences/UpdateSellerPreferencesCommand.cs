using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.SellingPreferences.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Commands.UpdateSellerPreferences;

public sealed record UpdateSellerPreferencesCommand(
    Guid UserId,
    bool? ListingsStayActiveWhenOutOfStock,
    bool? ShowExactQuantityAvailable,
    InvoicePreferencePayload? Invoice,
    bool? BuyersCanSeeVatNumber,
    string? VatNumber) : ICommand<SellerPreferenceDto>;

public sealed record InvoicePreferencePayload
{
    public InvoiceFormat? Format { get; init; }
    public bool? SendEmailCopy { get; init; }
    public bool? ApplyCreditsAutomatically { get; init; }
}

public sealed class UpdateSellerPreferencesCommandValidator : AbstractValidator<UpdateSellerPreferencesCommand>
{
    public UpdateSellerPreferencesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        When(x => x.Invoice is not null && x.Invoice.Format.HasValue, () =>
        {
            RuleFor(x => x.Invoice!.Format!.Value)
                .IsInEnum()
                .WithMessage("Invoice format is invalid.");
        });

        When(x => x.VatNumber is not null, () =>
        {
            RuleFor(x => x.VatNumber)
                .MaximumLength(SellerPreference.MaxBuyerIdentifierLength)
                .WithMessage($"VAT number cannot exceed {SellerPreference.MaxBuyerIdentifierLength} characters.");
        });
    }
}

public sealed class UpdateSellerPreferencesCommandHandler : ICommandHandler<UpdateSellerPreferencesCommand, SellerPreferenceDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSellerPreferencesCommandHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SellerPreferenceDto>> Handle(UpdateSellerPreferencesCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<SellerPreferenceDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

        var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
        var isNew = false;
        if (preference is null)
        {
            preference = SellerPreference.CreateDefault(userId);
            _sellerPreferenceRepository.Add(preference);
            isNew = true;
        }

        if (request.ListingsStayActiveWhenOutOfStock.HasValue || request.ShowExactQuantityAvailable.HasValue)
        {
            var listingsStayActive = request.ListingsStayActiveWhenOutOfStock ?? preference.ListingsStayActiveWhenOutOfStock;
            var showExactQuantity = request.ShowExactQuantityAvailable ?? preference.ShowExactQuantityAvailable;
            preference.UpdateMultiQuantitySettings(listingsStayActive, showExactQuantity);
        }

        if (request.Invoice is not null)
        {
            var invoiceFormat = request.Invoice.Format ?? preference.InvoicePreference.Format;
            var sendEmailCopy = request.Invoice.SendEmailCopy ?? preference.InvoicePreference.SendEmailCopy;
            var applyCredits = request.Invoice.ApplyCreditsAutomatically ?? preference.InvoicePreference.ApplyCreditsAutomatically;

            var invoiceResult = preference.UpdateInvoicePreferences(invoiceFormat, sendEmailCopy, applyCredits);
            if (invoiceResult.IsFailure)
            {
                return Result.Failure<SellerPreferenceDto>(invoiceResult.Error);
            }
        }

        if (request.BuyersCanSeeVatNumber.HasValue || request.VatNumber is not null)
        {
            var visible = request.BuyersCanSeeVatNumber ?? preference.BuyersCanSeeVatNumber;
            preference.UpdateVatSettings(visible, request.VatNumber ?? preference.VatNumber);
        }

        if (!isNew)
        {
            _sellerPreferenceRepository.Update(preference);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(preference.ToDto());
    }
}
