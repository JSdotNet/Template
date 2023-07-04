namespace SolutionTemplate.Domain._;



// TODO: I would like to have entity Id's as structs, but because a struct can't inherit from another struct, I have to use a class.

public record EntityId<TValue>(TValue Value)
{
    public static implicit operator TValue(EntityId<TValue> id) => id.Value;
}