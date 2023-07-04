using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SolutionTemplate.Domain.Models;

namespace SolutionTemplate.Infrastructure.EF.Configurations;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => new ArticleId(value));


        builder.Property(a => a.AuthorId)
            .HasConversion(
                id => id.Value,
                value => new AuthorId(value));

        builder.HasOne<Author>()
            .WithMany()
            .HasForeignKey(nameof(AuthorId)); 


        builder.OwnsMany(a => a.Tags, at =>
        {
            //at.WithOwner().HasForeignKey(nameof(ArticleId));
            at.HasKey(nameof(Tag.Name));
        });
    }
}
