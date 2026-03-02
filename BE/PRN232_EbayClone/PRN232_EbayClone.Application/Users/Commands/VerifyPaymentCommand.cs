using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Users.Commands;

public sealed record VerifyPaymentCommand(
    string Code,
    string CardHolderName,
    string CardNumber,
    string ExpiryMonth,
    string ExpiryYear,
    string Cvv
) : ICommand<PaymentVerificationReceipt>;

public sealed class VerifyPaymentCommandValidator : AbstractValidator<VerifyPaymentCommand>
{
    public VerifyPaymentCommandValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty().WithMessage("Vui lòng nhập mã xác minh thanh toán.")
            .Length(6).WithMessage("Mã xác minh thanh toán gồm 6 ký tự.");

        RuleFor(command => command.CardHolderName)
            .NotEmpty().WithMessage("Vui lòng nhập tên chủ thẻ.")
            .MaximumLength(100).WithMessage("Tên chủ thẻ tối đa 100 ký tự.");

        RuleFor(command => command.CardNumber)
            .NotEmpty().WithMessage("Vui lòng nhập số thẻ tín dụng.")
            .MinimumLength(12).WithMessage("Số thẻ tối thiểu 12 chữ số.")
            .MaximumLength(23).WithMessage("Số thẻ tối đa 23 ký tự bao gồm dấu cách.");

        RuleFor(command => command.ExpiryMonth)
            .NotEmpty().WithMessage("Vui lòng nhập tháng hết hạn của thẻ.");

        RuleFor(command => command.ExpiryYear)
            .NotEmpty().WithMessage("Vui lòng nhập năm hết hạn của thẻ.");

        RuleFor(command => command.Cvv)
            .NotEmpty().WithMessage("Vui lòng nhập mã CVV.")
            .MinimumLength(3).WithMessage("CVV phải có ít nhất 3 chữ số.")
            .MaximumLength(4).WithMessage("CVV tối đa 4 chữ số.");
    }
}

public sealed class VerifyPaymentCommandHandler : ICommandHandler<VerifyPaymentCommand, PaymentVerificationReceipt>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IOtpRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyPaymentCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaymentVerificationReceipt>> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return UserErrors.Unauthorized;
        }

        if (!Guid.TryParse(userId, out var sellerGuid))
        {
            return UserErrors.Unauthorized;
        }

        var seller = await _userRepository.GetByIdAsync(new UserId(sellerGuid), cancellationToken);
        if (seller is null)
        {
            return UserErrors.NotFound;
        }

        if (seller.IsPaymentVerified)
        {
            return UserErrors.PaymentAlreadyVerified;
        }

        if (!seller.IsEmailVerified)
        {
            return UserErrors.EmailNotVerified;
        }

        var cardResult = PaymentCardValidator.Inspect(
            request.CardHolderName,
            request.CardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Cvv);

        if (cardResult.IsFailure)
        {
            return cardResult.Error;
        }

        var otp = await _otpRepository.GetByEmailAndCodeAndTypeAsync(
            seller.Email,
            request.Code,
            OtpType.VerifyPayment,
            cancellationToken);

        if (otp is null || !otp.IsValid())
        {
            return IdentityErrors.InvalidOtp;
        }

        otp.MarkAsUsed();
        seller.VerifyPayment();

        _otpRepository.Update(otp);
        _userRepository.Update(seller);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var reference = $"SIM-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        var card = cardResult.Value;

        return Result.Success(new PaymentVerificationReceipt(
            reference,
            DateTime.UtcNow,
            card.CardholderName,
            card.CardBrand,
            card.CardLast4,
            card.MaskedCardNumber,
            card.ExpiryMonth,
            card.ExpiryYear));
    }
}

public sealed record PaymentVerificationReceipt(
    string ProviderReference,
    DateTime VerifiedOnUtc,
    string CardholderName,
    string CardBrand,
    string CardLast4,
    string MaskedCardNumber,
    int ExpiryMonth,
    int ExpiryYear);
