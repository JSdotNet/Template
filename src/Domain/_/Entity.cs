namespace SolutionTemplate.Domain._;

public abstract class Entity<TId>
    //where TId : struct
{
    public TId Id { get; }

    protected Entity(TId id) => Id = id;
}