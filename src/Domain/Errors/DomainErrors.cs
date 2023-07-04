using SolutionTemplate.Domain._;

namespace SolutionTemplate.Domain.Errors;

public static class DomainErrors
{
    public static class Article
    {
        public static readonly Error AtLeast3Tags = new("Article.AtLeast3Tags", "An article must have at least 3 tags.");
        public static readonly Error NoMoreThen10Tags = new("Article.NoMoreThen10Tags", "An article cannot have more than 10 tags.");
    }
}

