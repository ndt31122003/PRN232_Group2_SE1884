using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Roles.Errors;

public static class PermissionErrors
{
    public static Error InvalidPermission(string value) => Error.Failure(
        "Permission.InvalidPermission",
        $"Quyền '{value}' không hợp lệ");
}
