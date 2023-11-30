using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class DeleteAuthorCommandHandlerTests
{
    [Fact]
    public async Task Handle_Existing_Should_DeletesAuthor()
    {
        // Arrange
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        var command = new DeleteAuthor.Command(Guid.NewGuid());
        var handler = new DeleteAuthor.Handler(authorRepositoryMock);

        authorRepositoryMock.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .Returns(Author.Create("Email", "Firstname", "Lastname"));

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        authorRepositoryMock.Received(1).Remove(Arg.Any<Author>());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NotExisting_Should_ReturnsError()
    {
        // Arrange
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        var command = new DeleteAuthor.Command(Guid.NewGuid());
        var handler = new DeleteAuthor.Handler(authorRepositoryMock);


        authorRepositoryMock.GetByIdAsync(command.Id, default)
            .Returns((Author?)null);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        authorRepositoryMock.DidNotReceive().Remove(Arg.Any<Author>());
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}