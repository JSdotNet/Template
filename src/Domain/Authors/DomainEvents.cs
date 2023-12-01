using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Domain.Authors;

public static class DomainEvents
{
    public sealed record AuthorCreated(Guid AuthorId) : DomainEvent;
}

