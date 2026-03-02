using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Shared.Utilities;

namespace PRN232_EbayClone.Application.Roles.Queries;

public sealed record GetAllPermissionsQuery() : IQuery<GetAllPermissionsQueryResult>;
public sealed record PermissionResponse(string Value, string Description);
public sealed record GetAllPermissionsQueryResult(List<PermissionResponse> Permissions);

public sealed class GetAllPermissionsQueryHandler :
    IQueryHandler<GetAllPermissionsQuery, GetAllPermissionsQueryResult>
{
    public Task<Result<GetAllPermissionsQueryResult>> Handle(
        GetAllPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = Enum.GetValues<Permission>()
            .Select(p => new PermissionResponse(p.ToString(), p.GetDescription()))
            .ToList();
        var result = new GetAllPermissionsQueryResult(permissions);
        return Task.FromResult(Result.Success(result));
    }
}