using PRN232_EbayClone.Application.Abstractions.Identity;
using PRN232_EbayClone.Application.Abstractions.Security;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record ForgotPasswordCommand(
    string Email,
    string? CaptchaToken = null,
    string? CaptchaAction = null
) : ICommand;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IOtpRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpGenerator _otpGenerator;
    private readonly ICaptchaProtectionService _captchaProtectionService;

    public ForgotPasswordCommandHandler(
        IOtpGenerator otpGenerator,
        IUnitOfWork unitOfWork,
        IOtpRepository otpRepository,
        ICaptchaProtectionService captchaProtectionService)
    {
        _otpGenerator = otpGenerator;
        _unitOfWork = unitOfWork;
        _otpRepository = otpRepository;
        _captchaProtectionService = captchaProtectionService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var captchaResult = await _captchaProtectionService.EnsureValidAsync(
            CaptchaActions.IdentityForgotPassword,
            request.CaptchaToken,
            request.CaptchaAction,
            request.Email,
            cancellationToken);

        if (captchaResult.IsFailure)
        {
            return captchaResult;
        }

        var emailOrError = Email.Create(request.Email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        var code = _otpGenerator.GenerateOtp();

        var existingOtp = await _otpRepository.GetByEmailAndTypeAsync(
            emailOrError.Value,
            OtpType.ResetPassword,
            cancellationToken);

        if (existingOtp is not null && existingOtp.IsValid())
            return IdentityErrors.OtpAlreadySent;

        var otp = Otp.Create(
            emailOrError.Value,
            code,
            OtpType.ResetPassword);

        _otpRepository.Add(otp);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
