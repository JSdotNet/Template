using MediatR;

using SolutionTemplate.Domain;

namespace SolutionTemplate.Application.Authors.Events;

internal sealed class AuthorCreatedDomainEventHandler : INotificationHandler<DomainEvents.AuthorCreated>
{
    private readonly ILogger _logger;

    public AuthorCreatedDomainEventHandler(ILogger<AuthorCreatedDomainEventHandler> logger) => _logger = logger;


    public Task Handle(DomainEvents.AuthorCreated notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        _logger.LogInformation("Author Created: {AuthorId}", notification.AuthorId);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}
