using MediatR;

using Microsoft.Extensions.Logging;

using SolutionTemplate.Domain.Events;

namespace SolutionTemplate.Application.Articles.Events;

internal sealed class ArticleCreatedDomainEventHandler : INotificationHandler<ArticleCreatedDomainEvent>
{
    private readonly ILogger _logger;

    public ArticleCreatedDomainEventHandler(ILogger<ArticleCreatedDomainEventHandler> logger) => _logger = logger;


    public Task Handle(ArticleCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        _logger.LogInformation("Article Created: {ArticleId}", notification.ArticleId.Value);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}