namespace SolutionTemplate.Domain._;

public interface IHasDomainEvents
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }

    void Clear();
}
