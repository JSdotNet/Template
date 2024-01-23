using System.Diagnostics;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Articles;

[DebuggerDisplay("{Id}: {Title}")]
public sealed class Article : AggregateRoot
{
    private Article() : base(default!) { }

    private Article(Guid id) : base(id) { }

    public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
    public Guid AuthorId { get; private init; } = default!;

    public string Title { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;


    private readonly List<string> _tags = [];
    public IReadOnlyList<string> Tags => _tags.AsReadOnly();


    public static Result<Article> Create(string title, string content, Guid author, params string[] tags)
    {
        if (tags.Length < 3)
            return Result.Failure<Article>(DomainErrors.Article.AtLeast3Tags);

        if (tags.Length > 10)
            return Result.Failure<Article>(DomainErrors.Article.NoMoreThen10Tags);

        var article = new Article(Guid.NewGuid())
        {
            AuthorId = author,
            Title = title,
            Content = content
        };


        article._tags.AddRange(tags);

        article.DomainEvents.Raise(new DomainEvents.ArticleCreated(article.Id));

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
        _tags.Add(name);
        LastUpdated = DateTime.UtcNow;

        return Result.Success();
    }
}