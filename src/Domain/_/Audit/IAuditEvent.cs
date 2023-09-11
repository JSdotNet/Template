namespace SolutionTemplate.Domain._.Audit;

public interface IAuditEvent
{
    public string EntityType { get; }
    public Guid EntityId { get; }

    public string Name { get; }

    public IEnumerable<string>? TrackedFields { get; }

    public IReadOnlyDictionary<string, object?>? CustomFields { get; }

    public Guid? CustomUserId { get; }

    public int? CustomOrganizationId { get; }
}
