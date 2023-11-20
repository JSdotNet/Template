using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Behaviors;
 
internal sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
#pragma warning disable CA1848 // TODO How do I solve this for BeginScope?
        using (_logger.BeginScope("{Request}, {User}", request, "Job"))
#pragma warning restore CA1848
        { 
            var response = await next();

            _logger.LogResponse(response);
            return response;
        }
    }
}




public static partial class Log
{
    [LoggerMessage(0, LogLevel.Information, "This is some Information with value {Test}")]
    public static partial void SomeInformation(this ILogger logger, int test);

    [LoggerMessage(1, LogLevel.Warning, "This is a Warning with value {Test}")]
    public static partial void SomeWarning(this ILogger logger, int test);

    [LoggerMessage(2, LogLevel.Error, "This is an Error")]
    public static partial void SomeError(this ILogger logger, Exception exception);

    [LoggerMessage(3, LogLevel.Error, "This Error is Critical")]
    public static partial void SomeCriticalError(this ILogger logger, Exception exception);

    [LoggerMessage(4, LogLevel.Information, "Response: {Response}")]
    public static partial void LogResponse(this ILogger logger, object response);
}