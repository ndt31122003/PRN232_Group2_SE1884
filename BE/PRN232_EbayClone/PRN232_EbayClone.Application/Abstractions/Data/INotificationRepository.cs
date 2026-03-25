using PRN232_EbayClone.Domain.Notifications.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface INotificationRepository
{
    void Add(Notification notification);
    Task<List<Notification>> GetByUserIdAsync(Guid userId, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default);
    Task MarkAllReadAsync(Guid userId, CancellationToken cancellationToken = default);
}
