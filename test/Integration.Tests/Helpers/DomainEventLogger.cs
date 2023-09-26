using SolutionTemplate.Domain._.Events;

namespace SolutionTemplate.Integration.Tests.Helpers;

internal sealed class DomainEventLogger : IDomainEventLogger
{
    public IList<DomainEvent> ConsumedEvents { get; } = new List<DomainEvent>();




    public void Log(DomainEvent domainEvent)
    {
        ConsumedEvents.Add(domainEvent);
    }

    public IEnumerable<TDomainEvent> Get<TDomainEvent>() 
        where TDomainEvent : DomainEvent
    {
        return ConsumedEvents.OfType<TDomainEvent>();
    }
}
