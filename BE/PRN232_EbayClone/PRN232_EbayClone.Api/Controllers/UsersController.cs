using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Users.Commands;
using PRN232_EbayClone.Application.Users.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
        => SendAsync(new GetUserByIdQuery(id), cancellationToken);

    [HttpPatch("profile")]
    public Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("verify-payment")]
    public Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);
}
