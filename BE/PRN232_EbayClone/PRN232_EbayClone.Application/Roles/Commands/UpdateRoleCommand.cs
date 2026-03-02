using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Roles.Errors;
using PRN232_EbayClone.Domain.Roles.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Roles.Commands;

public sealed record UpdateRoleCommand(
    string RoleId,
    string Name,
    string Description,
    List<Permission> Permissions
) : ICommand;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID must not be empty.")
            .Must(roleId => Guid.TryParse(roleId, out _))
            .WithMessage("Invalid Role ID format.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name must not be empty.")
            .MaximumLength(100).WithMessage("Role name must not exceed 100 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Role description must not exceed 250 characters.");
    }
}

public sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var roleId = new RoleId(Guid.Parse(request.RoleId));

        var role = await _roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
            return RoleErrors.NotFound;

        var updateResult = role.Update(
            request.Name,
            request.Description,
            request.Permissions);
        if (updateResult.IsFailure)
            return updateResult.Error;

        _roleRepository.Update(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
