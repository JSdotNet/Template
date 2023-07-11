using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SolutionTemplate.Infrastructure.EF.Outbox;

namespace SolutionTemplate.Infrastructure.EF.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("Outbox");

        builder.HasKey(a => a.Id);
        builder.HasIndex(e => e.OccurredOnUtc);
    }
}
