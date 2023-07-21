namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot : Entity, IHasDomainEvents
{
    protected AggregateRoot(Guid id) : base(id) { }

    protected DomainEvents DomainEvents { get; } = new();

    IReadOnlyList<DomainEvent> IHasDomainEvents.DomainEvents => DomainEvents.Items;
    void IHasDomainEvents.Clear() => DomainEvents.Clear();
}