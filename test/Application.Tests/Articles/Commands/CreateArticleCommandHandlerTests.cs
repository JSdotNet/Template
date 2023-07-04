using AutoFixture;

using FluentAssertions;

using Moq;

using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

using Xunit;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class CreateArticleCommandHandlerTests
{
    private readonly Fixture _fixture = new();


    [Fact]
    public async Task Handle_Should_Create_New_Article_And_Author()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var content = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var firstname = _fixture.Create<string>();
        var lastname = _fixture.Create<string>();
        var command = new CreateArticle.Command(title, content, email, firstname, lastname, "1", "2", "3");

        var articleRepositoryMock = new Mock<IArticleRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(m => m.FindByEmail(email)).ReturnsAsync(default(Author));

        var handler = new CreateArticle.Handler(articleRepositoryMock.Object, authorRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Title.Should().Be(title);
        result.Value.Content.Should().Be(content);


        articleRepositoryMock.Verify(m =>
            m.Add(It.Is<Article>(a => a.Id.Value == result.Value.Id &&
                                                         a.Title == title &&
                                                         a.Content == content)), Times.Once);

        authorRepositoryMock.Verify(m =>
            m.Add(It.Is<Author>(a => a.Email == email &&
                                                       a.Firstname == firstname &&
                                                       a.Lastname == lastname)), Times.Once);

        articleRepositoryMock.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Create_New_Article_And_Link_Existing_Author()
    {
        // Arrange
        var title = _fixture.Create<string>();
        var content = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        var firstname = _fixture.Create<string>();
        var lastname = _fixture.Create<string>();
        var command = new CreateArticle.Command(title, content, email, "n/a", "n/a", "1", "2", "3");
        var author = Author.Create(email, firstname, lastname).Value;

        var articleRepositoryMock = new Mock<IArticleRepository>();
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(m => m.FindByEmail(email)).ReturnsAsync(author);

        var handler = new CreateArticle.Handler(articleRepositoryMock.Object, authorRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Title.Should().Be(title);
        result.Value.Content.Should().Be(content);


        articleRepositoryMock.Verify(m =>
            m.Add(It.Is<Article>(a => a.Id.Value == result.Value.Id &&
                                      a.Title == title &&
                                      a.Content == content)), Times.Once);

        authorRepositoryMock.Verify(m => m.Add(It.IsAny<Author>()), Times.Never);
        articleRepositoryMock.VerifyAll();
    }
}