namespace SolutionTemplate.WebApi.Middleware;

public static partial class Log
{
    [LoggerMessage(0, LogLevel.Error, "Unexpected exception")]
    public static partial void UnExpectedError(ILogger logger, Exception exception);


    [LoggerMessage(1, LogLevel.Warning, "Functional exception")]
    public static partial void ApplicationError(ILogger logger, Exception exception);
}
