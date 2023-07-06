using System.Diagnostics;

using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Events;


namespace SolutionTemplate.Domain.Models;

public sealed record ArticleId(Guid Value) : AggregateRootId(Value);

[DebuggerDisplay("{Id}: {Title}")]
public sealed class Article : AggregateRoot<ArticleId>
{
    private Article() : base(default!) { }

    private Article(ArticleId id) : base(id) { }

    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
    public AuthorId AuthorId { get; private init; } = default!;

    public string Title { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;

    
    private readonly List<Tag> _tags = new();
    public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();


    public static Result<Article> Create(string title, string content, AuthorId author, params string[] tags)
    {
        if (tags.Length < 3)
            return Result.Failure<Article>(DomainErrors.Article.AtLeast3Tags);
        
        if (tags.Length > 10)
            return Result.Failure<Article>(DomainErrors.Article.NoMoreThen10Tags);

        var article = new Article(new ArticleId(Guid.NewGuid()))
        {
            AuthorId = author,
            Title = title,
            Content = content
        };


        article._tags.AddRange(tags.Select(x => new Tag(x)));

        article.RaiseDomainEvent(new ArticleCreatedDomainEvent(article.Id));

        return article;
    }

    public Result Update(string title, string content)
    {
        Title = title;
        Content = content;
        LastUpdated = DateTime.UtcNow;

        return Result.Success();
    }


    public Result AddTag(string name)
    {
        _tags.Add(new Tag(name));
        LastUpdated = DateTime.UtcNow;

        return Result.Success();
    }
}




[DebuggerDisplay("{Name}")]
public sealed record Tag(string Name);