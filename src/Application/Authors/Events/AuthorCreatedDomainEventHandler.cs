using MediatR;

using SolutionTemplate.Domain;

namespace SolutionTemplate.Application.Authors.Events;

internal sealed class AuthorCreatedDomainEventHandler(ILogger<AuthorCreatedDomainEventHandler> logger) : INotificationHandler<DomainEvents.AuthorCreated>
{
    public Task Handle(DomainEvents.AuthorCreated notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
        logger.AuthorCreated(notification.AuthorId);

        return Task.CompletedTask;
    }
}

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Information, "Author Created: {AuthorId}")]
    public static partial void AuthorCreated(this ILogger logger, Guid authorId);
}
