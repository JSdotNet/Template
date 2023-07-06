namespace SolutionTemplate.Domain._;

public interface IRepository<TAggregate, in TId> 
    where TAggregate : IAggregateRoot<TId>
    where TId : AggregateRootId
{
    ValueTask<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    void Add(TAggregate entity);
    void Remove(TAggregate entity);
}
