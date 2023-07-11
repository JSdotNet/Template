using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain;

public static class DomainEvents
{
    public sealed record ArticleCreated(Guid ArticleId) : IDomainEvent;

    public sealed record AuthorCreated(Guid AuthorId) : IDomainEvent;
}

