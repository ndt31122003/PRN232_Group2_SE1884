using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Roles.Enums;

namespace PRN232_EbayClone.Application.Roles.Commands;

public sealed record CreateRoleCommand(
    string Name,
    string Description,
    List<Permission> Permissions
) : ICommand;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name must not be empty.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Role description must not exceed 250 characters.");
    }
}

public sealed class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleOrError = Role.Create(
            request.Name,
            request.Description,
            request.Permissions);
        if (roleOrError.IsFailure)
            return roleOrError.Error;

        _roleRepository.Add(roleOrError.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}