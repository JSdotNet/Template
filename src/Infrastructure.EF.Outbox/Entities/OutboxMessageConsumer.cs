namespace SolutionTemplate.Infrastructure.EF.Outbox.Entities;

public sealed class OutboxMessageConsumer
{
    public Guid Id { get; init; }

    public string Name { get; init; } = default!;
}
