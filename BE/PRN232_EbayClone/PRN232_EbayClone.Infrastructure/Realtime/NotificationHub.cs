using Microsoft.AspNetCore.SignalR;

namespace PRN232_EbayClone.Infrastructure.Realtime;

/// <summary>
/// SignalR hub that:
/// - Adds connected authenticated users to a group named after their userId
/// - Allows clients to join a listing group (to receive live bid/price updates)
/// </summary>
public sealed class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>Client calls this to subscribe to live updates for a listing.</summary>
    public async Task JoinListingGroup(string listingId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"listing-{listingId}");
    }

    /// <summary>Client calls this when leaving the listing page.</summary>
    public async Task LeaveListingGroup(string listingId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"listing-{listingId}");
    }
}
