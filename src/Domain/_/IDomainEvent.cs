using MediatR;

namespace SolutionTemplate.Domain._;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
}