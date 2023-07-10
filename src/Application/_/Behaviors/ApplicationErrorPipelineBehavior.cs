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

        if (response.IsFailure && response.Error == ApplicationErrors.Code.NotFound.ToString())
        {
            // Middleware should catch these exceptions and return a 404
            throw new NotFoundException(response.Error!.Value.Message);
        }

        if (response.IsFailure && response.Error == ApplicationErrors.Code.AlreadyExists.ToString())
        {
            // Middleware should catch these exceptions and return a 409
            throw new AlreadyExistsException(response.Error!.Value.Message);
        }

        // Application errors above are mapped specifically, so that middleware can return the correct status code
        // Domain errors below are mapped to one response type
        if (response.IsFailure)
        {
            // Middleware should catch these exceptions and return a 400
            throw new DomainException(response.Error!.Value.Message);
        }

        return response;
    }

}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message) : base(message)
    {
    }
}


public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}