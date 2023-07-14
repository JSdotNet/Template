using MediatR;

using SolutionTemplate.Domain._;

namespace SolutionTemplate.Application._.Messaging;



public interface IDomainEventNotificationHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}
