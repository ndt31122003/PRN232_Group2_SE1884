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
}
