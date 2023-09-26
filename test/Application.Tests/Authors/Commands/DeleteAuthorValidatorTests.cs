using FluentValidation.TestHelper;

using SolutionTemplate.Application.Authors.Commands;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class DeleteAuthorValidatorTests
{
    private readonly DeleteAuthor.Validator _validator = new();

    [Fact]
    public void Validator_EmptyId_Should_ReturnError()
    {
        // Arrange
        var command = new DeleteAuthor.Command(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("'Id' must not be empty.");
    }
}