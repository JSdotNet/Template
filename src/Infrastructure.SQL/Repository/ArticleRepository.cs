using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain.Models;
using SolutionTemplate.Domain.Repository;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF.Repository;


internal sealed class ArticleRepository : IArticleRepository
{
    private readonly DataContext _dataContext;

    public ArticleRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }


    public async ValueTask<Article?> GetByIdAsync(ArticleId id, CancellationToken cancellationToken = default)
    {
        var result = await _dataContext.Articles.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

        return result;
    }

    public async ValueTask<IReadOnlyList<Article>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dataContext.Articles.ToListAsync(cancellationToken);

        return result;
    }

    public IAsyncEnumerable<Article> AsAsyncEnumerable(CancellationToken cancellationToken = default) => _dataContext.Articles.AsAsyncEnumerable();

    public void Add(Article entity)
    {
        _dataContext.Articles.Add(entity);
        // TODO Is it useful to return the EntityEntry?
    }

    public void Remove(Article entity)
    {
        _dataContext.Articles.Remove(entity);
    }
}