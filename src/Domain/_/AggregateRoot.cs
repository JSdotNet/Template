namespace SolutionTemplate.Domain._;

public abstract class AggregateRoot<TAggregateId> : Entity<TAggregateId>, IAggregateRoot<TAggregateId> 
    where TAggregateId : AggregateRootId
{
    protected AggregateRoot(TAggregateId id) : base(id) { }
}
