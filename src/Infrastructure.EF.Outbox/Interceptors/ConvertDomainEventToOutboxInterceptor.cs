using Microsoft.EntityFrameworkCore.Diagnostics;

using Newtonsoft.Json;

using SolutionTemplate.Domain._.Events;
using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Interceptors;


public sealed class ConvertDomainEventToOutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        // For all entities that implement IHasDomainEvents, get the domain events and add them to the Outbox.
        var events = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Select(x => x.Entity)
            .SelectMany(entityWithDomainEvents =>
            {
                // Calling ToList so that we get a copy of the domain events, otherwise ClearDomainEvents will clear the events gathered here.
                var domainEvents = entityWithDomainEvents.DomainEvents.ToList();

                entityWithDomainEvents.Clear();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.Id,
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().FullName!,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            })
            .ToList();

        if (events.Any())
            dbContext.Set<OutboxMessage>().AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
