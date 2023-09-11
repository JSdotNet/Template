namespace SolutionTemplate.Domain._.Events;

public abstract record DomainEvent : IDomainEvent
{
    // The Id needs to have an init setter so that it can be deserialized by the outbox processor
    // We do not have Access to NewtonSoft here to apply the attribute instead.
    public Guid Id { get; init; } = Guid.NewGuid();
}
