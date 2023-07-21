namespace SolutionTemplate.Infrastructure.EF.Outbox.Entities;

public sealed class OutboxMessageConsumer
{
    public Guid OutboxMessageId { get; init; }

    public string Name { get; init; } = default!;
}
