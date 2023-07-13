using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain;

public static class DomainEvents
{
    public sealed record ArticleCreated(Guid ArticleId) : DomainEvent(Guid.NewGuid());

    public sealed record AuthorCreated(Guid AuthorId) : DomainEvent(Guid.NewGuid());
}

