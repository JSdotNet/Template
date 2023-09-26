﻿using SolutionTemplate.Application.Authors.Commands;
using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class DeleteAuthorCommandHandlerTests
{
    [Fact]
    public async Task Handle_Existing_Should_DeletesAuthor()
    {
        // Arrange
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        var command = new DeleteAuthor.Command(Guid.NewGuid());
        var handler = new DeleteAuthor.Handler(authorRepositoryMock.Object);

        authorRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default))
            .ReturnsAsync(Author.Create("Email", "Firstname", "Lastname"));

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        authorRepositoryMock.Verify(r => r.Remove(It.IsAny<Author>()), Times.Once());
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task Handle_NotExisting_Should_ReturnsError()
    {
        // Arrange
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        var command = new DeleteAuthor.Command(Guid.NewGuid());
        var handler = new DeleteAuthor.Handler(authorRepositoryMock.Object);


        authorRepositoryMock.Setup(r => r.GetByIdAsync(command.Id, default))
            .ReturnsAsync((Author?)null);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        authorRepositoryMock.Verify(r => r.Remove(It.IsAny<Author>()), Times.Never());
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
}