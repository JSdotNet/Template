namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) : base(id) { }


    // The Aggregate is responsible for keeping track of its domain events.
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

