namespace SolutionTemplate.Domain._.Audit;

public interface IAuditableCreate
{
    public DateTime CreatedUtc { get; set; }
}
