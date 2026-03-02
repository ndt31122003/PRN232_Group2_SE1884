using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Infrastructure.Outbox;
using Newtonsoft.Json;

namespace PRN232_EbayClone.Infrastructure.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEntities = dbContext.ChangeTracker
            .Entries()
            .Where(e => e.Entity is IHasDomainEvents)
            .Select(e => (IHasDomainEvents)e.Entity)
            .ToList();

        var events = domainEntities
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }),
                OccurredOn = domainEvent.OccurredOn
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
