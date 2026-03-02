using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Roles.Errors;
using PRN232_EbayClone.Domain.Roles.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Roles.Entities;

public sealed class Role : AggregateRoot<RoleId>
{
    private readonly List<RolePermission> _permissions = [];
    public IReadOnlyList<RolePermission> Permissions => _permissions.AsReadOnly();
    public string Name { get; private set; }
    public string Description { get; private set; }
    private Role(RoleId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }
    public static Result<Role> Create(
        string name,
        string description,
        List<Permission> permissions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        var role = new Role(
            RoleId.New(),
            name,
            description);

        foreach (var permission in permissions)
        {
            var result = role.AddPermission(permission);
            if (result.IsFailure)
                return result.Error;
        }

        return role;
    }

    public Result Update(
        string name,
        string description,
        List<Permission> permissions)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoleErrors.EmptyRoleName;

        Name = name;
        Description = description;
        ClearPermissions();
        foreach (var permission in permissions)
        {
            var result = AddPermission(permission);
            if (result.IsFailure)
                return result.Error;
        }
        return Result.Success();
    }

    public Result AddPermission(Permission permission)
    {
        var rolePermissionOrError = RolePermission.Create(permission);
        if (rolePermissionOrError.IsFailure)
            return rolePermissionOrError.Error;

        if (!_permissions.Contains(rolePermissionOrError.Value))
            _permissions.Add(rolePermissionOrError.Value);

        return Result.Success();
    }

    public Result RemovePermission(Permission permission)
    {
        var rolePermissionOrError = RolePermission.Create(permission);
        if (rolePermissionOrError.IsFailure)
            return rolePermissionOrError.Error;

        _permissions.Remove(rolePermissionOrError.Value);

        return Result.Success();
    }

    private void ClearPermissions()
    {
        _permissions.Clear();
    }
}
