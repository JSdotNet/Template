using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot : Entity, IHasDomainEvents
{
    protected AggregateRoot(Guid id) : base(id) { }

    protected DomainEventWrapper DomainEvents { get; } = new();

    IReadOnlyList<IDomainEvent> IHasDomainEvents.DomainEvents => DomainEvents.Items;
    void IHasDomainEvents.Clear() => DomainEvents.Clear();
}