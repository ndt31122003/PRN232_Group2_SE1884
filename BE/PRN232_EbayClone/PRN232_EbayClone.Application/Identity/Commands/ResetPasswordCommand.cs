using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Security;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record ResetPasswordCommand(
    string Email,
    string Code,
    string NewPassword,
    string? CaptchaToken = null,
    string? CaptchaAction = null
) : ICommand;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("OTP code is required.");
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8).WithMessage("New password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("New password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character.");
    }
}

public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IOtpRepository _otpRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICaptchaProtectionService _captchaProtectionService;

    public ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IOtpRepository otpRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ICaptchaProtectionService captchaProtectionService)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _captchaProtectionService = captchaProtectionService;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var captchaResult = await _captchaProtectionService.EnsureValidAsync(
            CaptchaActions.IdentityResetPassword,
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

        var otp = await _otpRepository.GetByEmailAndCodeAndTypeAsync(
            emailOrError.Value,
            request.Code,
            OtpType.ResetPassword,
            cancellationToken);

        if (otp is null || !otp.IsValid())
            return IdentityErrors.InvalidOtp;

        var newHashedPassword = _passwordHasher.Hash(request.NewPassword);

        var user = await _userRepository.GetByEmailAsync(emailOrError.Value, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        user.UpdatePassword(newHashedPassword);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
