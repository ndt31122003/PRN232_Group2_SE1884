using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using System.Diagnostics;

namespace PRN232_EbayClone.Infrastructure.Persistence.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserContext _userContext;

    public AuditSaveChangesInterceptor(IUserContext userContext) => _userContext = userContext;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var userId = _userContext.UserId ?? "System";

        var auditableEntries = dbContext.ChangeTracker
           .Entries<IAuditable>()
           .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
           .ToList();

        foreach (var auditableEntry in auditableEntries)
        {
            var auditableEntity = auditableEntry.Entity;

            if (auditableEntry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
                auditableEntity.CreatedBy = userId;
            }

            if (auditableEntry.State == EntityState.Modified)
            {
                auditableEntity.UpdatedAt = DateTime.UtcNow;
                auditableEntity.UpdatedBy = userId;
            }
        }


        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
