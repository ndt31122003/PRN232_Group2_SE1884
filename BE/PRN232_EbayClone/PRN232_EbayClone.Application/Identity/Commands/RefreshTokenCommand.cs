using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Errors;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record RefreshTokenCommand(
    string RefreshToken
) : ICommand<RefreshTokenCommandResult>;

public sealed record RefreshTokenCommandResult(
    string AccessToken,
    string RefreshToken);

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.")
            .NotNull().WithMessage("Refresh token must not be null.");
    }
}

public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandResult>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RefreshTokenCommandHandler(
        ITokenProvider tokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenProvider = tokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<RefreshTokenCommandResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(
            request.RefreshToken,
            cancellationToken);

        if (existingRefreshToken is null)
            return IdentityErrors.InvalidRefreshToken;

        if (existingRefreshToken.IsExpired())
        {
            _refreshTokenRepository.Remove(existingRefreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return IdentityErrors.InvalidRefreshToken;
        }

        var user = await _userRepository.GetByIdAsync(
            existingRefreshToken.UserId,
            cancellationToken);

        if (user is null)
            return IdentityErrors.InvalidRefreshToken;

        if (user.IsDeleted || !user.IsEmailVerified)
        {
            _refreshTokenRepository.Remove(existingRefreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return IdentityErrors.InvalidRefreshToken;
        }

        var newAccessToken = _tokenProvider.GenerateAccessToken(user);
        _refreshTokenRepository.Remove(existingRefreshToken);

        var newRefreshToken = RefreshToken.Create(user.Id, _tokenProvider.GenerateRefreshToken());
        _refreshTokenRepository.Add(newRefreshToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenCommandResult(newAccessToken, newRefreshToken.Token);
    }
}