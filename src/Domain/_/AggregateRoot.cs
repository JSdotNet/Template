using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot(Guid id) : Entity(id), IHasDomainEvents
{
    protected DomainEventWrapper DomainEvents { get; } = new();

    IReadOnlyList<IDomainEvent> IHasDomainEvents.DomainEvents => DomainEvents.Items;
    void IHasDomainEvents.Clear() => DomainEvents.Clear();
}