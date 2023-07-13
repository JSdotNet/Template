using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Messaging;

public interface IDomainEventNotificationHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
