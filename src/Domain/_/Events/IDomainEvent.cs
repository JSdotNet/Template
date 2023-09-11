using MediatR;

namespace SolutionTemplate.Domain._.Events;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}