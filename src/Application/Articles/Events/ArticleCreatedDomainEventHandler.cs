using MediatR;

using Microsoft.Extensions.Logging;

using SolutionTemplate.Domain;


namespace SolutionTemplate.Application.Articles.Events;

internal sealed class ArticleCreatedDomainEventHandler : INotificationHandler<DomainEvents.ArticleCreated>
{
    private readonly ILogger _logger;

    public ArticleCreatedDomainEventHandler(ILogger<ArticleCreatedDomainEventHandler> logger) => _logger = logger;


    public Task Handle(DomainEvents.ArticleCreated notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        _logger.LogInformation("Article Created: {ArticleId}", notification.ArticleId);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}