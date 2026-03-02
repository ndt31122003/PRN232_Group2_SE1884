using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.Services;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record ExternalLoginCommand(
    string Email,
    string FullName
) : ICommand<LoginCommandResult>;

public sealed class ExternalLoginCommandHandler : ICommandHandler<ExternalLoginCommand, LoginCommandResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailUniquenessChecker _emailUniquenessChecker;
    private readonly IUnitOfWork _unitOfWork;

    public ExternalLoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IEmailUniquenessChecker emailUniquenessChecker,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _emailUniquenessChecker = emailUniquenessChecker;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginCommandResult>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var emailOrError = Email.Create(request.Email);
        if (emailOrError.IsFailure)
        {
            return IdentityErrors.InvalidCredentials;
        }

        var email = emailOrError.Value;
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            var passwordHash = _passwordHasher.Hash(Guid.NewGuid().ToString("N"));
            var fullName = string.IsNullOrWhiteSpace(request.FullName) ? email.Value : request.FullName;
            var createResult = await User.CreateAsync(
                _emailUniquenessChecker,
                fullName,
                email.Value,
                passwordHash,
                []);

            if (createResult.IsFailure)
                return createResult.Error;

            user = createResult.Value;
            user.VerifyEmail();
            _userRepository.Add(user);
        }
        else
        {
            if (user.IsDeleted)
                return IdentityErrors.InvalidCredentials;

            if (!user.IsEmailVerified)
                user.VerifyEmail();
        }

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = RefreshToken.Create(user.Id, _tokenProvider.GenerateRefreshToken());

        _refreshTokenRepository.Add(refreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginCommandResult(accessToken, refreshToken.Token);
    }
}
