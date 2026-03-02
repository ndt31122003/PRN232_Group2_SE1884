namespace PRN232_EbayClone.Application.Abstractions.Realtime;

public interface IRealtimeNotifier
{
    Task SendMessageAsync<T>(
        string connectionId,
        string method,
        T message,
        CancellationToken cancellationToken = default);
}
