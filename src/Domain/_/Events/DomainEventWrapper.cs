namespace SolutionTemplate.Domain._.Events;

public sealed class DomainEventWrapper
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> Items => _domainEvents.AsReadOnly();

    internal void Raise(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    internal void Clear() => _domainEvents.Clear();
}
