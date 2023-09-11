namespace SolutionTemplate.Domain._;

public abstract class Entity : IHasId
{
    public Guid Id { get; }

    protected Entity(Guid id) => Id = id;
}