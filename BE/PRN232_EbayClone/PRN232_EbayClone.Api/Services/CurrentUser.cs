using PRN232_EbayClone.Application.Abstractions.Authentication;
using System.Security.Claims;

namespace PRN232_EbayClone.Api.Services;

public class CurrentUser : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUser(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string? UserId
         => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Username
        => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
}
