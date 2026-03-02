using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Identity.Commands;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/identity")]
public class IdentityController(ISender sender) : ApiController(sender)
{
    [HttpPost("login")]
    public Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("logout")]
    public Task<IActionResult> Logout(LogoutCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpGet("google/login")]
    public IActionResult LoginWithGoogle(string? returnUrl = null)
    {
        var completionUri = Url.ActionLink(nameof(GoogleComplete), "Identity")
                            ?? $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/identity/google/complete";

        var props = new AuthenticationProperties
        {
            RedirectUri = completionUri
        };

        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            props.Items["returnUrl"] = returnUrl;
        }

        return Challenge(props, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google/complete")]
    public async Task<IActionResult> GoogleComplete(CancellationToken cancellationToken)
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!result.Succeeded || result.Principal is null)
        {
            return Unauthorized();
        }

        var externalEmail = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var externalFullName = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

        var targetUrl = string.Empty;

        if (result.Properties?.Items.TryGetValue("returnUrl", out var storedReturn) == true &&
            !string.IsNullOrWhiteSpace(storedReturn))
        {
            targetUrl = storedReturn;
        }

        if (string.IsNullOrWhiteSpace(targetUrl) &&
            Request.Query.TryGetValue("returnUrl", out var queryReturn))
        {
            var queryReturnValue = queryReturn.ToString();
            if (!string.IsNullOrWhiteSpace(queryReturnValue))
            {
                targetUrl = queryReturnValue;
            }
        }

        if (!string.IsNullOrWhiteSpace(targetUrl))
        {
            targetUrl = Uri.UnescapeDataString(targetUrl);
        }

        if (Uri.TryCreate(targetUrl, UriKind.Relative, out var relativeUrl))
        {
            targetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{relativeUrl}";
        }
        else if (!Uri.TryCreate(targetUrl, UriKind.Absolute, out _))
        {
            targetUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        }

        var command = new ExternalLoginCommand(
            Email: externalEmail ?? string.Empty,
            FullName: externalFullName ?? string.Empty
        );

        var tokenResult = await Sender.Send(command, cancellationToken);
        if (tokenResult.IsFailure)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return HandleFailure(tokenResult);
        }

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        var redirectUri = $"{targetUrl}#access_token={tokenResult.Value.AccessToken}&refresh_token={tokenResult.Value.RefreshToken}";

        return Redirect(redirectUri);
    }

    [HttpPost("refresh-token")]
    public Task<IActionResult> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("register")]
    public Task<IActionResult> RegisterUser(RegisterUserCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("verify-email")]
    public Task<IActionResult> VerifyEmail(VerifyEmailCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("forgot-password")]
    public Task<IActionResult> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("reset-password")]
    public Task<IActionResult> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);

    [HttpPost("resend-otp")]
    public Task<IActionResult> ResendOtp(ResendOtpCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);
}