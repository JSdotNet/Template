using SolutionTemplate.Application._.Messaging;
using SolutionTemplate.Domain;


namespace SolutionTemplate.Application.Articles.Events;

internal sealed class ArticleCreatedDomainEventHandler(ILogger<ArticleCreatedDomainEventHandler> logger) : IDomainEventNotificationHandler<DomainEvents.ArticleCreated>
{

    public Task Handle(DomainEvents.ArticleCreated notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        logger.LogInformation("Article Created: {ArticleId}", notification.ArticleId);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}