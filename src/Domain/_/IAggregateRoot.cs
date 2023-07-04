namespace SolutionTemplate.Domain._;

public interface IAggregateRoot<out TId>
    where TId : AggregateRootId
{
    TId Id { get; }
}
