using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace PRN232_EbayClone.Infrastructure.Realtime;

public sealed class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Get userId from JWT token claims
        var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? connection.User?.FindFirst("sub")?.Value
                     ?? connection.User?.FindFirst("userId")?.Value
                     ?? connection.User?.FindFirst("id")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            Console.WriteLine($"[SignalR] Resolved userId: {userId} for connection {connection.ConnectionId}");
        }
        else
        {
            Console.WriteLine($"[SignalR] WARNING: Could not resolve userId for connection {connection.ConnectionId}");
            Console.WriteLine($"[SignalR] Available claims: {string.Join(", ", connection.User?.Claims.Select(c => $"{c.Type}={c.Value}") ?? Array.Empty<string>())}");
        }

        return userId;
    }
}
