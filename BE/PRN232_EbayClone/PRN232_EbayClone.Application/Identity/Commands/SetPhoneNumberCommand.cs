using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Identity;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record SetPhoneNumberCommand(
    string PhoneNumber
) : ICommand<OtpDeliveryResult>;

public sealed class SetPhoneNumberCommandValidator : AbstractValidator<SetPhoneNumberCommand>
{
    public SetPhoneNumberCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{6,14}$")
            .WithMessage("Phone number must be in E.164 format (e.g. +84912345678).");
    }
}

public sealed class SetPhoneNumberCommandHandler : ICommandHandler<SetPhoneNumberCommand, OtpDeliveryResult>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IOtpRepository _otpRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly bool _isSmsSimulation;

    public SetPhoneNumberCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IOtpGenerator otpGenerator,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _otpGenerator = otpGenerator;
        _unitOfWork = unitOfWork;
        var provider = configuration.GetValue<string>("Sms:Provider")?.ToLowerInvariant();
        _isSmsSimulation = string.IsNullOrEmpty(provider) || provider == "dev";
    }

    public async Task<Result<OtpDeliveryResult>> Handle(SetPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(Guid.Parse(_userContext.UserId!));
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        if (user.IsPhoneVerified)
            return UserErrors.PhoneAlreadyVerified;

        user.SetPhoneNumber(request.PhoneNumber);
        _userRepository.Update(user);

        var existingOtp = await _otpRepository.GetByEmailAndTypeAsync(
            user.Email, OtpType.VerifyPhone, cancellationToken);

        string code;
        Otp otp;

        if (existingOtp is null)
        {
            code = _otpGenerator.GenerateOtp();
            otp = Otp.Create(user.Email, code, OtpType.VerifyPhone);
            _otpRepository.Add(otp);
        }
        else if (existingOtp.IsValid())
        {
            code = existingOtp.Code;
            otp = existingOtp;
        }
        else
        {
            code = _otpGenerator.GenerateOtp();
            existingOtp.ResendOtp(code, OtpType.VerifyPhone);
            _otpRepository.Update(existingOtp);
            otp = existingOtp;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new OtpDeliveryResult(user.Email.Value, OtpType.VerifyPhone, code, otp.ExpiresOnUtc, _isSmsSimulation);
    }
}
