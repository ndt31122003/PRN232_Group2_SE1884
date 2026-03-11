using System;
using Microsoft.Extensions.Configuration;
using PRN232_EbayClone.Application.Abstractions.Identity;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record ResendOtpCommand(
    string Email,
    OtpType Type
) : ICommand<OtpDeliveryResult>;

public sealed class ResendOtpCommandHandler : ICommandHandler<ResendOtpCommand, OtpDeliveryResult>
{
    private readonly IOtpRepository _otpRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOtpGenerator _otpGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly bool _isSmsSimulation;

    public ResendOtpCommandHandler(
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IOtpGenerator otpGenerator,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _otpGenerator = otpGenerator;
        _unitOfWork = unitOfWork;
        var provider = configuration.GetValue<string>("Sms:Provider")?.ToLowerInvariant();
        _isSmsSimulation = string.IsNullOrEmpty(provider) || provider == "dev";
    }

    public async Task<Result<OtpDeliveryResult>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
    {
        var emailOrError = Email.Create(request.Email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        var user = await _userRepository.GetByEmailAsync(emailOrError.Value, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;
        if (request.Type == OtpType.VerifyEmail && user.IsEmailVerified)
            return UserErrors.EmailAlreadyVerified;
        if (request.Type == OtpType.VerifyPayment && !user.IsEmailVerified)
            return UserErrors.EmailNotVerified;
        if (request.Type == OtpType.VerifyPayment && user.IsPaymentVerified)
            return UserErrors.PaymentAlreadyVerified;
        if (request.Type == OtpType.VerifyPhone && user.IsPhoneVerified)
            return UserErrors.PhoneAlreadyVerified;

        var isSimulation = request.Type is OtpType.VerifyEmail or OtpType.VerifyPayment
                           || (request.Type == OtpType.VerifyPhone && _isSmsSimulation);

        var otp = await _otpRepository.GetByEmailAndTypeAsync(
            emailOrError.Value,
            request.Type,
            cancellationToken);

        if (otp is null)
        {
            var generatedCode = _otpGenerator.GenerateOtp();
            otp = Otp.Create(
                 emailOrError.Value,
                 generatedCode,
                 request.Type);

            _otpRepository.Add(otp);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new OtpDeliveryResult(emailOrError.Value.Value, request.Type, generatedCode, otp.ExpiresOnUtc, isSimulation));
        }

        if (otp.IsValid())
        {
            return Result.Success(new OtpDeliveryResult(emailOrError.Value.Value, request.Type, otp.Code, otp.ExpiresOnUtc, isSimulation));
        }

        var refreshedCode = _otpGenerator.GenerateOtp();
        otp.ResendOtp(refreshedCode, request.Type);

        _otpRepository.Update(otp);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new OtpDeliveryResult(emailOrError.Value.Value, request.Type, refreshedCode, otp.ExpiresOnUtc, isSimulation));
    }
}

public sealed record OtpDeliveryResult(string Email, OtpType Type, string Code, DateTime ExpiresOnUtc, bool IsSimulation);
