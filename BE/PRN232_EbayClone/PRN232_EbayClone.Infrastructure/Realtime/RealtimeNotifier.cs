using Microsoft.AspNetCore.SignalR;
using PRN232_EbayClone.Application.Abstractions.Realtime;

namespace PRN232_EbayClone.Infrastructure.Realtime;

public sealed class RealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public RealtimeNotifier(IHubContext<NotificationHub> hubContext)
        => _hubContext = hubContext;

    public Task SendMessageAsync<T>(
        string connectionId,
        string method,
        T message,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Client(connectionId)
            .SendAsync(method, message, cancellationToken);
    }

    public Task BroadcastToUserAsync<T>(
        string userId,
        string method,
        T message,
        CancellationToken cancellationToken = default)
    {
        // SignalR groups named after userId allow all user connections to be reached
        return _hubContext.Clients
            .Group(userId)
            .SendAsync(method, message, cancellationToken);
    }

    public Task BroadcastToListingGroupAsync<T>(
        Guid listingId,
        string method,
        T message,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Group($"listing-{listingId}")
            .SendAsync(method, message, cancellationToken);
    }

    public Task SendToUserAsync<T>(
        string userId,
        string method,
        T message,
        CancellationToken cancellationToken = default)
    {
        return BroadcastToUserAsync(userId, method, message, cancellationToken);
    }

    public Task SendToUsersAsync<T>(
        IEnumerable<string> userIds,
        string method,
        T message,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Groups(userIds)
            .SendAsync(method, message, cancellationToken);
    }
}
