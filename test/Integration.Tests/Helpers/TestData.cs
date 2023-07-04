using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain.Models;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Integration.Tests.Helpers;

public static class TestData
{
    public static Author Author { get; } = Author.Create("job.schepers@gmail.com", "Job", "Schepers");
    public static Article ArticleSolutionTemplate { get; } = Article.Create("Solution Template", "How to setup a solution for a DDD oriented API", Author.Id, "C#", "DDD", ".net");
    public static Article ArticleMediatR { get; } = Article.Create("MediatR", "Let's use MediatR", Author.Id, "Command", "Query", "CQRS");



    public static async Task SeedTestData(string connectionString)
    {
        await using var context = new DataContext(new DbContextOptionsBuilder<DataContext>().UseSqlServer(connectionString).Options);
        await context.Database.EnsureCreatedAsync();

        context.Authors.Add(TestData.Author);
        context.Articles.Add(TestData.ArticleSolutionTemplate);
        context.Articles.Add(TestData.ArticleMediatR);

        await context.SaveChangesAsync();
    }
}
