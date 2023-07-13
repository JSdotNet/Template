using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Configurations;

internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable("OutboxConsumer");

        builder.HasKey(e => e.Id);
    }
}
