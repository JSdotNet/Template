using SolutionTemplate.Domain.Articles;
using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Integration.Tests.Helpers;

public static class TestData
{
    public static Author Author { get; } = Author.Create("job.schepers@gmail.com", "Job", "Schepers");
    public static Article ArticleSolutionTemplate { get; } = Article.Create("Solution Template", "How to setup a solution for a DDD oriented API", Author.Id, "C#", "DDD", ".net");
    public static Article ArticleMediatR { get; } = Article.Create("MediatR", "Let's use MediatR", Author.Id, "Command", "Query", "CQRS");



    public static async Task SeedTestData(DataContext context)
    {
        context.Authors.Add(Author);
        context.Articles.Add(ArticleSolutionTemplate);
        context.Articles.Add(ArticleMediatR);

        await context.SaveChangesAsync();
    }
}
