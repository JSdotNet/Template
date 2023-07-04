using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain;

namespace SolutionTemplate.Infrastructure.EF.Data;

internal sealed class ReadOnlyDataContext : IReadOnlyDataContext
{
    private readonly DataContext _dbContext;

    public ReadOnlyDataContext(DataContext dbContext)
    {
        // TODO Use readonly connectionString?
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return _dbContext.Set<TEntity>().AsNoTracking();
    }


    public IAsyncEnumerable<TEntity> GetAsyncEnumerable<TEntity>() where TEntity : class
    {
        return _dbContext.Set<TEntity>().AsNoTracking().AsAsyncEnumerable();
    }
}
