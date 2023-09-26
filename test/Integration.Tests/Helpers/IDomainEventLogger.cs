using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Integration.Tests.Helpers;

internal interface IDomainEventLogger
{
    IList<DomainEvent> ConsumedEvents { get; }
    void Log(DomainEvent domainEvent);

    IEnumerable<TDomainEvent> Get<TDomainEvent>()
        where TDomainEvent : DomainEvent;
}
