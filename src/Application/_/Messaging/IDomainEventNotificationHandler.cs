using MediatR;

using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Application._.Messaging;


public interface IDomainEventNotificationHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
