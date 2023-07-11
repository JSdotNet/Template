using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

using Xunit;

namespace SolutionTemplate.Domain.Tests;

public class ArticleTests
{
    [Fact]
    public void Create_Should_Create_Article_With_Tags_For_Valid_Inputs()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new [] { "test", "tags", "example" };
        var authorId = Guid.NewGuid();

        // Act
        var result = Article.Create(title, content, authorId, tags);

        // Assert
        Assert.IsType<Result<Article>>(result);
        Assert.True(result.IsSuccess);

        var article = result.Value;

        Assert.NotNull(article);
        Assert.NotNull(article.Id);

        Assert.Equal(title, article.Title);
        Assert.Equal(content, article.Content);
        Assert.Equal(authorId, article.AuthorId);

        Assert.Equal(tags.Length, article.Tags.Count);
    }


    [Fact]
    public void Create_Should_Return_Failure_When_TooFewTags_Were_Provided()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new [] { "test", "tags" };

        // Act
        var result = Article.Create(title, content, Guid.NewGuid(), tags);

        // Assert
        Assert.IsType<Result<Article>>(result);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Article.AtLeast3Tags.Code, result.Error!);
    }

    [Fact]
    public void Create_Should_Return_Failure_When_TooManyTags_Were_Provided()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new [] { "test", "tags", "example", "sample", "unit", "mock", "research", "data", "analysis", "statistics", "extra" };

        // Act
        var result = Article.Create(title, content, Guid.NewGuid(), tags);

        // Assert
        Assert.IsType<Result<Article>>(result);
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.Article.NoMoreThen10Tags.Code, result.Error!);
    }

    [Fact]
    public void AddTag_Should_Add_Tag_To_An_Article_When_Valid()
    {
        // Arrange
        var article = Article.Create("Test Title", "Test Content", Guid.Empty, "test", "tags", "example").Value;
        var tagName = "test";

        // Act
        var result = article.AddTag(tagName);

        // Assert
        Assert.IsType<Result>(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(4, article.Tags.Count);
        Assert.Equal(tagName, article.Tags[3].Name);
    }
}