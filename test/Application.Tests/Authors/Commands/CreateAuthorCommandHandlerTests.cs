using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Domain.Authors.Repository;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class CreateAuthorCommandHandlerTests
{
    private readonly Fixture _fixture = new();


    [Fact]
    public async Task Handle_Should_Create_New_Author()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var firstname = _fixture.Create<string>();
        var lastname = _fixture.Create<string>();
        var command = new CreateAuthor.Command(email, firstname, lastname);

        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.FindByEmail(email).Returns(default(Author));

        var handler = new CreateAuthor.Handler(authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        authorRepositoryMock.Received(1).Add(Arg.Is<Author>(a => a.Id == result.Value &&
                                                       a.Email == email &&
                                                       a.Firstname == firstname &&
                                                       a.Lastname == lastname));
    }

    [Fact]
    public async Task Handle_Should_Return_Already_Exists_Error()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var firstname = _fixture.Create<string>();
        var lastname = _fixture.Create<string>();
        var command = new CreateAuthor.Command(email, firstname, lastname);

        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.FindByEmail(email).Returns(Author.Create(email, firstname, lastname));

        var handler = new CreateAuthor.Handler(authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.Code.Should().Be(ApplicationErrors.AlreadyExists<Author>(email).Code);
        result.Error?.Message.Should().Be(ApplicationErrors.AlreadyExists<Author>(email).Message);
    }
}