﻿using Microsoft.EntityFrameworkCore;
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

        builder.HasMany<OutboxMessageConsumer>()
            .WithOne()
            .HasForeignKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}