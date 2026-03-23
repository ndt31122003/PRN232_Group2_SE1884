using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Security;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.Services;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string FullName,
    string Password,
    string? CaptchaToken = null,
    string? CaptchaAction = null
) : ICommand<LoginCommandResult>;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage($"Email không hợp lệ");
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(100);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, LoginCommandResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailUniquenessChecker _emailUniquenessChecker;
    private readonly ICaptchaProtectionService _captchaProtectionService;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IEmailUniquenessChecker emailUniquenessChecker,
        ICaptchaProtectionService captchaProtectionService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _emailUniquenessChecker = emailUniquenessChecker;
        _captchaProtectionService = captchaProtectionService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginCommandResult>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var captchaResult = await _captchaProtectionService.EnsureValidAsync(
            CaptchaActions.IdentityRegister,
            request.CaptchaToken,
            request.CaptchaAction,
            request.Email,
            cancellationToken);

        if (captchaResult.IsFailure)
        {
            return captchaResult.Error;
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var userOrError = await User.RegisterAsync(
            _emailUniquenessChecker,
            request.FullName,
            request.Email,
            passwordHash);

        if (userOrError.IsFailure)
            return userOrError.Error;

        var user = userOrError.Value;
        _userRepository.Add(user);

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = RefreshToken.Create(user.Id, _tokenProvider.GenerateRefreshToken());
        _refreshTokenRepository.Add(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginCommandResult(accessToken, refreshToken.Token);
    }
}

