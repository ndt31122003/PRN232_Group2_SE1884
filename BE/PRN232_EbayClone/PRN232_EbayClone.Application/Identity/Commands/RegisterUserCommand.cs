using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.Services;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string FullName,
    string Password
) : ICommand;

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

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailUniquenessChecker _emailUniquenessChecker;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailUniquenessChecker emailUniquenessChecker,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailUniquenessChecker = emailUniquenessChecker;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.Hash(request.Password);

        var userOrError = await User.RegisterAsync(
            _emailUniquenessChecker,
            request.FullName,
            request.Email,
            passwordHash);

        if (userOrError.IsFailure)
            return userOrError.Error;

        _userRepository.Add(userOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

