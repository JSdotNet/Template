using Microsoft.EntityFrameworkCore;

using SolutionTemplate.Domain._;
using SolutionTemplate.Infrastructure.EF.Data;

namespace SolutionTemplate.Infrastructure.EF._;

internal abstract class RepositoryBase<TAggregate>
    where TAggregate : AggregateRoot
{
    protected DataContext DataContext { get; }
    private readonly DbSet<TAggregate> _dbSet;

    protected RepositoryBase(DataContext dataContext)
    {
        DataContext = dataContext;

        _dbSet = DataContext.Set<TAggregate>();
    }


    public async ValueTask<TAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _dbSet.SingleOrDefaultAsync(i => i.Id == id, cancellationToken);

        return result;
    }

    public async ValueTask<IReadOnlyList<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _dbSet.ToListAsync(cancellationToken);

        return result;
    }

    public IAsyncEnumerable<TAggregate> AsAsyncEnumerable() => _dbSet.AsAsyncEnumerable();

    public void Add(TAggregate entity)
    {
        _dbSet.Add(entity);
        // TODO Is it useful to return the EntityEntry?
    }

    public void Remove(TAggregate entity)
    {
        _dbSet.Remove(entity);
    }
}
