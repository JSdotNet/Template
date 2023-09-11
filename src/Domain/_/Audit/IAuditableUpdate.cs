namespace SolutionTemplate.Domain._.Audit;

public interface IAuditableUpdate
{
    public DateTime UpdatedUtc { get; set; }
}
