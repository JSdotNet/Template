
using SolutionTemplate.Domain.Models;
using Xunit;
using FluentAssertions;

namespace SolutionTemplate.Domain.Tests;

public class AuthorTests
{
    [Fact]
    public void Create_Should_CreateNewAuthor()
    {
        // Arrange
        var email = "test@test.com";
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var result = Author.Create(email, firstName, lastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(email);
        result.Value.Firstname.Should().Be(firstName);
        result.Value.Lastname.Should().Be(lastName);
    }

    [Fact]
    public void Update_Should_UpdateAuthor()
    {
        // Arrange
        var email = "test@test.com";
        var firstName = "John";
        var lastName = "Doe";

        var author = Author.Create(email, firstName, lastName).Value;

        var newFirstName = "Jane";
        var newLastName = "Doe";

        // Act
        var result = author.Update(newFirstName, newLastName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        author.Firstname.Should().Be(newFirstName);
        author.Lastname.Should().Be(newLastName);
    }
}
