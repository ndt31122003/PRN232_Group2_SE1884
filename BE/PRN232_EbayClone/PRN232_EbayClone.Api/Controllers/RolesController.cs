using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Roles.Commands;
using PRN232_EbayClone.Application.Roles.Queries;
using PRN232_EbayClone.Domain.Roles.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/roles")]
public class RolesController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [Authorize(Policy = nameof(Permission.CreateRole))]
    public Task<IActionResult> Create(CreateRoleCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPut]
    public Task<IActionResult> Update(UpdateRoleCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpDelete("{id}")]
    public Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        => SendAsync(new DeleteRoleCommand(id), cancellationToken);

    [HttpGet("permissions")]
    public Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
        => SendAsync(new GetAllPermissionsQuery(), cancellationToken);
}
