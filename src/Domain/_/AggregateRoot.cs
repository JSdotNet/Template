namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot<TAggregateId> : Entity<TAggregateId>, IAggregateRoot<TAggregateId> 
    where TAggregateId : AggregateRootId
{

    //private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TAggregateId id) : base(id) { }

    //public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    //protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    //public void ClearDomainEvents() => _domainEvents.Clear();
}
