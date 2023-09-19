using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Infrastructure.EF.Configurations;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(p => p.AuthorId);

        builder.Property(nameof(Article.Tags))
            .HasField("_tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(new ValueConverter<IReadOnlyList<string>, string>
            (
                v => string.Join(";", v), 
                v => v.Split(new[] { ';' }).ToList()
            ));
    }
}
