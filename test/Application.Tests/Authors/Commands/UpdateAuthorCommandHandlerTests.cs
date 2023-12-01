using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Domain.Authors.Repository;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class UpdateAuthorCommandHandlerTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Handle_Should_UpdateAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var firstName = _fixture.Create<string>();
        var lastName = _fixture.Create<string>();
        var author = Author.Create("email", "firstName", "lastName").Value;

        var command = new UpdateAuthor.Command(authorId, firstName, lastName);
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(author);

        var handler = new UpdateAuthor.Handler(authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        author.Firstname.Should().Be(firstName);
        author.Lastname.Should().Be(lastName);
    }


    [Fact]
    public async Task Handle_NotExisting_Should_ReturnError()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var firstName = _fixture.Create<string>();
        var lastName = _fixture.Create<string>();

        var command = new UpdateAuthor.Command(authorId, firstName, lastName);
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Author?)null);

        var handler = new UpdateAuthor.Handler(authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}
