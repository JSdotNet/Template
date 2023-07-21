# Domain Events with Outbox Pattern

In this project I abstracted my implementation for handling domain event with an outbox pattern, to a library that can be used in any project.
This implementation is build on top of [Entity Framework](https://docs.microsoft.com/en-us/ef/), [MediatR](https://github.com/jbogard/MediatR), [Scrutor](https://github.com/khellang/Scrutor) and [Quartz](https://www.quartz-scheduler.net/).

The library could potentially be made into a nuget package, but currently it still has some dependencies on my solution project.
(I would probably need to extract some contract to make it fully independent)

## The implementation

The project uses a SaveChangesInterceptor to detect if an entity has Domain Events and save them to the database.
An Entity has to implement the following interface for the interceptor to detect the events:

```csharp
public interface IHasDomainEvents
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }

    void Clear();
}
```

There is a DomainEvents class the can be added to the entity when it need to support domain events.
My preferred approach is to make the AggregateRoot implement this interface and be responsible for raising the events, but this library does not enforce this.

``` csharp
public abstract class AggregateRoot : Entity, IHasDomainEvents
{
    protected AggregateRoot(Guid id) : base(id) { }

    protected DomainEvents DomainEvents { get; } = new();

    IReadOnlyList<DomainEvent> IHasDomainEvents.DomainEvents => DomainEvents.Items;
    void IHasDomainEvents.Clear() => DomainEvents.Clear();
}
```

By exposing the DomainEvents class protected from the aggregate root, any aggregate implementation can raise domain events as follows:

``` csharp
article.DomainEvents.Raise(new DomainEvents.ArticleCreated(article.Id));
```

I added all domain events to on static class, so that I have an overview of them in one place. That makes it easy to find out if the are raised/handled somewhere
Each Domain event has to inherit from the DomainEvent class:

```csharp

public abstract record DomainEvent : IDomainEvent
{
    // The Id needs to have an init setter so that it can be deserialized by the outbox processor
    // We do not have Access to NewtonSoft here to apply the attribute instead.
    public Guid Id { get; init; } = Guid.NewGuid();
};

```

It enforces that each domain event has an unique Id. This Id is used to align with the Id of the OutBoxMessage record in the database.
It needs to align so that while handling the event, a Consumer record can be stored with the same Id.

This project contains 2 background services (/ Workers):

- The [OutboxMessageProcessor](workers/OutboxMessageProcessor.cs) is responsible for processing the OutboxMessage records in the database.
- The [OutboxMessageCleaner](workers/OutboxMessageCleaner.cs) is responsible for cleaning up the OutboxMessage records after a configurable retention period.

The OutboxMessageProcessor uses Quartz to schedule the processing of the OutboxMessage records.
It takes some unprocessed records, from the database and publishes them to MediatR. If an error occurs in a handler it is expected to throw an exception.
After some retries with Polly the message is marked as failed and the error is logged.

``` 
⚠️ It may be useful to extend this in a way that Outbox messages with an error are retried at a later date, but you probably don't want to retry forever.
One approach may be to register an expiration date for each event and set the Processed data only when processed successfully or expired.
```

With MediatR multiple handlers can be configured for each domain event.
Through Scrutor each of those handlers is decorated with the IdempotentDomainEventNotificationHandler.
This stores a Consumer record in the database (for each handler) to ensure that the event is only handled once.
When processing an OutboxMessage partially it can safely be retried without the risk of running the same handler multiple times.

```
⚠️ I do not like Scrutor for this. It is now only used to make the Outbox pattern possible in the project. There may be a better way to intercept the handler...

It may be possible to do this with a custom implementation for INotificationPublisher, supported starting form MediatR 12. I plan to try this at some point...
```

## Resources

- [Transactional Outbox Pattern | Clean Architecture, .NET 6](https://youtu.be/XALvnX7MPeo)
- [Handle Duplicate Messages With Idempotent Consumers | Idempotency](https://youtu.be/mGeEtokcjVQ)
