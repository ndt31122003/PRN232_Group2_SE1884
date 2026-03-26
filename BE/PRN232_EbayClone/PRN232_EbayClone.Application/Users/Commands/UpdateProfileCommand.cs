using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Users.Commands;

public sealed record UpdateProfileCommand(
    string FullName,
    string DisplayName
) : ICommand;

public sealed class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var guid))
            return Error.Unauthorized("Not authenticated.");

        var user = await _userRepository.GetByIdAsync(new UserId(guid), cancellationToken);
        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        user.UpdateProfile(request.FullName, request.DisplayName);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
