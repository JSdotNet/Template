namespace SolutionTemplate.Domain._.Audit;

public interface IContextProvider<out TContext>
    where TContext : class
{
    TContext Context { get; }
}