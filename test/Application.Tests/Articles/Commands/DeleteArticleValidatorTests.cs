using FluentValidation.TestHelper;

using SolutionTemplate.Application.Articles.Commands;

using Xunit;

namespace SolutionTemplate.Application.Tests.Articles.Commands;

public class DeleteArticleValidatorTests
{
    private readonly DeleteArticle.Validator _validator = new();

    [Fact]
    public void Validator_EmptyId_Should_ReturnError()
    {
        // Arrange
        var command = new DeleteArticle.Command(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("'Id' must not be empty.");
    }
}