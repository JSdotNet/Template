
using Microsoft.EntityFrameworkCore.Diagnostics;

using Newtonsoft.Json;

using SolutionTemplate.Domain._;
using SolutionTemplate.Infrastructure.EF.Outbox;

namespace SolutionTemplate.Infrastructure.EF.Interceptors;


internal sealed class ConvertDomainEventToOutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);


        var events = dbContext.ChangeTracker.Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                // Calling ToList so that we get a copy of the domain events, otherwise ClearDomainEvents will clear the events gathered here.
                var domainEvents = aggregateRoot.DomainEvents.ToList();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().FullName!,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
