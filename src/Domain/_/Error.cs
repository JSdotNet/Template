namespace SolutionTemplate.Domain._;

#pragma warning disable CA1716
public readonly record struct Error(string Code, string Message, params object?[] Arguments)
#pragma warning restore CA1716
{
    public static implicit operator string(Error error) => error.Code;

    //public string FormattedMessage => string.Format(Message, Arguments);
}