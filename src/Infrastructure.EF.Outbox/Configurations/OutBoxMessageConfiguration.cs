using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SolutionTemplate.Infrastructure.EF.Outbox.Entities;

namespace SolutionTemplate.Infrastructure.EF.Outbox.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("Outbox");

        builder.HasKey(a => a.Id);
        builder.HasIndex(e => e.OccurredOnUtc);

        builder.Property(e => e.Type).IsRequired().HasMaxLength(400);
        builder.Property(e => e.Content).IsRequired().HasMaxLength(2000);
        builder.Property(e => e.Error);

        builder.HasMany<OutboxMessageConsumer>()
            .WithOne()
            .HasForeignKey(c => c.OutboxMessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}