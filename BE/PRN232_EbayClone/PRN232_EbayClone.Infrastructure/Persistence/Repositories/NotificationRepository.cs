using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Notifications.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NotificationRepository(ApplicationDbContext dbContext)
        => _dbContext = dbContext;

    public void Add(Notification notification)
        => _dbContext.Notifications.Add(notification);

    public Task<List<Notification>> GetByUserIdAsync(
        Guid userId,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken cancellationToken = default)
        => _dbContext.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

    public async Task MarkAllReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), cancellationToken);
    }
}
