using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.Commands;

public sealed record SubmitBusinessVerificationCommand(
    string BusinessName,
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
) : ICommand;

public sealed class SubmitBusinessVerificationCommandValidator : AbstractValidator<SubmitBusinessVerificationCommand>
{
    public SubmitBusinessVerificationCommandValidator()
    {
        RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(300);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(100);
    }
}

public sealed class SubmitBusinessVerificationCommandHandler : ICommandHandler<SubmitBusinessVerificationCommand>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitBusinessVerificationCommandHandler(
        IUserContext userContext,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SubmitBusinessVerificationCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(Guid.Parse(_userContext.UserId!));
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        if (user.IsBusinessVerified)
            return UserErrors.BusinessAlreadyVerified;

        var addressOrError = BusinessAddress.Create(
            request.Street, request.City, request.State, request.ZipCode, request.Country);
        if (addressOrError.IsFailure)
            return addressOrError.Error;

        user.SetBusinessInfo(request.BusinessName, addressOrError.Value);
        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
