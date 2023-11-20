using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;


namespace SolutionTemplate.Application.Articles.Events;

internal sealed class ArticleCreatedDomainEventHandler(ILogger<ArticleCreatedDomainEventHandler> logger) : IDomainEventNotificationHandler<DomainEvents.ArticleCreated>
{
    public Task Handle(DomainEvents.ArticleCreated notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
        logger.ArticleCreated(notification.ArticleId);

        return Task.CompletedTask;
    }
}

public static partial class Log
{
    [LoggerMessage(1, LogLevel.Information, "Article Created: {ArticleId}")]
    public static partial void ArticleCreated(this ILogger logger, Guid articleId);
}
