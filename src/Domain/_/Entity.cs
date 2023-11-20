namespace SolutionTemplate.Domain._;

public abstract class Entity(Guid id) : IHasId
{
    public Guid Id { get; } = id;
}