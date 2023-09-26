using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class UpdateArticleCommandHandlerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Handle_Should_UpdateAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var articleId = Guid.NewGuid();
        var title = _fixture.Create<string>();
        var content = _fixture.Create<string>();
        var article = Article.Create("title", "contact", authorId, "1", "2", "3").Value;

        var command = new UpdateArticle.Command(articleId, title, content);
        var articleRepositoryMock = new Mock<IArticleRepository>();
        articleRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(article);

        var handler = new UpdateArticle.Handler(articleRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        article.Title.Should().Be(title);
        article.Content.Should().Be(content);
    }


    [Fact]
    public async Task Handle_NotExisting_Should_ReturnError()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var firstName = _fixture.Create<string>();
        var lastName = _fixture.Create<string>();

        var command = new UpdateArticle.Command(authorId, firstName, lastName);
        var articleRepositoryMock = new Mock<IArticleRepository>();
        articleRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Article?)null);

        var handler = new UpdateArticle.Handler(articleRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}
