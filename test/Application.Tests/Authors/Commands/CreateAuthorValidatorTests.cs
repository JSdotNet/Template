using FluentValidation.TestHelper;

using SolutionTemplate.Application.Authors.Commands;

namespace SolutionTemplate.Application.Tests.Authors.Commands;

public sealed class CreateAuthorValidatorTests
{
    private readonly CreateAuthor.Validator _validator = new();

    [Fact]
    public void Validator_EmptyEmail_Should_ReturnError()
    {
        // Arrange
        var command = new CreateAuthor.Command("", "Firstname", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage("'Email' must not be empty.");
    }


    [Fact]
    public void Validator_EmptyFirstName_Should_ReturnError()
    {
        // Arrange
        var command = new CreateAuthor.Command("Email", "", "Lastname");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Firstname).WithErrorMessage("'Firstname' must not be empty.");
    }

    [Fact]
    public void Validator_EmptyLastName_Should_ReturnError()
    {
        // Arrange
        var command = new CreateAuthor.Command( "Email", "Firstname", "");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Lastname).WithErrorMessage("'Lastname' must not be empty.");
    }
}


