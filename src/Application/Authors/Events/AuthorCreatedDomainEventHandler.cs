using MediatR;

using Microsoft.Extensions.Logging;

using SolutionTemplate.Domain.Events;

namespace SolutionTemplate.Application.Authors.Events;

internal sealed class AuthorCreatedDomainEventHandler : INotificationHandler<AuthorCreatedDomainEvent>
{
    private readonly ILogger _logger;

    public AuthorCreatedDomainEventHandler(ILogger<AuthorCreatedDomainEventHandler> logger) => _logger = logger;


    public Task Handle(AuthorCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        _logger.LogInformation("Author Created: {ArticleId}", notification.AuthorId.Value);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}
