namespace SolutionTemplate.Domain._;

public interface IRepository<TAggregate> 
    where TAggregate : AggregateRoot
{
    ValueTask<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(TAggregate entity);
    void Remove(TAggregate entity);
}
