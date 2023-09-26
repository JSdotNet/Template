using FluentValidation.TestHelper;

using SolutionTemplate.Application.Authors.Commands;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public class UpdateAuthorValidatorTests
{
    private readonly UpdateAuthor.Validator _validator = new();

    [Fact]
    public void Validator_EmptyId_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateAuthor.Command(Guid.Empty, "firstname", "lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id).WithErrorMessage("'Id' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyFirstName_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateAuthor.Command(Guid.NewGuid(), "", "lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Firstname).WithErrorMessage("'Firstname' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyLastName_Should_ReturnError()
    {
        // Arrange
        var command = new UpdateAuthor.Command(Guid.NewGuid(), "firstname", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Lastname).WithErrorMessage("'Lastname' must not be empty.");
    }
}
