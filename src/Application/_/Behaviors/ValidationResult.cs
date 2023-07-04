using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Behaviors;

public interface IValidationResult
{
    public static Error ValidationError => new("ValidationError", "A validation problem occured");

    Error[] Errors { get; }
}


public record ValidationResult : Result, IValidationResult
{
    public ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}


public record ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors) : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }


#pragma warning disable CA1000
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
#pragma warning restore CA1000
}