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

            if (response.IsFailure)
                _logger.Error(response.Error!);
            else
                _logger.LogResponse(response);

            return response;
        }
    }
}




public static partial class Log
{
    [LoggerMessage(2, LogLevel.Warning, "Error response {code}.")]
    public static partial void Error(this ILogger logger, string code);


    [LoggerMessage(4, LogLevel.Information, "Response: {Response}")]
    public static partial void LogResponse(this ILogger logger, object response);
}