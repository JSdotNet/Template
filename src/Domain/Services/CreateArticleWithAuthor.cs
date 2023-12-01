using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Articles;
using SolutionTemplate.Domain.Authors;

namespace SolutionTemplate.Domain.Services;

// A domain service is a stateless service that operates on domain objects and domain logic.
// It is a stateless service because it does not have any state of its own.
public static class CreateArticleService
{
    public static Result<(Article Article, Author Author)> CreateArticleForAuthor(string title, string content, string email, string firstname, string lastname, params string[] tags)
    {

        var author = Author.Create(email, firstname, lastname);
        if (author.IsFailure)
            return Result.Failure<(Article, Author)>(author.Error!.Value); // TODO I do not like the .Value here...

        var article = Article.Create(title, content, author.Value.Id, tags); // TODO I do not like the .Value here...
        if (article.IsFailure)
        {
            return Result.Failure<(Article, Author)>(article.Error!.Value);
        }

        return (article, author);
    }
}
