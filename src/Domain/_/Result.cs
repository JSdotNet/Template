namespace SolutionTemplate.Domain._;

public record Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error = null)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Cannot create successful result with error.");

        if (!isSuccess && error is null)
            throw new InvalidOperationException("Cannot create failure result without error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

}


public record Result<TValue> : Result
{
    private readonly TValue? _value; // TODO Does record make sense if value can be a reference type?

    protected internal Result(TValue? value, bool isSuccess, Error? error = null) : base(isSuccess, error) 
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of failure result. Use 'IsSuccess' property to check if the operation was successful.");


    // TODO Review...
    public static implicit operator Result<TValue>(TValue value) => new(value, true); 

    public static implicit operator TValue(Result<TValue> result) => result.Value;
}