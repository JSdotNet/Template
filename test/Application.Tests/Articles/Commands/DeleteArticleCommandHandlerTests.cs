using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Domain.Articles;
using SolutionTemplate.Domain.Articles.Repository;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class DeleteArticleCommandHandlerTests
{
    [Fact]
    public async Task Handle_Existing_Should_DeleteArticle()
    {
        // Arrange
        var articleRepositoryMock = Substitute.For<IArticleRepository>();
        var command = new DeleteArticle.Command(Guid.NewGuid());
        var handler = new DeleteArticle.Handler(articleRepositoryMock);

        articleRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Article.Create("Test", "Test Content", Guid.NewGuid(), "1", "2", "3").Value);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        articleRepositoryMock.Received(1).Remove(Arg.Any<Article>());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NotExisting_Should_ReturnError()
    {
        // Arrange
        var articleRepositoryMock = Substitute.For<IArticleRepository>();
        var command = new DeleteArticle.Command(Guid.NewGuid());
        var handler = new DeleteArticle.Handler(articleRepositoryMock);

        articleRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Article?)null);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        articleRepositoryMock.DidNotReceive().Remove(Arg.Any<Article>());
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}