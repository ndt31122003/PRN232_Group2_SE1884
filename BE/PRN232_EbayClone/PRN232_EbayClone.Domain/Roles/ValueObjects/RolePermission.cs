using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Roles.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Roles.ValueObjects;

public sealed record RolePermission
{
    public Permission Permission { get; }

    private RolePermission(Permission permission)
    {
        Permission = permission;
    }

    public static Result<RolePermission> Create(Permission value)
    {
        return new RolePermission(value);
    }

    public static RolePermission FromEnum(Permission permission) => new(permission);
    public override string ToString() => Permission.ToString();
}

