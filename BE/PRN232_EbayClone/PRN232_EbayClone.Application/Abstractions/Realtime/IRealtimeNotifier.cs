namespace PRN232_EbayClone.Application.Abstractions.Realtime;

public interface IRealtimeNotifier
{
    Task SendMessageAsync<T>(
        string connectionId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    /// <summary>Sends to all connections belonging to a specific user (userId group).</summary>
    Task BroadcastToUserAsync<T>(
        string userId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    /// <summary>Sends to everyone currently watching a listing (group = listingId).</summary>
    Task BroadcastToListingGroupAsync<T>(
        Guid listingId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    /// <summary>Sends to a specific user (userId group).</summary>
    Task SendToUserAsync<T>(
        string userId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    /// <summary>Sends to multiple users.</summary>
    Task SendToUsersAsync<T>(
        IEnumerable<string> userIds,
        string method,
        T message,
        CancellationToken cancellationToken = default);
}

