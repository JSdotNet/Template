
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Infrastructure.EF.Configurations;

internal sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.Email).IsUnique();
    }
}
