using SolutionTemplate.Domain._;
using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Domain.Tests;

public class ArticleTests
{
    [Fact]
    public void Create_Should_Create_Article_With_Tags_For_Valid_Inputs()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new[] { "test", "tags", "example" };
        var authorId = Guid.NewGuid();

        // Act
        var result = Article.Create(title, content, authorId, tags);

        // Assert
        result.Should().BeOfType<Result<Article>>()
              .Which.IsSuccess.Should().BeTrue();
     
        var article = result.Value;

        article.Should().NotBeNull();
        article.Id.Should().NotBeEmpty();

        article.Title.Should().Be(title);
        article.Content.Should().Be(content);
        article.AuthorId.Should().Be(authorId);

        article.Tags.Should().HaveCount(tags.Length);
    }


    [Fact]
    public void Create_Should_Return_Failure_When_TooFewTags_Were_Provided()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new[] { "test", "tags" };

        // Act
        var result = Article.Create(title, content, Guid.NewGuid(), tags);

        // Assert
        result.Should().BeOfType<Result<Article>>()
              .Which.IsFailure.Should().BeTrue();

        result.Error.Should().Be(DomainErrors.Article.AtLeast3Tags.Code);
    }

    [Fact]
    public void Create_Should_Return_Failure_When_TooManyTags_Were_Provided()
    {
        // Arrange
        var title = "Test Title";
        var content = "Test Content";
        var tags = new[] { "test", "tags", "example", "sample", "unit", "mock", "research", "data", "analysis", "statistics", "extra" };

        // Act
        var result = Article.Create(title, content, Guid.NewGuid(), tags);

        // Assert
        result.Should().BeOfType<Result<Article>>()
              .Which.IsFailure.Should().BeTrue();

        result.Error.Should().Be(DomainErrors.Article.NoMoreThen10Tags.Code);
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
        result.Should().BeOfType<Result>()
              .Which.IsSuccess.Should().BeTrue();

        article.Tags.Should().HaveCount(4);
        article.Tags.Should().Contain(tagName);
    }
}