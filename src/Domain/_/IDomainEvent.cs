using MediatR;

namespace SolutionTemplate.Domain._;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}

public abstract record DomainEvent(Guid Id) : IDomainEvent;
