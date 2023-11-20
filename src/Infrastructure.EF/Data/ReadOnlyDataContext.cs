using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain;

namespace SolutionTemplate.Infrastructure.EF.Data;

internal sealed class ReadOnlyDataContext(DataContext dbContext) : IReadOnlyDataContext
{
    // TODO Use readonly connectionString?

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return dbContext.Set<TEntity>().AsNoTracking();
    }


    public IAsyncEnumerable<TEntity> GetAsyncEnumerable<TEntity>() where TEntity : class
    {
        return dbContext.Set<TEntity>().AsNoTracking().AsAsyncEnumerable();
    }
}
