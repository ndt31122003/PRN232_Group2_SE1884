namespace PRN232_EbayClone.Application.Abstractions.Realtime;

public interface IRealtimeNotifier
{
    Task SendMessageAsync<T>(
        string connectionId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    Task SendToUserAsync<T>(
        string userId,
        string method,
        T message,
        CancellationToken cancellationToken = default);

    Task SendToUsersAsync<T>(
        IEnumerable<string> userIds,
        string method,
        T message,
        CancellationToken cancellationToken = default);
}
