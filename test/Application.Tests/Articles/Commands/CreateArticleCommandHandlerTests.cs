﻿using SolutionTemplate.Application.Articles.Commands;
using SolutionTemplate.Domain.Articles;
using SolutionTemplate.Domain.Articles.Repository;
using SolutionTemplate.Domain.Authors;
using SolutionTemplate.Domain.Authors.Repository;

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

        var articleRepositoryMock = Substitute.For<IArticleRepository>();
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.FindByEmail(email).Returns(default(Author));

        var handler = new CreateArticle.Handler(articleRepositoryMock, authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Title.Should().Be(title);
        result.Value.Content.Should().Be(content);


        articleRepositoryMock.Received(1).Add(Arg.Is<Article>(a =>
            a.Id == result.Value.Id &&
            a.Title == title &&
            a.Content == content));

        authorRepositoryMock.Received(1).Add(Arg.Is<Author>(a =>
            a.Email == email &&
            a.Firstname == firstname &&
            a.Lastname == lastname));
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

        var articleRepositoryMock = Substitute.For<IArticleRepository>();
        var authorRepositoryMock = Substitute.For<IAuthorRepository>();
        authorRepositoryMock.FindByEmail(email).Returns(author);

        var handler = new CreateArticle.Handler(articleRepositoryMock, authorRepositoryMock);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value.Title.Should().Be(title);
        result.Value.Content.Should().Be(content);


        articleRepositoryMock.Received(1).Add(Arg.Is<Article>(a => 
            a.Id == result.Value.Id &&
            a.Title == title &&
            a.Content == content));

        authorRepositoryMock.DidNotReceive().Add(Arg.Any<Author>());
    }
}