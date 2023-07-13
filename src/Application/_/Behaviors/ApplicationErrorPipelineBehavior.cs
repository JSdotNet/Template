using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Behaviors;

internal sealed class ApplicationErrorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (response.IsFailure && Enum.TryParse<ApplicationErrors.Code>(response.Error!.Value.Code, out var code))
        {
            // Middleware should catch these exceptions and return a 404
            throw new ApplicationException(code, response.Error!.Value.Message);
        }
        
        // Application errors above are mapped specifically, so that middleware can return the correct status code
        // Domain errors below are mapped to one response type
        if (response.IsFailure)
        {
            // Middleware should catch these exceptions and return a 400
            throw new DomainException(response.Error!.Value);
        }

        return response;
    }

}

public sealed class ApplicationException : Exception
{
    public ApplicationErrors.Code Code { get; }

    public ApplicationException(ApplicationErrors.Code code, string message) : base(message)
    {
        Code = code;
    }
}


public sealed class DomainException : Exception
{
    public string Code { get; }

    public DomainException(Error error) : base(error.Message)
    {
        Code = error.Code;
    }
}