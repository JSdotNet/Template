namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot : Entity, IHasDomainEvents
{
    protected AggregateRoot(Guid id) : base(id) { }

    public DomainEvents DomainEvents { get; } = new();
}