using System.Net;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using SolutionTemplate.Application._.Behaviors;

namespace SolutionTemplate.WebApi.Middleware;

internal sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }



    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";

        ProblemDetails problem = new();
        switch (exception)
        {
            case ValidationException:
                Log.ApplicationError(_logger, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problem.Title = "One or more validation errors occurred.";
                problem.Detail = exception.Message;
                break;
            case NotFoundException:
                Log.ApplicationError(_logger, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                problem.Title = "The specified resource was not found.";
                problem.Detail = exception.Message;
                break;
            case AlreadyExistsException:
                Log.ApplicationError(_logger, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                problem.Title = "The specified resource already exists.";
                problem.Detail = exception.Message;
                break;
            case DomainException:
                Log.ApplicationError(_logger, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problem.Type = nameof(DomainException);
                problem.Title = "The specified resource already exists.";
                problem.Detail = exception.Message;
                break;

            default:
                Log.UnExpectedError(_logger, exception);

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problem.Title = "An unhandled error occurred.";
                break;
        }

        problem.Status = httpContext.Response.StatusCode;

        var json = JsonConvert.SerializeObject(problem);

        await httpContext.Response.WriteAsync(json);
    }
}

public static partial class Log
{
    [LoggerMessage(0, LogLevel.Error, "Unexpected exception")]
    public static partial void UnExpectedError(ILogger logger, Exception exception);


    [LoggerMessage(1, LogLevel.Warning, "Functional exception")]
    public static partial void ApplicationError(ILogger logger, Exception exception);
}
