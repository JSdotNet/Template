using MediatR;

using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Integration.Tests.Helpers;

internal sealed class OutboxEventHandler<TDomainEvent>(IDomainEventLogger logger) : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    public Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        // This is only a sample. Normally you would do something useful here, like send an Email or ...
#pragma warning disable CA1848
        logger.Log(notification);
#pragma warning restore CA1848

        return Task.CompletedTask;
    }
}
