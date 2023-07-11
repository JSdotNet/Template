using FluentAssertions;

using Moq;

using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

using Xunit;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class DeleteArticleCommandHandlerTests
{
    [Fact]
    public async Task Handle_Existing_Should_DeleteArticle()
    {
        // Arrange
        var articleRepositoryMock = new Mock<IArticleRepository>();
        var command = new DeleteArticle.Command(Guid.NewGuid());
        var handler = new DeleteArticle.Handler(articleRepositoryMock.Object);

        articleRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default))
            .ReturnsAsync(Article.Create("Test", "Test Content", Guid.NewGuid(), "1", "2", "3").Value);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        articleRepositoryMock.Verify(r => r.Remove(It.IsAny<Article>()), Times.Once());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NotExisting_Should_ReturnError()
    {
        // Arrange
        var articleRepositoryMock = new Mock<IArticleRepository>();
        var command = new DeleteArticle.Command(Guid.NewGuid());
        var handler = new DeleteArticle.Handler(articleRepositoryMock.Object);

        articleRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default))
            .ReturnsAsync((Article?)null);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        articleRepositoryMock.Verify(r => r.Remove(It.IsAny<Article>()), Times.Never());
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}