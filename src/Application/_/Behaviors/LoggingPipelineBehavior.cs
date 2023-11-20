using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Behaviors;
 
internal sealed class LoggingPipelineBehavior<TRequest, TResponse>(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{   
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
#pragma warning disable CA1848 // TODO How do I solve this for BeginScope?
        using (logger.BeginScope("{Request}, {User}", request, "Job"))
#pragma warning restore CA1848
        { 
            var response = await next();

            if (response.IsFailure)
                logger.Error(response.Error!);
            else
                logger.LogResponse(response);

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