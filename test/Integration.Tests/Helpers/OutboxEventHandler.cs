using MediatR;

using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Integration.Tests.Helpers;

internal sealed class OutboxEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    private readonly IDomainEventLogger _logger;

    public OutboxEventHandler(IDomainEventLogger logger) => _logger = logger;


    public Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        _logger.Log(notification);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}
