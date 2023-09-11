using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Domain;

public static class DomainEvents
{
    public sealed record ArticleCreated(Guid ArticleId) : DomainEvent;

    public sealed record AuthorCreated(Guid AuthorId) : DomainEvent;
}

