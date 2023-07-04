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

        if (response.IsFailure && response.Error == ApplicationErrors.NotFoundCode)
        {
            // Middleware should catch these exceptions and return a 404
            throw new NotFoundException(response.Error!.Value.Message);
        }

        if (response.IsFailure && response.Error == ApplicationErrors.AlreadyExistsCode)
        {
            // Middleware should catch these exceptions and return a 409
            throw new AlreadyExistsException(response.Error!.Value.Message);
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