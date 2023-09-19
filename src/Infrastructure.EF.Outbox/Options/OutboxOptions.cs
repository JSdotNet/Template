namespace SolutionTemplate.Infrastructure.EF.Outbox.Options;

public sealed class OutboxOptions
{
    public int SimultaneousMessages { get; set; } = 20;

    public int MessageProcessorIntervalInSeconds { get; set; } = 10;

    public int MessageCleanupIntervalDays { get; set; } = 7;

    public int MessageRetentionInDays { get; set; } = 21;
}