namespace SolutionTemplate.Domain._;

public sealed class DomainEvents
{
    // The Aggregate is responsible for keeping track of its domain events.
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> Items => _domainEvents.AsReadOnly();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void Clear() => _domainEvents.Clear();
}
