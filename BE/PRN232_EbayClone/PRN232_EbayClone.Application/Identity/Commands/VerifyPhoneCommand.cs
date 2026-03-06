using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record VerifyPhoneCommand(
    string Code
) : ICommand;

public sealed class VerifyPhoneCommandValidator : AbstractValidator<VerifyPhoneCommand>
{
    public VerifyPhoneCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .WithMessage("OTP code must be 6 digits.");
    }
}

public sealed class VerifyPhoneCommandHandler : ICommandHandler<VerifyPhoneCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IOtpRepository _otpRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyPhoneCommandHandler(
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

    public async Task<Result> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(Guid.Parse(_userContext.UserId!));
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        if (user.IsPhoneVerified)
            return UserErrors.PhoneAlreadyVerified;

        var otp = await _otpRepository.GetByEmailAndCodeAndTypeAsync(
            user.Email, request.Code, OtpType.VerifyPhone, cancellationToken);
        if (otp is null)
            return IdentityErrors.InvalidOtp;

        if (!otp.IsValid())
            return IdentityErrors.InvalidOtp;

        otp.MarkAsUsed();
        user.VerifyPhone();

        _otpRepository.Update(otp);
        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
