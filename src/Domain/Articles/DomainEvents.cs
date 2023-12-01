using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Domain.Articles;

public static class DomainEvents
{
    public sealed record ArticleCreated(Guid ArticleId) : DomainEvent;
}

