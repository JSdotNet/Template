using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF.Repository;


internal sealed class ArticleRepository(DataContext dataContext) : IArticleRepository
{
    public async ValueTask<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await dataContext.Articles.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

        return result;
    }

    public void Add(Article entity)
    {
        dataContext.Articles.Add(entity);
        // TODO Is it useful to return the EntityEntry?
    }

    public void Remove(Article entity)
    {
        dataContext.Articles.Remove(entity);
    }
}