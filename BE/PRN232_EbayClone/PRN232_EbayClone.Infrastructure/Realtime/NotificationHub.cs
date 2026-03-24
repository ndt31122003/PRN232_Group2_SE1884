using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PRN232_EbayClone.Infrastructure.Realtime;

[Authorize]
public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;
        
        Console.WriteLine($"[SignalR] User {userId} connected with connection {connectionId}");
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;
        
        Console.WriteLine($"[SignalR] User {userId} disconnected from connection {connectionId}");
        
        await base.OnDisconnectedAsync(exception);
    }
}
