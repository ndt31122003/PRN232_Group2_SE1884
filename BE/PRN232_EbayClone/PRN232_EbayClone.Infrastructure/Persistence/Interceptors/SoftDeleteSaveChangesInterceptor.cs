using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Infrastructure.Persistence.Interceptors;

public sealed class SoftDeleteSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var softDeleteEntries = dbContext.ChangeTracker
           .Entries<ISoftDeletable>()
           .Where(e => e.State == EntityState.Deleted)
           .ToList();

        foreach (var softDeleteEntry in softDeleteEntries)
        {
            softDeleteEntry.State = EntityState.Modified;
            softDeleteEntry.Entity.IsDeleted = true;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
