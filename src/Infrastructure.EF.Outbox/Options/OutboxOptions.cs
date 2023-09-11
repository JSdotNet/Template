namespace SolutionTemplate.Infrastructure.EF.Outbox.Options;

public sealed class OutboxOptions
{
    public int MessageProcessorIntervalInSeconds { get; set; } = 10;

    public int MessageCleanupIntervalDays { get; set; } = 7;

    public int MessageRetentionInDays { get; set; } = 21;
}