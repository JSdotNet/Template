using FluentValidation.TestHelper;

using SolutionTemplate.Application.Articles.Commands;

using Xunit;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public sealed class CreateArticleValidatorTests
{
    private readonly CreateArticle.Validator _validator = new();

    [Fact]
    public void Validator_EmptyTitle_Should_ReturnError()
    {
        // Arrange
        var command = new CreateArticle.Command("", "Content", "Email", "Firstname", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Title).WithErrorMessage("'Title' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyContent_Should_ReturnError()
    {
        // Arrange
        var command = new CreateArticle.Command("Title", "", "Email", "Firstname", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Content).WithErrorMessage("'Content' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyEmail_Should_ReturnError()
    {
        // Arrange
        var command = new CreateArticle.Command("Title", "Content", "", "Firstname", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyFirstName_Should_ReturnError()
    {
        // Arrange
        var command = new CreateArticle.Command("Title", "Content", "Email", "", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Firstname).WithErrorMessage("'Firstname' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyLastName_Should_ReturnError()
    {
        // Arrange
        var command = new CreateArticle.Command("Title", "Content", "Email", "Firstname", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Lastname).WithErrorMessage("'Lastname' must not be empty.");
    }
}


