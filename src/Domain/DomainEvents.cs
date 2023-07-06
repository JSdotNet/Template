using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain;

public static class DomainEvents
{
    public sealed record ArticleCreated(ArticleId ArticleId) : IDomainEvent;

    public sealed record AuthorCreated(AuthorId AuthorId) : IDomainEvent;
}

