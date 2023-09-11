namespace SolutionTemplate.Domain._.Events;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void Clear();
}
