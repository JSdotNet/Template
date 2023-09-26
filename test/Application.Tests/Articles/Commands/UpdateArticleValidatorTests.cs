using FluentValidation.TestHelper;

using SolutionTemplate.Application.Articles.Commands;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class UpdateArticleValidatorTests
{
    private readonly UpdateArticle.Validator _validator = new();

    [Fact]
    public void Validator_EmptyId_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateArticle.Command(Guid.Empty, "title", "content");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("'Id' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyTitle_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateArticle.Command(Guid.NewGuid(), "", "lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Title).WithErrorMessage("'Title' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyContent_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateArticle.Command(Guid.NewGuid(), "title", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Content).WithErrorMessage("'Content' must not be empty.");
    }
}
