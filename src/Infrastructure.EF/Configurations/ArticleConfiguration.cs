using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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


        builder.OwnsMany(a => a.Tags, at =>
        {
            //at.WithOwner().HasForeignKey(nameof(ArticleId));
            at.HasKey(nameof(Tag.Name));
        });
    }
}
