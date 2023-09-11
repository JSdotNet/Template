namespace SolutionTemplate.Domain._.Audit;


public interface IHasAuditEvents : IHasId
{
    IReadOnlyList<IAuditEvent> AuditEvents { get; }

    //void Clear();
}


public sealed class AuditEventWrapper
{
    private readonly List<IAuditEvent> _auditEvents = new();
    public IReadOnlyList<IAuditEvent> Items => _auditEvents.AsReadOnly();

    internal void Raise(IAuditEvent domainEvent) => _auditEvents.Add(domainEvent);
    internal void Clear() => _auditEvents.Clear();
}