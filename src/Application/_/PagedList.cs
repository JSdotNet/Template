using Microsoft.EntityFrameworkCore;

namespace SolutionTemplate.Application._;

public class PagedList<TResponse>
{
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public IReadOnlyList<TResponse> Items { get; }

    public PagedList(int page, int pageSize, int totalCount, int totalPages, IReadOnlyList<TResponse> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
        Items = items;
    }

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;


#pragma warning disable CA1000 // TODO Review coding rule
    public static async Task<PagedList<TResponse>> CreateAsync(IQueryable<TResponse> source, int page, int pageSize, CancellationToken cancellationToken)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);

        return new PagedList<TResponse>(page, pageSize, count, totalPages, items);
    }
#pragma warning restore CA1000
}
